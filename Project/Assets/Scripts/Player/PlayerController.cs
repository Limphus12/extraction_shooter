using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float runSpeed = 6.0f, crouchSpeed = 1.0f, jumpSpeed = 5.0f, gravity = 20.0f, antiBumpAmount = 40.0f;

        [Space]
        [SerializeField] private float speedSmoothRate;

        //[Header("Falling Settings")]
        //[SerializeField] private float minFallDistance;
        //[SerializeField] private AnimationCurve fallDamageCurve;

        [Header("Camera Settings")]
        [SerializeField] private float lookSpeed = 1.0f;
        [SerializeField] private float lookXLimit = 90.0f;

        [Space]
        [SerializeField] private bool cameraLean = true;
        [SerializeField] private float cameraLeanAmount = 4.0f;

        [Space]
        [SerializeField] private Transform playerCamera;

        [Header("Stance Settings")]
        [SerializeField] private float standingHeight = 2.0f;
        [SerializeField] private float crouchingHeight = 1.0f;

        [Space]
        [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
        [SerializeField] private Vector3 crouchingCenter = new Vector3(0, -0.5f, 0), standingCameraPosition = new Vector3(0, 0.5f, 0), crouchingCameraPosition = new Vector3(0, 0, 0);

        [Space]
        [SerializeField] private float cameraSmoothRate = 5f;

        [Header("Input Settings")]
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

        [Header("Debug Settings")]
        [SerializeField] private bool debug;

        public static bool canMove = true, canRotate = true, canCameraLean = true;

        public CharacterController CharacterController { get; private set; }
        private Vector3 moveDirection = Vector3.zero;
        private float rotationX = 0, originalStepOffset, currentSpeed, currentCeilingRaycast, currentGroundRaycast;
        private bool isJumping, isCoyoteTime;

        public event EventHandler<Events.OnBoolChangedEventArgs> OnMoveChanged, OnRunChanged, OnCrouchChanged, OnGroundedChanged;
        public event EventHandler<EventArgs> OnJump;

        public bool CanRun { get; set; }



        public bool IsCrouching { get; private set; }
        public bool IsRunning { get; private set; }
        public float CurrentHeight { get; private set; }
        public bool Grounded { get; private set; }
        public bool WasGrounded { get; private set; }
        public bool IsMoving { get; private set; }

        private void Awake() => Init();

        private void Init()
        {
            //Grabs the CharacterController from the player object
            if (!CharacterController) CharacterController = GetComponent<CharacterController>();

            //Lock Cursor - replace with a player manager later on?
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Set the original step offset - used for a bugfix
            originalStepOffset = CharacterController.stepOffset;
        }

        //Update is called once per frame
        void Update() => Inputs();

        void Inputs()
        {
            //if we're not pressing the run key, then walk (sets our speed to either crouch speed or walk speed)
            if (!Input.GetKey(runKey))
            {
                Walk();
            }

            //else if we're only pressing the run key and we have the space to stand (just incase we go from crouching to running)
            //btw not sure if we actually need to do the check? idk. EDIT, NO CEILING CHECK, AS WE CAN RUN WHILST CROUCHING
            else if (Input.GetKey(runKey) && CanRun) //add a stamina check later on (DONE)
            {
                Run();
            }

            //if were not pressing the crouch key, and we have the room to stand
            if (!Input.GetKey(crouchKey) && !HitCeiling() || IsRunning)
            {
                Stand();
            }

            //else if we're pressing the crouch key (and we're not running)
            else if (Input.GetKey(crouchKey) && !IsRunning)
            {
                Crouch();
            }

            CalculateMovement();
        }

        #region Movement

        private float curSpeedX, curSpeedZ;

        //Calculates Player Movement
        void CalculateMovement()
        {
            //Recalculate move direction based on axes, as we are grounded.
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            //calculate current speed in the Horizontal and Vertical directions (WASD)
            curSpeedX = currentSpeed * Input.GetAxis("Horizontal");
            curSpeedZ = currentSpeed * Input.GetAxis("Vertical");

            if (curSpeedX == 0 && curSpeedZ == 0) IsMoving = false;
            else IsMoving = true;

            OnMoveChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = IsMoving });

            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedZ) + (right * curSpeedX);

            //isGrounded is checked first to ensure the y velocity is reset when the player
            //hits the ground. After that, check for the player input, and jump if so.
            if (CharacterController.isGrounded)
            {
                CharacterController.stepOffset = originalStepOffset;

                if (HitGround())
                {
                    //fixes a stutter issue when we're going down small slopes
                    moveDirection.y = -antiBumpAmount;
                }

                //jumping!
                if (Input.GetButton("Jump") && canMove)
                {
                    moveDirection.y = jumpSpeed;

                    OnJump?.Invoke(this, EventArgs.Empty);
                }
            }

            else
            {
                moveDirection.y = movementDirectionY;
            }

            //Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            //when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            //as an acceleration (ms^-2)
            if (!CharacterController.isGrounded)
            { 
                //if we hit the ceiling when jumping, cancel our vertical velocity.
                if (HitCeiling())
                {
                    moveDirection.y = -0.1f;
                }

                moveDirection.y -= gravity * Time.deltaTime;

                CharacterController.stepOffset = 0f; //Fixes a bug where jumping against something that, if will end up at your step height during the jump, it would suddenly put you back on the ground. 
            }

            CalculateFall();

            Move();
        }

        float fallStartPosY;

        private void CalculateFall()
        {
            bool b = HitGround();

            //if we were not grounded last frame
            if (!WasGrounded && Grounded)
            {
                //OLD FALL DAMAGE CALCS

                //calculate the distance we fell
                //float distanceFell = fallStartPosY - transform.position.y;
                //
                ////if we fell farther than the minimum fall distance
                //if (distanceFell > minFallDistance)
                //{
                //    //calculate fall damage
                //    distanceFell -= minFallDistance;
                //
                //    //rounding to the nearest int
                //    int damage = Mathf.RoundToInt(fallDamageCurve.Evaluate(distanceFell));
                //
                //    playerStats.Damage(damage);
                //}
            }

            //else if we were grounded last frame
            else if (WasGrounded && !Grounded)
            {
                //set our starting y position
                fallStartPosY = transform.position.y;
            }
            
            WasGrounded = Grounded;
        }

        //Deals with Movement
        void Move()
        {
            //Move the controller
            if (canMove) CharacterController.Move(moveDirection * Time.deltaTime);

            //Player and Camera rotation
            if (canRotate)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

                if (!cameraLean)
                {
                    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                }

                //camera lean, sets the camera's Z rotation based off horizontal input
                else if (cameraLean && canCameraLean)
                {
                    float currentX = Input.GetAxis("LeanHorizontal");
                    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, currentX * -cameraLeanAmount);
                }

                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }

        #endregion

        #region Stance Methods

        void Crouch()
        {
            IsCrouching = true;

            ChangeStance(crouchingHeight, crouchingCenter, crouchingCameraPosition);
            ChangeSpeed(crouchSpeed);

            OnCrouchChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = IsCrouching });
        }

        void Stand()
        {
            IsCrouching = false;

            ChangeStance(standingHeight, standingCenter, standingCameraPosition);

            OnCrouchChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = IsCrouching });
        }

        void Run()
        {
            IsRunning = true;

            //if we're not crouching, use our run speed
            if (!IsCrouching)
            {
                ChangeSpeed(runSpeed);
            }

            //if we're crouching, use our crouch run speed (like project zomboid lmao).
            //EDIT: No, we just gonna un-crouch and run
            else if (IsCrouching)
            {
                IsCrouching = false;

                ChangeSpeed(runSpeed);
            }

            OnRunChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = IsRunning });
        }

        void Walk()
        {
            IsRunning = false;

            //if we're not crouching, use our walk speed
            if (!IsCrouching)
            {
                ChangeSpeed(walkSpeed);
            }

            //if we're crouching, use our crouch speed
            else if (IsCrouching)
            {
                ChangeSpeed(crouchSpeed);
            }

            OnRunChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = IsRunning });
        }

        private Vector3 previousCameraPos;

        private float stanceI = 0f;

        void ChangeStance(float height, Vector3 center, Vector3 cameraPos)
        {
            //If our previous pos is not our current inputed pos, reset stanceI to 0
            if (previousCameraPos != cameraPos)
            {
                previousCameraPos = cameraPos;
                stanceI = 0f;
            }

            CharacterController.height = height;
            CharacterController.center = center;

            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, cameraPos, (stanceI + Time.deltaTime) * cameraSmoothRate);

            //calculate current raycasts used in determining if we can either stand or for when we hit the ceiling whilst jumping
            //as well as the ground check stuff for the antibump
            if (!IsCrouching)
            {
                currentCeilingRaycast = standingHeight / 2 + 0.1f;
                currentGroundRaycast = standingHeight / 2 + 0.25f;
            }

            else if (IsCrouching)
            {
                currentCeilingRaycast = crouchingHeight / 2 + 0.1f;
                currentGroundRaycast = crouchingHeight / 2 + 0.25f;
            }

            CurrentHeight = height;
        }

        #endregion

        #region HitChecks

        bool HitCeiling()
        {
            //raycast upwards from our center
            if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit, currentCeilingRaycast))
            {
                if (hit.transform != transform) return true;

                else return false;
            }

            else return false;
        }

        private bool previousGrounded;

        bool HitGround()
        {
            //raycast downwards from our center
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, currentGroundRaycast))
            {
                if (hit.transform != transform) Grounded = true;

                else Grounded = false;
            }

            else Grounded = false;

            if (previousGrounded != Grounded)
            {
                OnGroundedChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = Grounded });

                previousGrounded = Grounded;
            }

            return Grounded;
        }

        #endregion

        #region Speed Methods

        private float previousSpeed, speedI = 0f;

        [Tooltip("Value between 0 and 1; 0 is no speed, 1 is full speed")]
        [Range(0f, 1f)] private float speedPercentage = 1f;

        public float GetSpeedPercentage()
        {
            return speedPercentage;
        }

        public void SetSpeedPercentage(int i) => speedPercentage = i;

        public void ResetSpeedPercentage() => speedPercentage = 1;

        void ChangeSpeed(float speed)
        {
            //if our previous speed is not our current inputed speed, reset speedI to 0
            if (previousSpeed != speed)
            {
                previousSpeed = speed;
                speedI = 0f;
            }

            //calculate our current speed by lerping between the new speed and current speed, and divided by our speed %
            currentSpeed = Mathf.Lerp(currentSpeed, speed, (speedI + Time.deltaTime) * speedSmoothRate) / speedPercentage;
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (debug)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.up * currentCeilingRaycast);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, -transform.up * currentGroundRaycast);
            }
        }
    }

    public struct PlayerControllerInputs 
    {
        public Vector2 vertical, horizontal, cameraLean;

        public bool jump, crouch, run;
    }
}