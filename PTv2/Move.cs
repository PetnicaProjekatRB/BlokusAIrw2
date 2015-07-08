using System;

namespace BlokusDll
{
    public struct Move
    {
        public int Xcoord { get; private set; }

        public int Ycoord { get; private set; }

        public Piece Pc { get; private set; }

        public Tuple<int, int> Coord
        {
            get { return new Tuple<int, int>(Xcoord,Ycoord); }
        }

        public Move(int x, int y, Piece p)
            :this()
        {
            Xcoord = x;
            Ycoord = y;
            Pc = p;
        }
    }
}