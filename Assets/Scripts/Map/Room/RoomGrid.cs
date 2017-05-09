using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class RoomGrid
    {
        public int Length
        {
            get { return _roomGrid.Length; }
        }

        public int Width
        {
            get
            {
                if(Length > 0)
                    return _roomGrid[0].Length;

                return 0;
            }
        }

        public float DiagonalCase
        {
            get { return _diagonalCase; }
        }

        private Vector2 _roomPosition;
        private Vector2 _roomSize;

        private Vector2 _caseSize;

        private NodeGrid _nodeGrid;

        private RoomCase[][] _roomGrid;

        private float _diagonalCase;

        public RoomGrid(int length, int width, Vector2 roomPosition)
        {
            _roomPosition = roomPosition;
            CreateRoomGrid(length, width);
            _nodeGrid = new NodeGrid(Length, Width);
        }

        public void DestroyObjects()
        {
            for (int i = 0; i < _roomGrid.Length; i++)
            {
                for (int j = 0; j < _roomGrid[i].Length; j++)
                {
                    _roomGrid[i][j].Destroy();
                }
            }
                
        }

        public bool PointIsInGrid(Vector2 point)
        {
            return !(point.x < _roomPosition.x - _caseSize.x/2 || point.x > _roomPosition.x + _roomSize.x - _caseSize.x/2
                   || point.y < _roomPosition.y - _caseSize.y/2 || point.y > _roomPosition.y + _roomSize.y - _caseSize.y/2);
        }

        public Node GetNodeAt(Vector2 position)
        {
            if (!PointIsInGrid(position))
            {
                return null;
            }

            int x = (int)((position.x - _roomPosition.x) / _caseSize.x + _caseSize.x /2);
            int y = (int)((position.y - _roomPosition.y) / _caseSize.y + _caseSize.y / 2);

            //Debug.Log("(position.x - _roomPosition.x)/_caseSize.x = " + "( " + position.x + " - " + _roomPosition.x + " ) " + " / " + _caseSize.x + ") => " + x);
            //Debug.Log("(position.y - _roomPosition.y)/_caseSize.y = " + "( " + position.y + " - " + _roomPosition.y + " ) " + " / " + _caseSize.y + ") => " + y);

            return _nodeGrid.Get(x, y);
        }

        public Node GetClosestNodeAt(Vector2 targetPosition, Vector2 from)
        {
            if (!PointIsInGrid(targetPosition))
            {
                targetPosition = MathHelper.ClosestPointToRect(new Rect(_roomPosition + new Vector2(DiagonalCase, DiagonalCase), 
                    _roomSize - new Vector2(DiagonalCase*2, DiagonalCase*2)), targetPosition);
            }

            Node targetNode = GetNodeAt(targetPosition);

            if (!targetNode.IsBlocking)
            {
                return targetNode;
            }

            Vector2 direction = from - targetPosition;
            direction.Normalize();
            direction *= DiagonalCase;

            while (true)
            {
                Vector2 nodePosition = GetPositionAt(targetNode.Coord);

                targetPosition = nodePosition + direction;

                targetNode = GetNodeAt(targetPosition);

                if (targetNode == null)
                {
                    return null;
                }

                if (!targetNode.IsBlocking)
                {
                    return targetNode;
                }
            }
        }

        public void ComputeNodeConnections()
        {
            _nodeGrid.ComputeConnections();
        }

        public RoomCase GetRoomCase(Coord coord)
        {
            return GetRoomCase(coord.x, coord.y);
        }

        public RoomCase GetRoomCase(int x, int y)
        {
            return _roomGrid[x][y];
        }

        public void Organize(float lengthCase, float widthCase)
        {
            _roomSize.x = lengthCase*Length;
            _roomSize.y = widthCase*Width;

            _caseSize.x = lengthCase;
            _caseSize.y = widthCase;

            _diagonalCase = Mathf.Sqrt(lengthCase*lengthCase + widthCase*widthCase);

            for (int i = 0; i < _roomGrid.Length; i++)
            {
                for (int j = 0; j < _roomGrid[i].Length; j++)
                {
                    _roomGrid[i][j].SetPosition(_roomPosition + new Vector2(lengthCase*i, widthCase*j));
                    _roomGrid[i][j].SetSize(new Vector2(lengthCase, widthCase));
                }
            }
        }

        public void SetActive(bool state)
        {
            for (int i = 0; i < _roomGrid.Length; i++)
            {
                for (int j = 0; j < _roomGrid[i].Length; j++)
                {
                    _roomGrid[i][j].SetActive(state);
                }
            }
        }

        private void CreateRoomGrid(int length, int width)
        {
            // we count the case for the walls
            length += 2;
            width += 2;

            _roomGrid = new RoomCase[length][];

            for (int i = 0; i < length; i++)
            {
                _roomGrid[i] = new RoomCase[width];

                for (int j = 0; j < width; j++)
                {
                    _roomGrid[i][j] = new RoomCase();
                }
            }
        }

        public Vector2 GetPositionAt(Coord coord)
        {
            return GetRoomCase(coord).GetPosition();
        }

        public Vector2 GetCenterPositionAt(Coord coord)
        {
            return GetRoomCase(coord).GetCenterPosition();
        }

        public void SetObstacle(Coord coord, Obstacle obstacle)
        {
            SetObstacle(coord.x, coord.y, obstacle);
        }

        public void SetObstacle(int x, int y, Obstacle obstacle)
        {
            _roomGrid[x][y].SetObstacle(obstacle);

            if (obstacle != null)
            {
                _nodeGrid.Get(x, y).IsBlocking = true;
            }
        }

        public void SetCharacter(Coord coord, Character character)
        {
            SetCharacter(coord.x, coord.y, character);
        }

        public void SetCharacter(int x, int y, Character character)
        {
            _roomGrid[x][y].SetCharacter(character);
        }

        public void SetGround(Coord coord, Ground ground)
        {
            SetGround(coord.x, coord.y, ground);
        }

        public void SetGround(int x, int y, Ground ground)
        {
            _roomGrid[x][y].SetGround(ground);
        }

        public NodeGrid GetNodeGrid()
        {
            return _nodeGrid;
        }
    }
}