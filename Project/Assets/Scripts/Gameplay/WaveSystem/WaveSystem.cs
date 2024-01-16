using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public class WaveSystem : MonoBehaviour
    {
        public enum SpawnState { SPAWNING, WAITING, COUNTING };

        [System.Serializable]
        public class Wave
        {
            [Tooltip("The amount of enemies that are spawned")]
            public int enemyCount;

            [Tooltip("The rate (in seconds) that enemies are spawned")]
            public float spawnRate;

            //in the future, we're gonna want to add things for enemy health and damage!

            //ah, and speed too! because we wanna be able to go from walking to running over time!!
        }

        [SerializeField] protected Wave[] waves;
        protected static int currentWave = -1;

        [SerializeField] protected float timeBetweenWaves = 30f;

        //[SerializeField] private bool loopWaves = false;

        protected float waveCountDown;

        protected SpawnState state;

        //gotta keep track of the current enemies
        protected List<EntityStats> enemies = new List<EntityStats>();

        //a private list of spawn points; populate this in the awake/start method
        protected List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        //private List<SpawnPoint> bossSpawnPoints = new List<SpawnPoint>();
        //private List<SpawnPoint> turretSpawnPoints = new List<SpawnPoint>();

        public event EventHandler<EventArgs> OnStartWave, WaveEnded;
        public event EventHandler<Events.OnIntChangedEventArgs> WaveChanged;

        protected virtual void Awake()
        {
            //find all of the spawnpoints; we can do this via tags and a for loop
            GameObject[] spawnPointOBJs = GameObject.FindGameObjectsWithTag("SpawnPoint");

            for (int i = 0; i < spawnPointOBJs.Length; i++)
            {
                SpawnPoint sp = spawnPointOBJs[i].GetComponent<SpawnPoint>();

                if (sp != null) spawnPoints.Add(sp);
            }

            //by the end of this, we should have a nice list of spawn points to use!

            waveCountDown = 5f; //also init the wave countdown

            state = SpawnState.COUNTING; //and the spawn state
        }

        private void Update()
        {
            CheckWave();
        }

        protected virtual void CheckWave()
        {
            switch (state)
            {
                case SpawnState.SPAWNING:

                    //in the spawning state; don't bother doing anything ig?

                    break;

                case SpawnState.WAITING:

                    //waiting state: check if there are any enemies left alive, and end the wave if there are none
                    //since we only go to the waiting state once we've spawned all enemies, this is fine; the logic makes sense
                    if (!EnemyIsAlive()) EndWave();

                    else return;

                    break;
                case SpawnState.COUNTING:

                    //if we're not in the spawning state (so ig we'd be in the counting state), then do the countdown till the next wave!
                    waveCountDown -= Time.deltaTime; Debug.Log(Mathf.RoundToInt(waveCountDown));

                    //TODO: add event for wave countdown

                    //if we're not in the spawning state, and we've reached the end of the wave countdown, start the spawning!
                    if (waveCountDown <= 0)
                    {
                        NextWave();

                        StartCoroutine(SpawnWave(waves[currentWave]));
                    }

                    break;

                default:
                    break;
            }
        }

        protected void ChangeSpawnState(SpawnState newState)
        {
            if (state == newState) return;

            //fire an event here, i.e. OnSpawnStateChanged()

            state = newState;
        }

        protected bool EnemyIsAlive()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsDead || enemies[i] == null) enemies.RemoveAt(i);
            }

            //simple check; see if we have any more enemies (since we're storing the ~~AIBase~~ EntityStats class, it should (in the future) account for bosses and other enemy types, since they'll be using entity stats too)
            return enemies.Count > 0;
        }

        protected IEnumerator SpawnWave(Wave wave)
        {
            ChangeSpawnState(SpawnState.SPAWNING);

            OnStartWave?.Invoke(this, EventArgs.Empty);

            yield return new WaitForSeconds(1f); //gonna put in a lil buffer here

            //loop to spawn all of the enemies over time
            for (int i = 0; i < wave.enemyCount; i++)
            {
                //TODO
                //get the current number of enemies; if we have not reached the maximum, then continue spawning
                //else we're just gonna hold off for now, until an enemy dies

                SpawnEnemy();

                yield return new WaitForSeconds(wave.spawnRate);
            }

            //once we've spawned them all, we enter the waiting state
            ChangeSpawnState(SpawnState.WAITING);

            yield break;
        }

        protected virtual void SpawnEnemy()
        {
            //TODO: LOOP THROUGH SP AND FIND ONES WITHIN THE SPAWN RANGE (I.E. 20-40m AWAY)
            //THEN USE ONE OF *THOSE* TO SPAWN AN ENEMY

            //pick a random spawn point, and call the spawn function on it!
            SpawnPoint sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

            //we'll also add the spawned enemy to our list!
            EntityStats enemy = sp.Spawn().GetComponent<EntityStats>();

            enemies.Add(enemy);
        }

        protected virtual void EndWave()
        {
            OnWaveEnded();

            Debug.Log("Wave " + (currentWave + 1) + " Completed");

            ChangeSpawnState(SpawnState.COUNTING);

            waveCountDown = timeBetweenWaves;
        }

        protected virtual void NextWave()
        {
            if (currentWave >= waves.Length - 1)
            {
                currentWave = 0; Debug.Log("ALL WAVES COMPLETE! Looping...");
            }

            else currentWave++;

            OnWaveChanged(new Events.OnIntChangedEventArgs { i = currentWave + 1 });
        }

        protected virtual void OnWaveChanged(Events.OnIntChangedEventArgs e)
        {
            WaveChanged?.Invoke(this, e);
        }

        protected virtual void OnWaveEnded()
        {
            WaveEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}