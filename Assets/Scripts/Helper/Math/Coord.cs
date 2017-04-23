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

        public static Coord operator +(Coord c1, Coord c2)
        {
            return new Coord(c1.x + c2.x, c1.y + c2.y);
        }

        public override string ToString()
        {
            return "( " + x + ", " + y + ")";
        }
    }
}