using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.limphus.extraction_shooter
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private KeyCode fireKey, aimKey, reloadKey, meleeKey;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CheckInputs();
        }

        private void CheckInputs()
        {
            //if we press the melee key, we're basically gonna interrupt anything; reloading, aiming, shooting etc.
            if (Input.GetKeyDown(meleeKey))
            {
                //i think we should also stop sprinting too? 
                //send off something or connect this to the player controller and make them stop sprinting for the duration of the melee
            }

            //and then we need to check all the other inputs; you can shoot whilst aiming so that's a seperate check ig?
            //if you're reloading, you can't shoot, sooooo
            

            
        }
    }
}