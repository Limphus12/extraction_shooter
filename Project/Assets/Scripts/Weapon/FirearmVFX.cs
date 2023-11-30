using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;

namespace com.limphus.extraction_shooter
{
    public class FirearmVFX : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private Transform firePoint;

        [Space]
        [SerializeField] private GameObject bulletParticles;
        [SerializeField] private GameObject muzzleParticles;

        [SerializeField] private GameObject enemyHitParticles;
        [SerializeField] private GameObject envHitParticles;

        private Firearm firearm;

        private void Start()
        {
            SubToEvents();
        }

        private void OnDisable()
        {
            UnSubToEvents();
        }

        private void OnDestroy()
        {
            UnSubToEvents();
        }

        private void SubToEvents()
        {
            firearm = GetComponent<Firearm>();

            if (!firearm) return;

            firearm.OnStartFire += Firearm_OnStartFire;
            firearm.OnFire += Firearm_OnFire;
            firearm.OnEndFire += Firearm_OnEndFire;
            firearm.OnEnvHit += Firearm_OnEnvHit;
            firearm.OnEnemyHit += Firearm_OnEnemyHit;
        }

        private void UnSubToEvents()
        {
            firearm = GetComponent<Firearm>();

            if (!firearm) return;

            firearm.OnStartFire -= Firearm_OnStartFire;
            firearm.OnFire -= Firearm_OnFire;
            firearm.OnEndFire -= Firearm_OnEndFire;
            firearm.OnEnvHit -= Firearm_OnEnvHit;
            firearm.OnEnemyHit -= Firearm_OnEnemyHit;
        }

        private void Firearm_OnEnvHit(object sender, Events.OnRaycastHitEventArgs e)
        {
            if (!envHitParticles) return;

            GameObject particles = Instantiate(envHitParticles, e.i.point, Quaternion.FromToRotation(Vector3.up, e.i.normal));
            Destroy(particles, 2f);
        }

        private void Firearm_OnEnemyHit(object sender, Events.OnRaycastHitEventArgs e)
        {
            if (!enemyHitParticles) return;

            GameObject particles = Instantiate(enemyHitParticles, e.i.point, Quaternion.FromToRotation(Vector3.up, e.i.normal));
            Destroy(particles, 2f);
        }

        private void Firearm_OnStartFire(object sender, EventArgs e)
        {
            if (!bulletParticles || !muzzleParticles) return;

            Debug.Log("ONSTARTFIRE!");

            GameObject particlesA = Instantiate(bulletParticles, firePoint.position, firePoint.rotation);
            Destroy(particlesA, 2f);

            GameObject particlesB = Instantiate(muzzleParticles, firePoint.position, firePoint.rotation, firePoint);
            Destroy(particlesB, 2f);
        }

        private void Firearm_OnFire(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Firearm_OnEndFire(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}