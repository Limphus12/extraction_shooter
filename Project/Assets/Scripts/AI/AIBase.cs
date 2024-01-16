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
        private NavMeshAgent agent;

        private float targetDistance;
        private Vector3 targetPosition, previousTargetPosition;
        private Transform targetTransform;

        public bool IsMoving { get; protected set; }

        private bool isAttacking = false, canAttack = true;

        protected EnemyStats stats;

        public event EventHandler<EventArgs> OnStartAttack, OnAttack, OnEndAttack;
        public event EventHandler<Events.OnBoolChangedEventArgs> OnMoveChanged;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            agent = GetComponent<NavMeshAgent>();

            targetTransform = GameManager.Player.transform;

            InvokeRepeating(nameof(CheckDestination), 0f, 0.2f);
            InvokeRepeating(nameof(SetDestination), 0f, 0.2f);

            if (!stats) stats = GetComponent<EnemyStats>();

            stats.OnKill += EnemyStats_OnKill;
            stats.OnSpeedChanged += EnemyStats_OnSpeedChanged;
        }

        private void EnemyStats_OnSpeedChanged(object sender, EventArgs e)
        {
            SetAgentSpeed(stats.GetCurrentSpeed());
        }

        private void EnemyStats_OnKill(object sender, EventArgs e)
        {
            //cancel invokes and disable scripts when we die
            CancelInvoke(); agent.enabled = false; enabled = false;
        }

        private void StartAttack()
        {
            isAttacking = true;

            SetAgentSpeed(0f);

            Invoke(nameof(Attack), stats.GetAttackStats().attackDelay);

            OnStartAttack?.Invoke(this, EventArgs.Empty);
        }

        private void Attack()
        {
            if (TargetDistance() < stats.GetAttackStats().attackDistance)
            {
                //deal damage to the player; grab the player stats from the game manager and do that!

                GameManager.PlayerStats.Damage(1);
            }

            Invoke(nameof(EndAttack), stats.GetAttackStats().endAttackDelay);

            OnAttack?.Invoke(this, EventArgs.Empty);
        }

        private void EndAttack()
        {
            isAttacking = false;
            canAttack = false;

            SetAgentSpeed(stats.GetCurrentSpeed());

            Invoke(nameof(ResetAttack), stats.GetAttackStats().attackCooldown);

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

            if (TargetDistance() <= stats.GetAttackStats().attackDistance && !isAttacking && canAttack)
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