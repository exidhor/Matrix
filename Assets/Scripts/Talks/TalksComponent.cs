using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class TalksComponent : MonoBehaviour
    {
        public string TargetName;

        public TalksSpecs TalksSpecs;

        public bool IsTalking
        {
            get { return _isTalking; }
        }

        private Dictionary<string, Talks> _talks;

        private bool _isTalking = false;
        private Talks _currentTalks = null;

        void Awake()
        {
            List<Talks> talks = TalksTable.Instance.GetTalksList(TargetName);

            if (talks != null)
            {
                ConstructTalksDictionnary(talks);
            }
        }

        public void StartTalks(string talksName, bool isInGame = true)
        {
            TalksSpecs.IsInGame = isInGame;

            _isTalking = true;
            _currentTalks = _talks[talksName];
            TalksPhase.Instance.SetTalks(_currentTalks, TalksSpecs);
        }

        private void ConstructTalksDictionnary(List<Talks> talksList)
        {
            _talks = new Dictionary<string, Talks>();

            for (int i = 0; i < talksList.Count; i++)
            {
                _talks.Add(talksList[i].Name, talksList[i]);
            }
        }

        void Update()
        {
            if (_isTalking && _currentTalks != null)
            {
                _isTalking = !TalksPhase.Instance.Actualize();
            }
        }
    }
}
