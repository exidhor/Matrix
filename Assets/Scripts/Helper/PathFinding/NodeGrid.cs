using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class NodeGrid
    {
        private Node[][] _grid;

        public NodeGrid(int width, int height)
        {
            _grid = new Node[width][];

            for (int i = 0; i < width; i++)
            {
                _grid[i] = new Node[height];

                for (int j = 0; j < height; j++)
                {
                    _grid[i][j] = new Node(new Coord(i, j));
                }
            }
        }

        public void ClearReccords()
        {
            for (int i = 0; i < _grid.Length; i++)
            {
                for (int j = 0; j < _grid[i].Length; j++)
                {
                    _grid[i][j].ClearReccords();
                }
            }
        }

        public void ComputeConnections(bool includeDiagonals = true)
        {
            ComputeSideConnections();

            if (includeDiagonals)
            {
                ComputeDiagonalConnections();
            }
        }

        private void ComputeSideConnections()
        {
            for (int i = 0; i < _grid.Length; i++)
            {
                for (int j = 0; j < _grid[i].Length; j++)
                {
                    if (!_grid[i][j].IsBlocking)
                    {
                        _grid[i][j].Connections.ComputeRightConnection(_grid[i][j], this);
                        _grid[i][j].Connections.ComputeTopConnection(_grid[i][j], this);
                    }
                }
            }
        }

        private void ComputeDiagonalConnections()
        {
            for (int i = 0; i < _grid.Length; i++)
            {
                for (int j = 0; j < _grid[i].Length; j++)
                {
                    if (!_grid[i][j].IsBlocking)
                    {
                        _grid[i][j].Connections.ComputeTopLeftDiagonalFromSide(_grid[i][j], this);
                        _grid[i][j].Connections.ComputeTopRightDiagonalFromSide(_grid[i][j], this);
                    }
                }
            }
        }

        public Node Get(Coord coord)
        {
            return Get(coord.x, coord.y);
        }

        public Node Get(int x, int y)
        {
            return _grid[x][y];
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 && x < _grid.Length
                   && y >= 0 && y < _grid[0].Length;
        }

        public bool IsInside(Coord coord)
        {
            return IsInside(coord.x, coord.y);
        }
    }
}