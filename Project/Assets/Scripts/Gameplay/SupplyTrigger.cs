using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public class SupplyTrigger : Trigger
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Supply"))
            {
                onTriggerEnterEvent?.Invoke();
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Supply"))
            {
                onTriggerExitEvent?.Invoke();
            }
        }
    }
}