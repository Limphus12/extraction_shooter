using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public struct WaveStruct
    {
        public int enemyAmount;

        //public bool gunship;

        //public bool gunshipAmount

        //public bool boss;

        //public int bossAmount;
    }

    public class EndlessWaveSystem : MonoBehaviour
    {
        [Tooltip("The time between each wave; gotta give the player some breathing room ya know?")]
        [SerializeField] private float timeBetweenWaves;

        [Tooltip("The minimum and maximum distances the enemy can spawn")]
        [SerializeField] private Vector2 spawnDistanceMinMax;

        //for performance reasons, we don't wanna spawn too many enemies
        [SerializeField] private int enemyCap;

        //gotta also then keep track of the current enemies
        private List<AIBase> enemies = new List<AIBase>();

        //a private list of spawn points; populate this in the awake/start method
        private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        int waveNumber = 0; bool spawnedAllEnemies;

        private WaveState currentWaveState;

        private void Awake()
        {
            //find all of the spawnpoints; we can do this via tags and a for loop
            GameObject[] spawnPointOBJs = GameObject.FindGameObjectsWithTag("SpawnPoint");

            for (int i = 0; i < spawnPointOBJs.Length; i++)
            {
                SpawnPoint sp = spawnPointOBJs[i].GetComponent<SpawnPoint>();

                if (sp != null) spawnPoints.Add(sp);
            }

            //by the end of this, we should have a nice list of spawn points to use!
        }

        private void Update()
        {
            CheckWave();
        }

        private void CheckWave()
        {
            if (!spawnedAllEnemies)
            {

            }
        }

        private void StartWave()
        {
            waveNumber++; //increment the wave number; we'll start on wave 1, rather than 0 ig?
            //we can then just use the exact no. for the UI stuff


        }

        private void SpawnEnemy()
        {
            //find a spawn point both close enough and far enough away from the player; we'll pick a random spawn point to use
        }

        private void EndWave()
        {

        }
    }
}