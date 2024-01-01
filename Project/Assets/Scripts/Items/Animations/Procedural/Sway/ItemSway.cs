using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.procedural_animation
{
    public struct ItemSwayStruct
    {
        public Vector3 position; public Quaternion rotation;

        public float swayAmount, swayMaximum, swaySmooth;
        public float tiltAmount, tiltMaximum, tiltSmooth;
    }

    public class ItemSway : MonoBehaviour
    {
        [SerializeField] protected ItemSwayStruct idleData;

        [Space]
        [SerializeField] protected ItemSwayStruct runningData;

        protected Quaternion initialRotation;

        protected bool isRunning;

        public void Run(bool b) => isRunning = b;

        protected virtual void Awake()
        {
            initialRotation = idleData.rotation;
        }

        protected virtual void Update() 
        {
            CheckSwayAndTilt();
        }

        protected virtual void CheckSwayAndTilt()
        {
            if (isRunning) SwayAndTilt(runningData);
            else SwayAndTilt(idleData);
        }

        protected void SwayAndTilt(ItemSwayStruct data)
        {
            //calculate inputs
            Vector2 inputs = Inputs();

            //calculate sway positions
            Vector2 swayPositions = new Vector2(Mathf.Clamp(inputs.x * data.swayAmount, -data.swayMaximum, data.swayMaximum) / 100, Mathf.Clamp(inputs.y * data.swayAmount, -data.swayMaximum, data.swayMaximum) / 100);

            //calculate final target position
            Vector3 targetPosition = new Vector3(swayPositions.x, swayPositions.y);

            //apply target position
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition + data.position, data.swaySmooth * Time.deltaTime);

            //calculate tilt rotations
            Vector3 tiltRotations = new Vector3(Mathf.Clamp(inputs.x * data.tiltAmount, -data.tiltMaximum, data.tiltMaximum), Mathf.Clamp(inputs.y * data.tiltAmount, -data.tiltMaximum, data.tiltMaximum), Mathf.Clamp(Input.GetAxis("Horizontal") * data.tiltAmount, -data.tiltMaximum, data.tiltMaximum));

            //calculate target rotation
            Quaternion targetRotation = Quaternion.Euler(new Vector3(tiltRotations.x, -tiltRotations.y, -tiltRotations.z)) * data.rotation;

            //apply target rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation * initialRotation, data.tiltSmooth * Time.deltaTime);
        }

        private Vector2 Inputs()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }
}