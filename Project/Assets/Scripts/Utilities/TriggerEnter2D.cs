using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.utilities
{
    public class TriggerEnter2D : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerEnterEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            onTriggerEnterEvent?.Invoke();
        }
    }
}