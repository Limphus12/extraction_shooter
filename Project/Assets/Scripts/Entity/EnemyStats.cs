using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    [Serializable]
    public struct EnemyAttackStats
    {
        public float attackDistance, attackDelay, endAttackDelay, attackCooldown;
    }

    public class EnemyStats : EntityStats
    {
        [Header("Attributes - Attacking")]
        [SerializeField] private EnemyAttackStats attackStats;

        public EnemyAttackStats GetAttackStats() => attackStats;

        protected override void Kill()
        {
            base.Kill();

            GetComponent<Collider>().enabled = false; //gonna disable the collider on this object

            Destroy(gameObject, 10f);
        }
    }
}