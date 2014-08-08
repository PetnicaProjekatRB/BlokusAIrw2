using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlokusDLL
{
    struct Polynomino
    {
        public Tuple<int, int>[] Coords;

        public Polynomino(Tuple<int, int>[] c)
        {
            Coords = c;
        }

        public Polynomino(int x1, int y1)
        {
            Coords = new Tuple<int, int>[] { new Tuple<int, int>(x1, y1) };
        }

        public Polynomino(int x1, int y1, int x2, int y2)
        {
            Coords = new Tuple<int, int>[] { new Tuple<int, int>(x1, y1), new Tuple<int, int>(x2, y2) };
        }

        public Polynomino(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            Coords = new Tuple<int, int>[] { new Tuple<int, int>(x1, y1), new Tuple<int, int>(x2, y2), new Tuple<int, int>(x3, y3) };
        }

        public Polynomino(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            Coords = new Tuple<int, int>[] { new Tuple<int, int>(x1, y1), new Tuple<int, int>(x2, y2), new Tuple<int, int>(x3, y3), new Tuple<int, int>(x4, y4) };
        }

        public Polynomino(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4, int x5, int y5)
        {
            Coords = new Tuple<int, int>[] { new Tuple<int, int>(x1, y1), new Tuple<int, int>(x2, y2), new Tuple<int, int>(x3, y3), new Tuple<int, int>(x4, y4), new Tuple<int, int>(x5, y5) };
        }
    }
}
