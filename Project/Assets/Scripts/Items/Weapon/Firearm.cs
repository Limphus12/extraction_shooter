using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;
using com.limphus.procedural_animation;

namespace com.limphus.extraction_shooter
{
    public class Firearm : MonoBehaviour
    {
        [Header("Shooting")]
        [SerializeField] private int damage;
        [SerializeField] private float rof;

        [Header("Reloading")]
        [SerializeField] private float reloadTime;
        [SerializeField] private float partialReloadTime, fullReloadTime;
        [SerializeField] private int maxAmmo;

        private WeaponRecoil weaponRecoil;

        //private FirearmSound firearmSound;

        private FirearmSway firearmSway;
        private FirearmAnimation firearmAnimation;
        //private FirearmFX firearmFX;

        //private FirearmFunctionAnimation firearmFunctionAnimation;

        public bool IsAttacking { get; private set; }
        public bool IsAiming { get; private set; }
        public bool IsReloading { get; private set; }
        public bool IsEquipped { get; private set; }

        private int currentAmmo;

        public bool CanReload()
        {
            return currentAmmo < maxAmmo;
        }

        private Transform playerCameraTransform;

        //event examples
        //public event EventHandler<Events.OnIntChangedEventArgs> OnHealthChanged;
        //public event EventHandler<EventArgs> OnHealthDepleted, OnHealthReplenished;

        public event EventHandler<EventArgs> OnStartAttack, OnAttack, OnEndAttack, OnStartReload, OnReload, OnEndReload;
        public event EventHandler<Events.OnRaycastHitEventArgs> OnEnvHit, OnEnemyHit;
        public event EventHandler<Events.OnBoolChangedEventArgs> OnAim;
        public event EventHandler<Events.OnIntChangedEventArgs> OnAmmoChanged;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (!playerCameraTransform) playerCameraTransform = GameManager.PlayerCamera.transform;

            if (!weaponRecoil) weaponRecoil = playerCameraTransform.GetComponentInParent<WeaponRecoil>();

            if (!firearmSway) firearmSway = GetComponent<FirearmSway>();

            currentAmmo = maxAmmo;
        }

        public bool InUse()
        {
            return IsAttacking && IsReloading;
        }

        public void StartAttack()
        {
            IsAttacking = true;

            OnStartAttack?.Invoke(this, new EventArgs { });

            Attack();
        }

        private void Attack()
        {
            OnAttack?.Invoke(this, new EventArgs { });

            if (weaponRecoil) weaponRecoil.Recoil();

            currentAmmo--;

            OnAmmoChanged?.Invoke(this, new Events.OnIntChangedEventArgs { i = currentAmmo });

            //TODO: add a layermask so that we can stop hitting the supply triggers!
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    OnEnemyHit?.Invoke(this, new Events.OnRaycastHitEventArgs { i = hit });

                    damageable.Damage(damage);
                }

                else if (damageable == null) OnEnvHit?.Invoke(this, new Events.OnRaycastHitEventArgs { i = hit });
            }

            Invoke(nameof(EndAttack), 1f / rof);
        }

        private void EndAttack()
        {
            OnEndAttack?.Invoke(this, new EventArgs { });

            IsAttacking = false;
        }

        public void StartReload()
        {
            IsReloading = true;

            OnStartReload?.Invoke(this, new EventArgs { });

            Invoke(nameof(Reload), reloadTime);
        }

        private void Reload()
        {
            //in the future we may want to add a proper ammo system,
            //but for now we'll just reset the current ammo to the max ammo
            currentAmmo = maxAmmo;

            OnAmmoChanged?.Invoke(this, new Events.OnIntChangedEventArgs { i = currentAmmo });

            EndReload();
        }

        private void EndReload()
        {
            OnEndReload?.Invoke(this, new EventArgs { });

            IsReloading = false;
        }

        public void Aim(bool b)
        {
            if (IsReloading) b = false;

            else IsAiming = b;

            weaponRecoil.Aim(b);
            firearmSway.Aim(b);

            OnAim?.Invoke(this, new Events.OnBoolChangedEventArgs { i = b });
        }

        public void Interrupt()
        {
            if (!InUse()) return;

            else CancelFunctions();
        }

        private void CancelFunctions()
        {
            if (IsReloading) CancelInvoke(nameof(Reload));
            if (IsAttacking) CancelInvoke(nameof(EndAttack));

            IsReloading = false; IsAttacking = false;
        }
    }
}