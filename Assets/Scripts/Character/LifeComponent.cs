using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class LifeComponent : MonoBehaviour
    {
        public int MaxLife;
        public int Life;

        void Awake()
        {
            Life = MaxLife;
        }

        public void ReceiveDamage(int damage)
        {
            Life -= damage;
        }
    }
}
