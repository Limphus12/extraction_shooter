using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.procedural_animation
{
    public class FirearmSway : ItemSway
    {
        [Header("Attributes - Weapon Aiming Settings")]
        [SerializeField] protected ItemSwayStruct aimingData;

        [Header("Attributes - Firearm Reloading Settings")]
        [SerializeField] protected ItemSwayStruct reloadingData;

        [Header("Attributes - Firearm Cocking Settings")]
        [SerializeField] protected ItemSwayStruct cockingData;

        [Header("Attributes - Firearm Aim Cocking Settings")]
        [SerializeField] protected ItemSwayStruct aimCockingData;

        protected bool isAiming, isReloading, isCocking;

        public void Reload(bool b) => isReloading = b; public void Cock(bool b) => isCocking = b; public void Aim(bool b) => isAiming = b;

        protected override void CheckSwayAndTilt()
        {
            if (isRunning) SwayAndTilt(runningData);

            else if (isReloading) SwayAndTilt(reloadingData);

            else if (isCocking)
            {
                if (!isAiming) SwayAndTilt(cockingData);
                else if (isAiming) SwayAndTilt(aimCockingData);
            }

            else if (!isAiming && !isCocking) SwayAndTilt(idleData);

            else if (isAiming) SwayAndTilt(aimingData);
        }
    }
}