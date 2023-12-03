using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class WaveSystem : MonoBehaviour
    {
        public enum SpawnState { SPAWNING, WAITING, COUNTING };

        [System.Serializable]
        public class Wave
        {
            public Transform[] enemy;
            public int enemyCount;
            public float spawnRate;
        }

        public Wave[] waves;
        int nextWave = 0;

        [SerializeField] private float timeBetweenWaves = 5f;
        float waveCountDown;

        SpawnState state = SpawnState.COUNTING;

        //gotta keep track of the current enemies
        private List<AIBase> enemies = new List<AIBase>();

        //a private list of spawn points; populate this in the awake/start method
        private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        //private List<SpawnPoint> bossSpawnPoints = new List<SpawnPoint>();
        //private List<SpawnPoint> turretSpawnPoints = new List<SpawnPoint>();

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

        private void Start()
        {
            waveCountDown = timeBetweenWaves;
        }

        private void Update()
        {
            CheckWave();
        }

        private void CheckWave()
        {
            if (state == SpawnState.WAITING && !EnemyIsAlive()) WaveCompleted();

            else return;

            if (waveCountDown <= 0 && state != SpawnState.SPAWNING) StartCoroutine(SpawnWave(waves[nextWave]));

            else waveCountDown -= Time.deltaTime;
        }

        private void WaveCompleted()
        {
            Debug.Log("Wave Completed");

            state = SpawnState.COUNTING;
            waveCountDown = timeBetweenWaves;

            if (nextWave + 1 > waves.Length - 1)
            {
                nextWave = 0;
                Debug.Log("ALL WAVES COMPLETE! Looping...");
            }

            else nextWave++;
        }

        private bool EnemyIsAlive()
        {
            //need to redo this 






            return true;
        }

        private IEnumerator SpawnWave(Wave wave)
        {
            state = SpawnState.SPAWNING;

            for (int i = 0; i < wave.enemyCount; i++)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(wave.spawnRate);
            }

            state = SpawnState.WAITING;

            yield break;
        }

        private void SpawnEnemy()
        {
            //TODO: LOOP THROUGH SP AND FIND ONES WITHIN THE SPAWN RANGE (I.E. 20-40m AWAY)
            //THEN USE ONE OF *THOSE* TO SPAWN AN ENEMY

            //pick a random spawn point, and call the spawn function on it!
            SpawnPoint sp = spawnPoints[Random.Range(0, spawnPoints.Count)];

            //we'll also add the spawned enemy to our list!
            enemies.Add(sp.Spawn().GetComponent<AIBase>());

            //TODO: SUBSCRIBE TO THE ENEMY'S ONDEATH EVENT
            //WE CAN THEN MAKE SURE TO REMOVE IT FROM THE LIST
            //AND CALL A CHECK TO SEE IF THERE'S ANY MORE ENEMIES LEFT
            //THEN POTENTIALLY END THE ROUND
        }
    }
}