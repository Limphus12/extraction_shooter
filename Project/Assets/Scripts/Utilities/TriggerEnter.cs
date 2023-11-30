using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.utilities
{
    public class TriggerEnter : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onTriggerEnterEvent;

        protected virtual void OnTriggerEnter(Collider other)
        {
            onTriggerEnterEvent?.Invoke();
        }
    }
}