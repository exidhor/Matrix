using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Matrix
{
    public class TalksPhase : MonoSingleton<TalksPhase>
    {
        public Sprite BackgroundScreen;
        public Text Text;

        private Talks _talks;
        private TalksSpecs _specs;
        private int _nodeIndex;
        private int _letterIndex;

        private float _currentLetterTime;

        private bool _isFinished = true;
        private bool _isPaused;
        private float _currentEndNodePauseTime;

        public void SetTalks(Talks talks, TalksSpecs specs)
        {
            ResetTalks();

            _talks = talks;
            _specs = specs;
        }

        public bool Actualize()
        {
            if (GameManager.InternalInstance != null && GameManager.Instance.GameIsStarted  && !_isFinished)
            {
                float deltaTime;

                if (_specs.IsInGame)
                {
                    deltaTime = TimeManager.Instance.deltaTime;
                }
                else
                {
                    deltaTime = TimeManager.Instance.menuDeltaTime;
                }

                if (_isPaused)
                {
                    return HandleEndNodePause(deltaTime);
                }
                else
                {
                   HandleTextScrolling(deltaTime); 
                }
            }

            return false;
        }

        private bool HandleEndNodePause(float deltaTime)
        {
            _currentEndNodePauseTime += deltaTime;

            if (_currentEndNodePauseTime > _specs.EndTalksNodePause)
            {
                _nodeIndex++;

                if (_nodeIndex >= _talks.Nodes.Count)
                {
                    StopTalk();
                    _isFinished = true;
                    return true;
                }

                _isPaused = false;
                _currentLetterTime = _currentEndNodePauseTime - _specs.EndTalksNodePause;
                _currentEndNodePauseTime = 0;
                _letterIndex = 0;
            }

            return false;
        }

        public void StopTalk()
        {
            ResetTalks();

            _talks = null;
            _specs = null;
        }

        private void ResetTalks()
        {
            _isPaused = false;
            _isFinished = false;

            _currentLetterTime = 0f;
            _currentEndNodePauseTime = 0f;

            _letterIndex = 0;
            _nodeIndex = 0;
        }

        private void HandleTextScrolling(float deltaTime)
        {
            _currentLetterTime += deltaTime;

            while (_currentLetterTime > _specs.LetterSpeed)
            {
                _currentLetterTime -= _specs.LetterSpeed;
                _letterIndex++;
            }

            int sentenceSize = _talks.Nodes[_nodeIndex].Sentence.Text.Length;

            if (_letterIndex >= sentenceSize)
            {
                _isPaused = true;
                _currentEndNodePauseTime = _specs.LetterSpeed*(_letterIndex - sentenceSize) + _currentLetterTime;
                _currentLetterTime = 0;
                _letterIndex = sentenceSize - 1;
            }
        }

        void Update()
        {
            if (GameManager.InternalInstance != null && GameManager.Instance.GameIsStarted && !_isFinished)
            {
                DisplayText();
            }
            else
            {
                Text.text = "";
            }
        }

        private void DisplayText()
        {
            Text.text = _talks.Nodes[_nodeIndex].Sentence.Text.Substring(0, _letterIndex + 1);
        }
    }
}