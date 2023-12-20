using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class PickupScript : MonoBehaviour
    {
        [SerializeField] private Transform pickupPoint;

        [SerializeField] private float moveForce = 5f, raycastLength = 10f;

        private Transform playerCamera;
        private Rigidbody heldObject;

        bool pickup;

        private void Awake()
        {
            playerCamera = GameManager.PlayerCamera.transform;
        }

        private void LateUpdate()
        {
            Inputs(); CheckPickup();
        }

        private void FixedUpdate()
        {
            MovePickup();
        }

        private void Inputs()
        {
            if (!heldObject && pickup) ResetPickup(); //if we don't have a held object (because it got deleted, for instance), then call the resetpickup function

            if (Input.GetMouseButtonDown(2)) pickup = !pickup;
            
            if (Input.GetMouseButtonUp(2) && !heldObject) ResetPickup();
        }

        void CheckPickup()
        {
            //if we're already picking up the object, then return
            if (pickup && heldObject) return;

            //if we're trying to pick up and we have no held object
            if (pickup && !heldObject)
            {
                //try to pick up the object, using a raycast
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, 10f))
                {
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

                    if (rb != null && rb.isKinematic == false)
                    {
                        //pick up the object!
                        heldObject = rb;
                        heldObject.useGravity = false;
                        heldObject.drag = 10;
                        heldObject.constraints = RigidbodyConstraints.FreezeRotation;

                        return;
                    }

                    rb = hit.collider.GetComponentInParent<Rigidbody>();

                    if (rb != null && rb.isKinematic == false)
                    {
                        //pick up the object!
                        heldObject = rb;
                        heldObject.useGravity = false;
                        heldObject.drag = 10;
                        heldObject.constraints = RigidbodyConstraints.FreezeRotation;

                        return;
                    }
                }
            }

            //if we're turning off the pickup and we have a held object
            else if (!pickup && heldObject)
            {
                DropPickup();
            }
        }

        void ResetPickup()
        {
            pickup = false;
        }

        void DropPickup()
        {
            //place down the object!
            heldObject.useGravity = true;
            heldObject.drag = 1;
            heldObject.constraints = RigidbodyConstraints.None;

            heldObject = null;
        }

        void MovePickup()
        {
            // Move the held object towards the hold position using Lerp
            if (heldObject != null)
            {
                if (Vector3.Distance(heldObject.transform.position, pickupPoint.position) > 0.1f)
                {
                    Vector3 moveDirection = (pickupPoint.position - heldObject.transform.position);
                    heldObject.AddForce(1000 * moveForce * Time.fixedDeltaTime * moveDirection);
                }
            }
        }
    }
}