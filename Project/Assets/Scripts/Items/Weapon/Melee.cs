using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class Melee : MonoBehaviour
    {
        [SerializeField] private int damage, attackRange;
        [SerializeField] private float attackDelay = 1f, endAttackDelay = 1f, attackCooldown = 1f;

        //later on, we wanna add a thing to enable stabby stabby, or slashy slashy
        //stabbing will use raycast, slashing will use boxcast

        private Transform playerCameraTransform;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (!playerCameraTransform) playerCameraTransform = GameManager.PlayerCamera.transform;
        }

        public bool InUse()
        {
            return IsAttacking;
        }

        public bool IsAttacking { get; private set; }

        private bool canAttack = true;

        public void StartAttack()
        {
            IsAttacking = true;

            Invoke(nameof(Attack), attackDelay);
        }

        private void Attack()
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, attackRange))
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();

                if (damageable != null) damageable.Damage(damage);
            }

            Invoke(nameof(EndAttack), endAttackDelay);
        }

        private void EndAttack()
        {
            IsAttacking = false;
            canAttack = false;

            Invoke(nameof(ResetAttack), attackCooldown);
        }

        private void ResetAttack()
        {
            canAttack = true;
        }
    }
}