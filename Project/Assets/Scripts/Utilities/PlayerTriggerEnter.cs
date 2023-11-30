using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.utilities
{
    public class PlayerTriggerEnter : TriggerEnter
    {
        [SerializeField] private string playerTag = "Player";

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                onTriggerEnterEvent?.Invoke();
            }
        }
    }
}