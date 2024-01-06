using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace com.limphus.extraction_shooter
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactionText, ammoText;
        [SerializeField] private GameObject crosshairUI;

        private InteractionScript interaction;

        private Firearm firearm;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            //grab references
            interaction = GameManager.Player.GetComponent<InteractionScript>();
            firearm = GameManager.Player.GetComponentInChildren<Firearm>();

            //sub to events
            interaction.OnInteractionCheck += Interaction_OnInteractionCheck;
            firearm.OnAmmoChanged += Firearm_OnAmmoChanged;
            firearm.OnAim += Firearm_OnAim;

        }

        private void Firearm_OnAim(object sender, utilities.Events.OnBoolChangedEventArgs e)
        {
            if (crosshairUI) crosshairUI.SetActive(!e.i);
        }

        private void Firearm_OnAmmoChanged(object sender, utilities.Events.OnIntChangedEventArgs e)
        {
            if (ammoText) ammoText.text = e.i.ToString();
        }

        private void Interaction_OnInteractionCheck(object sender, InteractionScript.OnInteractEventArgs e)
        {
            if (e.i) interactionText.text = "E"; else interactionText.text = null;
        }
    }
}