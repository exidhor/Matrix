using System.Collections;
using System.Collections.Generic;
using Matrix;
using UnityEditor;
using UnityEngine;

namespace MatrixEditor
{
    [CustomEditor(typeof(Room))]
    public class RoomInspector : Editor
    {
        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(Room room, GizmoType type)
        {
            DrawGraph(room);
        }

        private static void DrawGraph(Room room)
        {
            DrawConnectionsInRoom(room);
        }

        private static void DrawConnectionsInRoom(Room room)
        {
            Handles.color = Color.black;

            for (int i = 0; i < room.Grid.Length; i++)
            {
                for (int j = 0; j < room.Grid.Width; j++)
                {
                    DrawConnectionsFor(room.Grid.GetNodeGrid().Get(i, j), room);
                }
            }
        }

        private static void DrawConnectionsFor(Node node, Room room)
        {
            ConnectionList connectionList = node.Connections;

            DrawConnection((int)ConnectionIndex.Top, connectionList, room);
            DrawConnection((int)ConnectionIndex.Right, connectionList, room);

            DrawConnection((int)ConnectionIndex.TopLeft, connectionList, room);
            DrawConnection((int)ConnectionIndex.TopRight, connectionList, room);
        }

        private static void DrawConnection(int index, ConnectionList list, Room room)
        {
            if (list.Exists(index))
            {
                Node node0 = list.Get(index).Node0;
                Vector2 start = room.Grid.GetPositionAt(node0.Coord);

                Node node1 = list.Get(index).Node1;
                Vector2 end = room.Grid.GetPositionAt(node1.Coord);

                Handles.DrawLine(start, end);
            }
        }

        private static void DrawCircle(Selectable selectable)
        {
            Circle circle = selectable.Collider;

            Handles.color = Color.red;

            Handles.DrawWireDisc(circle.Center, Vector3.forward, circle.Radius);
        }
    }
}