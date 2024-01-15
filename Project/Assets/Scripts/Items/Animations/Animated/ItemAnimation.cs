using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;
using System;

namespace com.limphus.extraction_shooter
{
    public class ItemAnimation : AnimationHandler
    {
        protected const string IDLE = "idle";
        protected const string RUNNING = "running";
        protected const string EQUIP = "equip";
        protected const string DEEQUIP = "deequip";

        protected const string IS_MOVING = "isMoving";
        protected const string IS_WALKING = "isWalking";
        protected const string IS_RUNNING = "isRunning";
        protected const string IS_CROUCHING = "isCrouching";

        public void PlayIdle() => PlayAnimation(IDLE);
        public void PlayRunning() => PlayAnimation(RUNNING);
        public void PlayEquip() => PlayAnimation(EQUIP);
        public void PlayDeEquip() => PlayAnimation(DEEQUIP);
    }
}