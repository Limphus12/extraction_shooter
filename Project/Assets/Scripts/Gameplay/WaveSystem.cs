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

            [Tooltip("The rate (in seconds) that enemies are spawned")]
            public float spawnRate;
        }

        [SerializeField] private Wave[] waves;
        private int currentWave;

        [SerializeField] private float timeBetweenWaves = 30f;

        private float waveCountDown;

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

            waveCountDown = timeBetweenWaves; //also init the wave countdown
        }

        private void Update()
        {
            CheckWave();
            CheckEnemies();
        }

        private void CheckEnemies()
        {
            enemies.TrimExcess();
        }

        private void CheckWave()
        {
            //waiting state: check if there are any enemies left alive, and end the wave if there are none
            //since we only go to the waiting state once we've spawned all enemies, this is fine; the logic makes sense
            if (state == SpawnState.WAITING)
            {
                if (!EnemyIsAlive()) EndWave();

                else return;
            }

            //if we're not in the spawning state, and we've reached the end of the wave countdown, start the spawning!
            if (waveCountDown <= 0 && state != SpawnState.SPAWNING) StartCoroutine(SpawnWave(waves[currentWave]));

            //if we're not in the spawning state (so ig we'd be in the counting state, then do the countdown till the next wave!
            else waveCountDown -= Time.deltaTime;
        }

        private bool EnemyIsAlive()
        {
            //simple check; see if we have any more enemies (since we're storing the AIBase class, it should (in the future) account for bosses and other enemy types)
            return enemies.Count > 0;
        }

        private IEnumerator SpawnWave(Wave wave)
        {
            state = SpawnState.SPAWNING;

            //loop to spawn all of the enemies over time
            for (int i = 0; i < wave.enemyCount; i++)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(wave.spawnRate);
            }

            //once we've spawned them all, we enter the waiting state
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

            //OR, WE COULD JUST CHECK IF THE ENEMY GAME OBJECT IS NOT NULL, AND CLEAN UP THE LIST EVERY FRAME IG?
        }

        private void EndWave()
        {
            Debug.Log("Wave " + currentWave + 1 + " Completed");

            state = SpawnState.COUNTING;

            waveCountDown = timeBetweenWaves;

            NextWave();
        }

        private void NextWave()
        {
            if (currentWave >= waves.Length - 1)
            {
                currentWave = 0; Debug.Log("ALL WAVES COMPLETE! Looping..."); //need to remove this loop thingy in the future, as we want inf. waves
            }

            else currentWave++;
        }
    }
}