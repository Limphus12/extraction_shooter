using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.extraction_shooter
{
    public class Button : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent buttonEvent;

        public void Interact()
        {
            Debug.Log("Interacting!");

            buttonEvent?.Invoke();
        }
    }
}