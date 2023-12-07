using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class GameManager : MonoBehaviour
    {
        public static GameObject Player;
        public static Camera PlayerCamera;

        public static int Supplies { get; private set; }
        public static int TempSupplies { get; private set; }

        public static void AddSupplies()
        {
            //calculate the total supplies we're gonna get (this works as intended for now; wave 2 with 700 mats gives us 1400 in total)
            TempSupplies *= EndlessWaveSystem.GetCurrentWave() + 1;

            //and add them to our supplies!
            Supplies += TempSupplies;

            Debug.Log("Total Supplies Looted: " + TempSupplies + ", Total Supplies: " + Supplies);

            TempSupplies = 0; //reset our temp supplies
        }

        public static void AddTempSupplies()
        {
            TempSupplies += 100;

            Debug.Log("Current Supplies Looted: " + TempSupplies);
        }

        public static bool CanRemoveSupplies(int supplyAmount) { return Supplies - supplyAmount >= 0; }
        public static void AddSupplies(int supplyAmount) => Supplies += supplyAmount;
        public static void RemoveSupplies(int supplyAmount) => Supplies -= supplyAmount;

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            PlayerCamera = Camera.main;
        }
    }
}