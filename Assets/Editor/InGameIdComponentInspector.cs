using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Matrix;
using UnityEditor;

namespace MatrixEditor
{
    [CustomEditor(typeof(InGameIdComponent))]
    public class InGameIdComponentInspector : UnityEditor.Editor
    {
        private InGameIdComponent _id;

        public enum Mask
        {
            Team = 1,
            DeplacementMode = 2
        }

        private Mask mask = 0;

        void OnEnable()
        {
            _id = (InGameIdComponent)serializedObject.targetObject;

            mask = (Mask)_id.Id.GetMask();
        }

        public override void OnInspectorGUI()
        {
            // work out with the mask 
            mask = (Mask)EditorGUILayout.EnumMaskField("Mask", mask);

            mask = (Mask)GetMask((int)mask);

            _id.Id.SetMask((int)mask);

            // display internal data
            EditorGUILayout.SelectableLabel("id : " + _id.Id.GetId() + "\nmask : " + _id.Id.GetMask() + " | " + (int)mask);


            // display id options from the current mask
            DrawBit("Hero", "Ennemy", InGameId.TEAM_HERO);
            DrawBit("Flying", "Walking", InGameId.FLYING);

            // ...
        }

        private int GetMask(int unityValue)
        {
            // If "Everything" is set, force Unity to unset the extra bits by iterating through them 
            if ((int)mask < 0)
            {
                int bits = 0;
                foreach (var enumValue in System.Enum.GetValues(typeof(Mask)))
                {
                    int checkBit = (int)mask & (int)enumValue;
                    if (checkBit != 0)
                        bits |= (int)enumValue;
                }

                return bits;
            }

            return unityValue;
        }

        private bool DrawBit(string name, string contrary, InGameIdValue value)
        {
            if (_id.Id.MaskContains(value.Value))
            {
                bool result = EditorGUILayout.Toggle(GetFullName(name, contrary), _id.Id.Contains(value));

                _id.Id.Set(value, result);

                return result;
            }

            return false;
        }

        private string GetFullName(string value, string contrary)
        {
            return "True : " + value + " (!= " + contrary + " )";
        }
    }
}
