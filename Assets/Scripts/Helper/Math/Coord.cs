using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Coord first, Coord second)
        {
            return first.x == second.x && first.y == second.y;
        }

        public static bool operator !=(Coord first, Coord second)
        {
            return !(first == second);
        }

        public override string ToString()
        {
            return "( " + x + ", " + y + ")";
        }
    }
}