using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class SupplyPoint : MonoBehaviour
    {
        [SerializeField] private float supplySpawnTime;

        //potentially add different supply prefabs in the future? with like, different supply amounts??
        [SerializeField] private GameObject supplyPrefab;

        [SerializeField] private GameObject currentSupply; //basically, we don't wanna spawn a supply if we already have one!

        private bool canSpawn = true;

        private void Update()
        {
            CheckSpawn();
        }

        private void CheckSpawn()
        {
            if (canSpawn && currentSupply == null)
            {
                SpawnSupply();
            }
        }

        private void SpawnSupply()
        {
            canSpawn = false;

            currentSupply = Instantiate(supplyPrefab, transform.position, transform.rotation);

            Invoke(nameof(ResetSpawn), supplySpawnTime);
        }

        private void ResetSpawn()
        {
            canSpawn = true;
        }

        //call this to remove the current supply
        public void RemoveCurrentSupply()
        {
            currentSupply = null;
        }

    }
}