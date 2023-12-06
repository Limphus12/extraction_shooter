using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.utilities
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onTriggerEnterEvent, onTriggerExitEvent;

        protected virtual void OnTriggerEnter(Collider other)
        {
            onTriggerEnterEvent?.Invoke();
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            onTriggerExitEvent?.Invoke();
        }
    }
}