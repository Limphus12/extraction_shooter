using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public class FirearmSFX : SoundHandler
    {
        [Header("Attributes - Firearm SFX")]
        [Space]
        [SerializeField] protected AudioClip firingClip;
        [SerializeField] protected AudioClip reloadingClip;

        public void PlayFireSound()
        {
            PlayOneShotSound(firingClip, GameManager.Player.transform.position, 25f);
        }

        public void PlayReloadSound()
        {
            PlayOneShotSound(reloadingClip, GameManager.Player.transform.position, 25f);
        }
    }
}