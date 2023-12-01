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

        private void Start()
        {

        }

        private void Update()
        {
            Inputs();
        }

        private void Inputs()
        {
            //if we press the rmb, and we're not already firing
            if (Input.GetKeyDown(KeyCode.V) && !isAttacking)
            {
                StartAttack();
            }
        }

        private bool isAttacking = false, canAttack = true;

        private void StartAttack()
        {
            isAttacking = true;

            Invoke(nameof(Attack), attackDelay);
        }

        private void Attack()
        {
            //for stabbing, we're gonna do this boxcast
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, attackRange))
            {
                //check if we hit an enemy or not
                    //if we did, fire off an event saying so
                    //oh, and do damage stuff ig

                //otherwise if we didn't
                    //just fire an event saying that we didn't!
            }

            Invoke(nameof(EndAttack), endAttackDelay);
        }

        private void EndAttack()
        {
            isAttacking = false;
            canAttack = false;

            Invoke(nameof(ResetAttack), attackCooldown);
        }

        private void ResetAttack()
        {
            canAttack = true;
        }
    }
}