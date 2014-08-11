using System;
using System.Collections.Generic;
using System.Linq;

namespace BlokusDll
{
    class GameGrid
    {
        public Dictionary<Tuple<int, int>, Square> Squares = new Dictionary<Tuple<int, int>, Square>();

        public bool Covered(int x, int y)
        {
            return Squares.ContainsKey(new Tuple<int, int>(x, y));
        }

        public bool Covered(int x, int y, Player pl)
        {
            if (!Squares.ContainsKey(new Tuple<int, int>(x, y)))
                return false;
            if (Squares[new Tuple<int, int>(x, y)].Covered(pl))
                return true;
            return false;
        }

        public bool Free(int x, int y, Player pl)
        {
            return (
                !Covered(x, y) &&
                !Covered(x + 1, y, pl) &&
                !Covered(x - 1, y, pl) &&
                !Covered(x, y + 1, pl) &&
                !Covered(x, y - 1, pl)
                );
        }

        public bool isLiberty(int x, int y, Player pl)
        {
            return Free(x, y, pl) && (Covered(x + 1, y + 1, pl) || Covered(x + 1, y - 1, pl) || Covered(x - 1, y + 1, pl) || Covered(x - 1, y - 1, pl));
        }

        public LibertyType getLibertyType(int x, int y, Player pl)
        {
            if (!isLiberty(x, y, pl))
                throw new InvalidOperationException("isLiberty == false");
            LibertyType t = LibertyType.None;
            if (Covered(x + 1, y + 1, pl))
                t |= LibertyType.UR;
            if (Covered(x + 1, y - 1, pl))
                t |= LibertyType.UL;
            if (Covered(x - 1, y + 1, pl))
                t |= LibertyType.LR;
            if (Covered(x - 1, y - 1, pl))
                t |= LibertyType.LL;
            return t;
        }
    }
}
