using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Matrix
{
    [RequireComponent(typeof(Text))]
    public class DisplayPlayerMana : MonoBehaviour
    {
        public ManaComponent ManaComponent;

        private Text _text;

        void Awake()
        {
            _text = GetComponent<Text>();
        }

        void Update()
        {
            _text.text = "Mana : " + ((int)ManaComponent.Mana);
        }
    }
}
