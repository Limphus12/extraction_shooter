using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.limphus.extraction_shooter
{
    public class ExtractionManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent extractionEvent;

        [Space, SerializeField] private float timeToExtract = 60f;

        private bool isExtracting = false;

        private float timer;

        public void StartExtract()
        {
            if (isExtracting) return;

            isExtracting = true;

            timer = timeToExtract;

            //start our lil timer!
            StartCoroutine(ExtractionTimer());
        }

        IEnumerator ExtractionTimer()
        {
            //loop whilst our timer is not below 0
            while (timer > 0)
            {
                timer -= Time.deltaTime;

                //Debug.Log(Mathf.RoundToInt(timer));

                yield return null;
            }

            Extract();
        }

        private void Extract()
        {
            //fire off an event, and we can then tell the game manager to add the temp supplies to the total supplies
            extractionEvent?.Invoke();

            EndExtract();
        }

        private void EndExtract()
        {
            isExtracting = false;
        }
    }
}