using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactionText, ammoText, waveText;
        [SerializeField] private GameObject crosshairUI;
        [SerializeField] private Animator hitmarkerAnim;

        private InteractionScript interaction;

        private Firearm firearm;

        private WaveSystem waveSystem;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            //grab references
            interaction = GameManager.Player.GetComponent<InteractionScript>();
            firearm = GameManager.Player.GetComponentInChildren<Firearm>();
            waveSystem = GameObject.FindObjectOfType<WaveSystem>();

            //sub to events
            interaction.OnInteractionCheck += Interaction_OnInteractionCheck;
            firearm.OnAmmoChanged += Firearm_OnAmmoChanged;
            firearm.OnAim += Firearm_OnAim;
            firearm.OnEnemyHit += Firearm_OnEnemyHit;
            waveSystem.WaveChanged += WaveSystem_OnWaveChanged;
        }

        private void Firearm_OnEnemyHit(object sender, Events.OnRaycastHitEventArgs e)
        {
            if (hitmarkerAnim) hitmarkerAnim.SetTrigger("Hit");
        }

        private void WaveSystem_OnWaveChanged(object sender, Events.OnIntChangedEventArgs e)
        {
            if (waveText) waveText.text = "Wave: " + e.i;
        }

        private void Firearm_OnAim(object sender, Events.OnBoolChangedEventArgs e)
        {
            if (crosshairUI) crosshairUI.SetActive(!e.i);
        }

        private void Firearm_OnAmmoChanged(object sender, Events.OnIntChangedEventArgs e)
        {
            if (ammoText) ammoText.text = "Ammo: " + e.i.ToString();
        }

        private void Interaction_OnInteractionCheck(object sender, InteractionScript.OnInteractEventArgs e)
        {
            if (e.i) interactionText.text = "E"; else interactionText.text = null;
        }
    }
}