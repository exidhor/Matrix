using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(Collider2D))]
    public class ManaDrain : MonoBehaviour
    {
        public ManaComponent Receiver;

        void OnTriggerEnter2D(Collider2D collider)
        {
            ManaBubble manaBubble = collider.GetComponent<ManaBubble>();

            if (manaBubble != null)
            {
                Absorb(manaBubble);
            }
        }

        private void Absorb(ManaBubble manaBubble)
        {
            if (Receiver != null)
            {
                Receiver.Add(ManaBubble.MANA_PER_BUBBLE);
            }

            manaBubble.Release();
        }
    }
}
