using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public enum SpawnPointType { REGULAR, TURRET, BOSS }

    public class SpawnPoint : MonoBehaviour
    {
        //in the future, we wanna be able to spawn different enemies
        //oh, and the dropship thingy; that'd be pretty cool
        [SerializeField] private GameObject enemyPrefab;

        //we can determine if bosses should be able to spawn at this particular point
        [SerializeField] private SpawnPointType type;

        public SpawnPointType GetSpawnPointType() => type;

        public GameObject Spawn()
        {
            //spawn an enemy

            //TODO: we can spawn it in a lil radius around the point
            //gotta calc a navmesh pos tho

            GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            return enemy;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}