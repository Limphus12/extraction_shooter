using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using com.limphus.utilities;
using System;

namespace com.limphus.extraction_shooter
{
    public class AIBase : MonoBehaviour
    {
        [Header("AI Attributes - Movement")]
        [SerializeField] private float movementSpeed = 3f;

        [Header("AI Attributes - Attacking")]
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private float attackDelay = 1f, endAttackDelay = 1f, attackCooldown = 1f;

        private NavMeshAgent agent;

        private float targetDistance;
        private Vector3 targetPosition, previousTargetPosition;
        private Transform targetTransform;

        public bool IsMoving { get; protected set; }

        private bool isAttacking = false, canAttack = true;

        [SerializeField] protected EnemyStats enemyStats;

        public event EventHandler<EventArgs> OnStartAttack, OnAttack, OnEndAttack;
        public event EventHandler<Events.OnBoolChangedEventArgs> OnMoveChanged;


        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (!enemyStats) enemyStats = GetComponent<EnemyStats>();

            agent = GetComponent<NavMeshAgent>();

            targetTransform = GameManager.Player.transform;

            InvokeRepeating(nameof(CheckDestination), 0f, 0.2f);
            InvokeRepeating(nameof(SetDestination), 0f, 0.2f);

            enemyStats.OnKill += EnemyStats_OnKill;
        }

        private void EnemyStats_OnKill(object sender, EventArgs e)
        {
            //disable scripts when we die

            agent.enabled = false;
            enabled = false;
        }

        private void StartAttack()
        {
            isAttacking = true;

            SetAgentSpeed(0f);

            Invoke(nameof(Attack), attackDelay);

            OnStartAttack?.Invoke(this, EventArgs.Empty);
        }

        private void Attack()
        {
            if (TargetDistance() < attackDistance)
            {
                //deal damage to the player; grab the player stats from the game manager and do that!

                GameManager.PlayerStats.Damage(1);
            }

            Invoke(nameof(EndAttack), endAttackDelay);

            OnAttack?.Invoke(this, EventArgs.Empty);
        }

        private void EndAttack()
        {
            isAttacking = false;
            canAttack = false;

            SetAgentSpeed(movementSpeed);

            Invoke(nameof(ResetAttack), attackCooldown);

            OnEndAttack?.Invoke(this, EventArgs.Empty);
        }

        private void ResetAttack()
        {
            canAttack = true;
        }

        private float TargetDistance()
        {
            return Vector3.Distance(transform.position, targetTransform.position);
        }

        private void CheckDestination()
        {
            targetPosition = targetTransform.position;

            if (TargetDistance() <= attackDistance && !isAttacking && canAttack)
            {
                StartAttack();
            }
        }

        private void SetDestination()
        {
            if (targetPosition != previousTargetPosition && agent.enabled)
            {
                agent.SetDestination(targetPosition);
            }
            
            previousTargetPosition = targetPosition;
        }

        private void SetAgentSpeed(float speed)
        {
            agent.speed = speed;

            IsMoving = agent.speed != 0f; //real simple check for if we're moving or not (no speed = not moving)

            OnMoveChanged?.Invoke(this, new Events.OnBoolChangedEventArgs { i = IsMoving });
        }
    }
}