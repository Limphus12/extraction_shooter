using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public class Firearm : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private float reloadTime, rof;

        //private WeaponRecoil weaponRecoil;
        //private WeaponRecoil cameraRecoil;

        //private FirearmSound firearmSound;

        //private FirearmSway firearmSway;
        //private FirearmAnimation firearmAnimation;
        //private FirearmFX firearmFX;

        //private FirearmFunctionAnimation firearmFunctionAnimation;

        private bool isFiring;

        private Transform playerCameraTransform;

        //event examples
        //public event EventHandler<Events.OnIntChangedEventArgs> OnHealthChanged;
        //public event EventHandler<EventArgs> OnHealthDepleted, OnHealthReplenished;

        public event EventHandler<EventArgs> OnStartFire, OnFire, OnEndFire;
        public event EventHandler<Events.OnRaycastHitEventArgs> OnEnvHit, OnEnemyHit;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (!playerCameraTransform) playerCameraTransform = GameManager.PlayerCamera.transform;
        }

        private void Start()
        {

        }

        private void Update()
        {
            Inputs();
        }

        private void Inputs()
        {
            //if we press the rmb, and we're not already firing
            if (Input.GetMouseButtonDown(0) && !isFiring)
            {
                StartFire();
            }
        }

        private void StartFire()
        {
            isFiring = true;

            OnStartFire?.Invoke(this, new EventArgs { });

            Fire();
        }

        private void Fire()
        {
            OnFire?.Invoke(this, new EventArgs { });

            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                //check if we hit an enemy or not
                    //if we did, fire off an event saying so
                    //oh, and do damage stuff ig

                //otherwise if we didn't
                    //just fire an event saying that we didn't!
            }

            Invoke(nameof(EndFire), 1f / rof);
        }

        private void EndFire()
        {
            OnEndFire?.Invoke(this, new EventArgs { });

            isFiring = false;
        }
    }
}