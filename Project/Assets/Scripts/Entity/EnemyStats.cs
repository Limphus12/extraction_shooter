using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.extraction_shooter
{
    public class EnemyStats : EntityStats
    {
        protected override void Kill()
        {
            base.Kill();

            Destroy(gameObject);
        }
    }
}