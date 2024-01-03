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
        const string RELOAD = "Reload";

        protected override void Init()
        {
            if (!firearm) return;

            firearm.OnAim += Firearm_OnAim;

            firearm.OnStartAttack += Firearm_OnStartAttack;
            firearm.OnEndAttack += Firearm_OnEndAttack;

            firearm.OnStartReload += Firearm_OnStartReload;
            firearm.OnEndReload += Firearm_OnEndReload;
        }

        private void Firearm_OnStartReload(object sender, System.EventArgs e)
        {
            SetTrigger(RELOAD, true);
            SetParamater(IS_RELOADING, true);
        }

        private void Firearm_OnEndReload(object sender, System.EventArgs e)
        {
            SetTrigger(RELOAD, false);
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