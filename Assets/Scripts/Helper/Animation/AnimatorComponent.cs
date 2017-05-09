using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using UnityEngine;

namespace Matrix
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimatorComponent : MonoBehaviour
    {
        public int CurrentIndex;
        public List<MyAnimation> Animations;

        private Dictionary<string, Dictionary<int, Dictionary<EOrientation, int>>> _animationIndex;

        private SpriteRenderer _spriteRenderer;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            ConstructDictionary();
        }

        private void ConstructDictionary()
        {
            _animationIndex = new Dictionary<string, Dictionary<int, Dictionary<EOrientation, int>>>();

            for (int i = 0; i < Animations.Count; i++)
            {
                string currentTag = Animations[i].Key.Tag;
                int currentNumber = Animations[i].Key.Number;
                EOrientation currentOrientation = Animations[i].Key.Orientation;

                // if the tag already exists, we add the animation to it
                if (_animationIndex.ContainsKey(currentTag))
                {
                    // if the number already exists, we add the animation to it
                    if (_animationIndex[currentTag].ContainsKey(currentNumber))
                    {
                        _animationIndex[currentTag][currentNumber].Add(currentOrientation, i);
                    }
                    else // we construct a new dictionary for this number
                    {
                        Dictionary<EOrientation, int> newOrientationIndexDic = new Dictionary<EOrientation, int>
                        {
                            {currentOrientation, i}
                        };

                        _animationIndex[currentTag].Add(currentNumber, newOrientationIndexDic);
                    }
                }
                else // we construct a new dictionary for this tag
                {
                    Dictionary<EOrientation, int> newOrientationIndexDic = new Dictionary<EOrientation, int>
                    {
                        {currentOrientation, i}
                    };

                    Dictionary<int, Dictionary<EOrientation, int>> newNumberIndexDic = new Dictionary<int, Dictionary<EOrientation, int>>
                    {
                        {currentNumber, newOrientationIndexDic}
                    };

                    _animationIndex.Add(currentTag, newNumberIndexDic);
                }
            }
        }

        void LateUpdate()
        {
            Animations[CurrentIndex].Actualize(TimeManager.Instance.deltaTime, _spriteRenderer, transform);
        }

        public void SetCurrentAnimation(string animationTag, int number = 0, EOrientation orientation = 0)
        {
            CurrentIndex = _animationIndex[animationTag][number][orientation];
            Animations[CurrentIndex].Reset();
        }

        public void SetCurrentAnimation(string animationTag, EOrientation orientation)
        {
            SetCurrentAnimation(animationTag, 0, orientation);
        }

        public void SetCurrentAnimation(AnimationKey animationKey)
        {
            SetCurrentAnimation(animationKey.Tag, animationKey.Number, animationKey.Orientation);
        }

        /// <summary>
        /// Usefull to store the animation index and by this way
        /// reduce the research time into the dictionary
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public int GetAnimationIndex(AnimationKey animationKey)
        {
            return GetAnimationIndex(animationKey.Tag, animationKey.Number, animationKey.Orientation);
        }

        private int GetAnimationIndex(string tag, int number, EOrientation orientation)
        {
            return _animationIndex[tag][number][orientation];
        }

        public List<int> GetAllNumberFromTag(string tag)
        {
            List<int> allNumbers = new List<int>();

            if (_animationIndex.ContainsKey(tag))
            {
                foreach (int number in _animationIndex[tag].Keys)
                {
                    allNumbers.Add(number);
                }
            }

            return allNumbers;
        }
    }
}
