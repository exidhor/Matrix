using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(ManaComponent))]
    public class ManaExpeller : MonoBehaviour
    {
        public float Range;

        private ManaComponent _manaComponent;

        void Awake()
        {
            _manaComponent = GetComponent<ManaComponent>();
        }

        public void Expeller()
        {
            int bubbleNumber = ComputeBubbleNumber((int)_manaComponent.Mana);

            List<ManaBubble> bubbles = new List<ManaBubble>();

            // tmp ?
            Transform receiver = QuickFindManager.Instance.GetPlayer().transform;

            for (int i = 0; i < bubbleNumber; i++)
            {
                ManaBubble manaBubble = (ManaBubble) EffectManager.Instance.GetFreeEffect(EffectType.ManaBubble);

                manaBubble.transform.position = transform.position;

                Vector2 pausePosition = GetCirclePosition(i, bubbleNumber) + manaBubble.transform.position;

                manaBubble.SetMovement(pausePosition, receiver);

                bubbles.Add(manaBubble);
            }
        }

        private int ComputeBubbleNumber(int manaAmount)
        {
            return (int)Mathf.Ceil(manaAmount / (float)ManaBubble.MANA_PER_BUBBLE);
        }

        //source : https://github.com/SFML/SFML/blob/master/src/SFML/Graphics/CircleShape.cpp
        private Vector3 GetCirclePosition(int index, int Count)
        {
            float angle = index * 2 * Mathf.PI / Count - Mathf.PI / 2;
            float x = Mathf.Cos(angle) * Range;
            float y = Mathf.Sin(angle) * Range;

            //return new Vector2(Range + x, Range + y);
            return new Vector2(x, y);
        } 
    }
}
