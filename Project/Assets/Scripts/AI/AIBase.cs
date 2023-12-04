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

        private bool isAttacking = false, canAttack = true;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            targetTransform = GameManager.Player.transform;

            InvokeRepeating(nameof(CheckDestination), 0f, 0.2f);
            InvokeRepeating(nameof(SetDestination), 0f, 0.2f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Destroy(gameObject);
            }
        }

        private void StartAttack()
        {
            isAttacking = true;

            SetAgentSpeed(0f);

            Invoke(nameof(Attack), attackDelay);
        }

        private void Attack()
        {
            if (TargetDistance() < attackDistance)
            {
                //deal damage to the player
            }

            Invoke(nameof(EndAttack), endAttackDelay);
        }

        private void EndAttack()
        {
            isAttacking = false;
            canAttack = false;

            SetAgentSpeed(movementSpeed);

            Invoke(nameof(ResetAttack), attackCooldown);
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

            if (TargetDistance() <= attackDistance)
            {
                StartAttack();
            }
        }

        private void SetDestination()
        {
            if (targetPosition != previousTargetPosition)
            {
                agent.SetDestination(targetPosition);
            }
            
            previousTargetPosition = targetPosition;
        }

        private void SetAgentSpeed(float speed)
        {
            agent.speed = speed;
        }
    }
}