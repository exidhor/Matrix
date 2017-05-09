using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class ManaComponent : MonoBehaviour
    {
        public int MaxMana;
        public float Mana;
        public float ManaRegeration; 

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            Mana = MaxMana;
        }

        public bool HasEnoughMana(int manaNeeded)
        {
            return Mana >= manaNeeded;
        }

        public void Add(int amount)
        {
            Mana += amount;

            if (Mana > MaxMana)
                Mana = MaxMana;
        }


        public void Use(int amount)
        {
            Mana -= amount;

            if (Mana < 0)
                Mana = 0;
        }

        void Update()
        {
            Mana += ManaRegeration * TimeManager.Instance.deltaTime;

            if (Mana > MaxMana)
                Mana = MaxMana;
        }
    }
}
