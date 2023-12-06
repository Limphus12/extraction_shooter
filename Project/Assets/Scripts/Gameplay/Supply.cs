using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class Supply : MonoBehaviour
    {
        public int SupplyAmount { get; private set; }

        public void SetSupplyAmount(int amount)
        {
            SupplyAmount = amount;
        }
    }
}