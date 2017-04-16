using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    [Serializable]
    public class MyAnimation
    {
        public AnimationKey Key;

        public float Speed;
        public bool Flip;
        public bool Loop;

        public List<Sprite> Sprites;

        private float _currentTime;
        private int _currentIndex;

        public void Reset()
        {
            _currentIndex = 0;
            _currentTime = 0;
        }

        public void Actualize(float deltaTime, SpriteRenderer spriteRenderer, Transform objectTransform)
        {
            // we dodge the time update if there is no speed
            if (Speed > float.Epsilon)
            {
                _currentTime += deltaTime;

                while (_currentTime > Speed)
                {
                    _currentTime -= Speed;
                    _currentIndex++;

                    if (_currentIndex > Sprites.Count - 1)
                    {
                        if (Loop)
                        {
                            _currentIndex = 0;
                        }
                        else
                        {
                            _currentIndex = Sprites.Count - 1;
                        }
                    }
                }
            }


            if (Flip)
            {
                // check if the transform is already flipped
                if (objectTransform.localScale.x > 0)
                {
                    objectTransform.localScale = new Vector3(-objectTransform.localScale.x,
                        objectTransform.localScale.y,
                        objectTransform.localScale.z);
                }
            }
            else
            {
                if (objectTransform.localScale.x < 0)
                {
                    objectTransform.localScale = new Vector3(-objectTransform.localScale.x,
                        objectTransform.localScale.y,
                        objectTransform.localScale.z);
                }
            }

            // set the sprite to the renderer
            if (_currentIndex < 0 || _currentIndex > Sprites.Count - 1)
            {
                Debug.LogWarning("bad Index : " + _currentIndex + " . It should be in range (0, " + Sprites.Count + ").");
                Debug.LogWarning("For the animation " + Key.Tag + " (number : " + Key.Number + ", orientation  : " +
                                 Key.Orientation + ")");
                Debug.LogWarning("For the object : " + objectTransform.name);

                spriteRenderer.sprite = null;
            }
            else
            {
                spriteRenderer.sprite = Sprites[_currentIndex];
            }
        }
    }
}