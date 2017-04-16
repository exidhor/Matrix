using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class InputManager : MonoSingleton<InputManager>
    {
        public List<Selectable> Selectables = new List<Selectable>();

        public void Register(Selectable selectable)
        {
            Selectables.Add(selectable);
        }

        public void Unregister(Selectable selectable)
        {
            Selectables.Remove(selectable);
        }

        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if(!MenuManager.Instance.IsActive)
                    MenuManager.Instance.DisplayMenu();
                else
                    MenuManager.Instance.Resume();
            }

            for (int i = 0; i < Selectables.Count; i++)
            {
                Selectables[i].Actualize();
            }

            if (Input.GetButtonDown("Selection"))
            {
                Selectable selectable = FindTheClosest();

                ClearSelection();

                if (selectable != null)
                {
                    Debug.Log("Selection !");
                    selectable.Select();
                }
            }
        }

        public void ClearSelection()
        {
            for (int i = 0; i < Selectables.Count; i++)
            {
                Selectables[i].Unselect();
            }
        }

        private Selectable FindTheClosest()
        {
            float smallestDistance = float.MaxValue;
            Selectable bestSelectable = null;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            for (int i = 0; i < Selectables.Count; i++)
            {
                if (Selectables[i].Collider.IsInside(mousePosition))
                {
                    float distance = Vector2.Distance(mousePosition, Selectables[i].Collider.Center);

                    if (distance < smallestDistance)
                    {
                        bestSelectable = Selectables[i];
                        smallestDistance = distance;
                    }
                }
            }

            return bestSelectable;
        }
    }
}