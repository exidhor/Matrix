using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using Matrix;
using UnityEngine;

namespace MatrixEditor
{
    [CustomEditor(typeof(ManaExpeller))]
    public class ManaExpellerInspector : Editor
    {
        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(ManaExpeller manaExpeller, GizmoType type)
        {
            DrawRange(manaExpeller);
        }

        private static void DrawRange(ManaExpeller manaExpeller)
        {
            Handles.color = Color.blue;

            Handles.DrawWireDisc(manaExpeller.transform.position,
                Vector3.forward, manaExpeller.Range);
        }
    }
}
