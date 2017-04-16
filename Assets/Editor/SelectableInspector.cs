using System.Collections;
using System.Collections.Generic;
using Matrix;
using UnityEditor;
using UnityEngine;

namespace MatrixEditor
{
    [CustomEditor(typeof(Selectable))]
    public class SelectableInspector : Editor
    {
        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(Selectable selectable, GizmoType type)
        {
            DrawCircle(selectable);
        }

        private static void DrawCircle(Selectable selectable)
        {
            Circle circle = selectable.Collider;

            Handles.color = Color.red;
            
            Vector3 position = circle.Center;

            if (!Application.isPlaying)
            {
                position += selectable.transform.position;
            }

            Handles.DrawWireDisc(position, Vector3.forward, circle.Radius);
        }
    }
}