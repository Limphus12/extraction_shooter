using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class SupplySpawner : MonoBehaviour
    {
        [SerializeField] private float supplySpawnTime;

        //potentially add different supply prefabs in the future? with like, different supply amounts??
        [SerializeField] private GameObject supplyPrefab;

        private GameObject currentSupply; //basically, we don't wanna spawn a supply if we already have one!

        private bool canSpawn = true;

        private void Update()
        {
            CheckSpawn();
        }

        private void CheckSpawn()
        {
            if (canSpawn && !currentSupply) SpawnSupply();
        }

        private void SpawnSupply()
        {
            canSpawn = false;

            currentSupply = Instantiate(supplyPrefab, transform.position, transform.rotation);
        }

        private void ResetSpawn()
        {
            canSpawn = true;
        }

        public void RemoveCurrentSupply()
        {
            currentSupply = null;

            Invoke(nameof(ResetSpawn), supplySpawnTime);
        }
    }
}