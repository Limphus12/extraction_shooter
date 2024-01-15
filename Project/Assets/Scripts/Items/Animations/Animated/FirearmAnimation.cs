using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public class FirearmAnimation : ItemAnimation
    {
        [Space, SerializeField] protected Firearm firearm;

        const string IS_AIMING = "isAiming";
        const string IS_FIRING = "isFiring";
        const string IS_RELOADING = "isReloading";
        const string FIRE = "Fire";
        const string PARTIAL_RELOAD = "PartialReload";
        const string FULL_RELOAD = "FullReload";

        protected override void Init()
        {
            if (!firearm) return;

            firearm.OnAim += Firearm_OnAim;

            firearm.OnStartAttack += Firearm_OnStartAttack;
            firearm.OnEndAttack += Firearm_OnEndAttack;

            firearm.OnStartReload += Firearm_OnStartReload;
            firearm.OnEndReload += Firearm_OnEndReload;

            GameManager.PlayerController.OnMoveChanged += PlayerController_OnMoveChanged;
            GameManager.PlayerController.OnRunChanged += PlayerController_OnRunChanged;
            GameManager.PlayerController.OnCrouchChanged += PlayerController_OnCrouchChanged;

            //GameManager.PlayerController.OnJump += PlayerController_OnJump;
            //GameManager.PlayerController.OnGroundedChanged += PlayerController_OnGroundedChanged;
        }

        //private void PlayerController_OnGroundedChanged(object sender, Events.OnBoolChangedEventArgs e)
        //{
        //    SetParamater(IS_GROUNDED, e.i);
        //
        //    if (e.i)
        //    {
        //        SetTrigger(JUMP, false); //basically, if we land we'll reset this trigger
        //    
        //        Debug.Log("landing!!");
        //    }
        //}
        //private void PlayerController_OnJump(object sender, System.EventArgs e)
        //{
        //    SetTrigger(JUMP, true);
        //}

        private void PlayerController_OnCrouchChanged(object sender, Events.OnBoolChangedEventArgs e)
        {
            SetParamater(IS_CROUCHING, e.i);
        }

        private void PlayerController_OnRunChanged(object sender, Events.OnBoolChangedEventArgs e)
        {
            SetParamater(IS_RUNNING, e.i);
        }

        private void PlayerController_OnMoveChanged(object sender, Events.OnBoolChangedEventArgs e)
        {
            SetParamater(IS_MOVING, e.i);
        }

        private void Firearm_OnStartReload(object sender, Events.OnBoolChangedEventArgs e)
        {
            if (e.i) SetTrigger(PARTIAL_RELOAD, true);
            else if (!e.i) SetTrigger(FULL_RELOAD, true);

            SetParamater(IS_RELOADING, true);
        }

        private void Firearm_OnEndReload(object sender, System.EventArgs e)
        {
            SetTrigger(PARTIAL_RELOAD, false); SetTrigger(FULL_RELOAD, false);

            SetParamater(IS_RELOADING, false);
        }

        private void Firearm_OnAim(object sender, Events.OnBoolChangedEventArgs e)
        {
            SetParamater(IS_AIMING, e.i);
        }

        private void Firearm_OnStartAttack(object sender, System.EventArgs e)
        {
            SetParamater(IS_FIRING, true);
            SetTrigger(FIRE, true);
        }

        private void Firearm_OnEndAttack(object sender, System.EventArgs e)
        {
            SetParamater(IS_FIRING, false);
            SetTrigger(FIRE, false);
        }
    }
}