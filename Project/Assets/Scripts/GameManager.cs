using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class GameManager : MonoBehaviour
    {
        public static GameObject Player;
        public static Camera PlayerCamera;

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            PlayerCamera = Camera.main;
        }
    }
}