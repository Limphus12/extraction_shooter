using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.utilities
{
    public class Trigger2D : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerEnterEvent, onTriggerExitEvent;

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            onTriggerEnterEvent?.Invoke();
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            onTriggerExitEvent?.Invoke();
        }
    }
}