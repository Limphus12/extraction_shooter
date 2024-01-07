using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private int fireKey, aimKey;

        [Space, SerializeField] private KeyCode reloadKey, meleeKey;

        private Firearm firearm;
        private Melee melee;

        // Start is called before the first frame update
        void Start()
        {
            if (!firearm) firearm = GetComponentInChildren<Firearm>();
            if (!melee) melee = GetComponentInChildren<Melee>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (melee.InUse()) return; //basically, if we're using the melee then we can't use anything (including another melee attack)

            //if we press the melee key, we're basically gonna interrupt anything; reloading, aiming, shooting etc.
            if (Input.GetKeyDown(meleeKey))
            {
                melee.StartAttack(); firearm.Interrupt();

                //i think we should also stop sprinting too?
            }

            if (firearm.InUse()) return; //and if we're already using our gun, then there's no point in checking the inputs! i.e. we're already shooting/reloading

            //else firearm stuff
            if (!firearm.IsReloading) //if we're not reloading
            {
                //if we can reload and we press the reload key
                if (firearm.CanReload() && Input.GetKeyDown(reloadKey)) 
                {
                    firearm.Interrupt(); firearm.StartReload();
                }

                else //if we don't press the reload key
                {
                    firearm.Aim(Input.GetMouseButton(aimKey)); //always check aiming

                    if (Input.GetMouseButtonDown(fireKey)) //if we press the fire key
                    {
                        //if we have no ammo
                        if (!firearm.HasAmmo()) firearm.StartReload();

                        //if we can attack (i.e. we have ammo, and we're not already firing)
                        else if (firearm.CanAttack()) firearm.StartAttack();
                    }
                }
            }

            else if (firearm.IsReloading)
            {
                firearm.Aim(false); //no aiming whilst reloading for now (perhaps in the future??)
            }
        }
    }
}