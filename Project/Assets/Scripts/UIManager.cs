using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace com.limphus.extraction_shooter
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactionText;

        private InteractionScript interaction;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            //grab references
            interaction = GameManager.Player.GetComponent<InteractionScript>();

            //sub to events
            interaction.OnInteractionCheck += Interaction_OnInteractionCheck;
        }

        private void Interaction_OnInteractionCheck(object sender, InteractionScript.OnInteractEventArgs e)
        {
            if (e.i) interactionText.text = "E"; else interactionText.text = null;
        }
    }
}