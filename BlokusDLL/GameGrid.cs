using System;
using System.Collections.Generic;
using System.Linq;

namespace BlokusDll
{
    public class GameGrid
    {
        public Dictionary<Tuple<int, int>, Square> Squares = new Dictionary<Tuple<int, int>, Square>();

        private uint[,] libertyCache = new uint[14, 14];
        private bool libertyCacheValid = false;
        private uint[,] blockedCache = new uint[14, 14];
        private bool blockedCacheValid = false;
        private uint[,] squaresCache = new uint[14, 14];
        private bool squaresCacheValid = false;

        public LibertyType GetLibertyCache(int x, int y, Player pl)
        {
            if (libertyCacheValid)
            {
                if (pl == Player.PL1)
                    return (LibertyType)(libertyCache[x, y] & 0x0f);
                else
                    return (LibertyType)((libertyCache[x, y] & 0xf0) >> 4);
            }
            else
            {
                libertyCache = new uint[14, 14];
                for (int i = 0; i < 14; i++)
                    for (int j = 0; j < 14; j++)
                    {
                        libertyCache[i, j] = (uint)getLibertyType(i, j, Player.PL1) &
                            (uint)getLibertyType(i, j, Player.PL2) << 4;
                    }
                libertyCacheValid = true;
                return GetLibertyCache(x, y, pl);
            }
        }

        public bool GetBlockedCache(int x, int y, Player pl)
        {
            if (blockedCacheValid)
            {
                if (pl == Player.PL1)
                    return (blockedCache[x, y] & 1) > 0;
                else
                    return (blockedCache[x, y] & 2) > 0;
            }
            else
            {
                blockedCache = new uint[14, 14];
                for (int i = 0; i < 14; i++)
                    for (int j = 0; j < 14; j++)
                    {
                        blockedCache[i, j] = (uint)(
                            (Free(i, j, Player.PL1) ? 0 : 1) &
                            (Free(i, j, Player.PL2) ? 0 : 2));
                    }
                blockedCacheValid = true;
                return GetBlockedCache(x, y, pl);
            }
        }

        public Player GetSquareCache(int x, int y)
        {
            if (squaresCacheValid)
                return (Player)squaresCache[x, y];
            else
            {
                squaresCache = new uint[14, 14];
                for (int i = 0; i < 14; i++)
                    for (int j = 0; j < 14; j++)
                    {
                        var coords = new Tuple<int, int>(i, j);
                        if (Squares.ContainsKey(coords))
                            squaresCache[i, j] = (uint)Squares[coords].Owner;
                    }
                squaresCacheValid = true;
                return GetSquareCache(x, y);
            }
        }

        public bool Covered(int x, int y)
        {
            return Squares.ContainsKey(new Tuple<int, int>(x, y));
        }

        public bool Covered(int x, int y, Player pl)
        {
            var coords = new Tuple<int, int>(x, y);
            if (!Squares.ContainsKey(coords))
                return false;
            if (Squares[coords].Covered(pl))
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
                !Covered(x, y - 1, pl));
        }

        public bool isLiberty(int x, int y, Player pl)
        {
            return Free(x, y, pl) && (
                Covered(x + 1, y + 1, pl) ||
                Covered(x + 1, y - 1, pl) ||
                Covered(x - 1, y + 1, pl) ||
                Covered(x - 1, y - 1, pl));
        }

        public LibertyType getLibertyType(int x, int y, Player pl)
        {
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

        public bool Place(int x, int y, Piece p, Player pl, bool validate)
        {
            if (validate && !Validate(x, y, p, pl))
                return false;

            foreach (var s in p.coords)
            {
                var coords = new Tuple<int, int>(x + s[0], y + s[1]);
                if (!Squares.ContainsKey(coords))
                    Squares.Add(coords, new Square(pl));
            }
            invalidateCache();
            return true;
        }

        private void invalidateCache()
        {
            libertyCacheValid = false;
            blockedCacheValid = false;
            squaresCacheValid = false;
        }

        public bool Place(int x, int y, Piece p, Player pl)
        { return Place(x, y, p, pl, true); }

        public bool Validate(int x, int y, Piece p, Player pl)
        {
            bool coveresLiberty = false;
            foreach (var s in p.coords)
            {
                var ax = s[0] + x;
                var ay = s[1] + y;
                if (!Free(ax, ay, pl))
                    return false;
                coveresLiberty = coveresLiberty || ((ax == 4) && (ay == 4)) || ((ax == 9) && (ay == 9)) || isLiberty(ax, ay, pl);
            }
            return coveresLiberty;
        }
    }
}
