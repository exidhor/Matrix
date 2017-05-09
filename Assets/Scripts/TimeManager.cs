using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class TimeManager : MonoSingleton<TimeManager>
    {
        public float MenuTimeScale = 1f;

        public float deltaTime
        {
            get { return Time.deltaTime; }
        }

        public float fixedDeltaTime
        {
            get { return Time.fixedDeltaTime; }
        }

        public float menuDeltaTime
        {
            get { return Time.unscaledDeltaTime*MenuTimeScale; }
        }

        public bool IsPaused
        {
            get { return Time.timeScale == 0f; }
        }

        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
        }
    }
}