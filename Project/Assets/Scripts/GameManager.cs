using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class GameManager : MonoBehaviour
    {
        public static GameObject Player;
        public static Camera PlayerCamera;

        public static int Supplies;

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