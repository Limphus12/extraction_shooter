using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public class EndlessWaveSystem : WaveSystem
    {
        [Header("Arributes")]
        [SerializeField] private float baseEnemyCount = 10;
        [SerializeField] private float baseSpawnRate = 5f;

        [Space]
        [SerializeField] private float enemyMultipyAmount = 1.1f;
        [SerializeField] private float rateDivisionAmount = 1.05f;

        [Space]
        [SerializeField] private Vector2 enemyCountMinMax;
        [SerializeField] private Vector2 spawnRateMinMax;

        public static int GetCurrentWave() => currentWave;

        protected override void Awake()
        {
            base.Awake();

            currentEnemyCount = baseEnemyCount;
            currentSpawnRate = baseSpawnRate;
        }

        protected override void CheckWave()
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
                    waveCountDown -= Time.deltaTime; //Debug.Log(Mathf.RoundToInt(waveCountDown));

                    //TODO: add event for wave countdown

                    //if we're not in the spawning state, and we've reached the end of the wave countdown, start the spawning!
                    //also! we've changed this to pick a wave via the generate wave method!
                    if (waveCountDown <= 0) StartCoroutine(SpawnWave(GenerateWave()));

                    break;

                default:
                    break;
            }
        }

        private float currentEnemyCount, currentSpawnRate;

        private Wave GenerateWave()
        {
            //generate a new wave
            Wave wave = new Wave
            {
                enemyCount = Mathf.RoundToInt(currentEnemyCount), spawnRate = currentSpawnRate
            };

            return wave;
        }

        protected override void NextWave()
        {
            //current enemy count; multiplying and clamping!
            currentEnemyCount *= enemyMultipyAmount; currentEnemyCount = Mathf.Clamp(currentEnemyCount, enemyCountMinMax.x, enemyCountMinMax.y);

            //current spawn rate; dividing and clamping!
            currentSpawnRate /= rateDivisionAmount; currentSpawnRate = Mathf.Clamp(currentSpawnRate, spawnRateMinMax.x, spawnRateMinMax.y);

            currentWave++;

            //need to send off an event for the next wave
            OnWaveChanged(new Events.OnIntChangedEventArgs { i = currentWave + 1 });
        }

        protected override void OnWaveChanged(Events.OnIntChangedEventArgs e)
        {
            base.OnWaveChanged(e);
        }
    }
}