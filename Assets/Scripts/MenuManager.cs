using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class MenuManager : MonoSingleton<MenuManager>
    {
        public GameObject Menu;

        public bool IsActive
        {
            get { return _isActive; }
        }

        private bool _isActive = false;

        public void DisplayMenu()
        {
            Menu.SetActive(true);
            Pause();

            _isActive = true;
        }

        public void HideMenu()
        {
            Menu.SetActive(false);

            _isActive = false;
        }

        public void Restart()
        {
            HideMenu();
            GameManager.Instance.StartGame();
            TimeManager.Instance.Resume();
        }

        public void Resume()
        {
            TimeManager.Instance.Resume();
            HideMenu();
        }

        public void Pause()
        {
            TimeManager.Instance.Pause();
        }

        public void Quit()
        {
        #if UNITY_STANDALONE
            //Quit the application
            Application.Quit();
        #endif

            //If we are running in the editor
        #if UNITY_EDITOR
            //Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        }
    }
}
