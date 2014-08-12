using System;
using System.Collections.Generic;
using System.Linq;

namespace BlokusDll
{
    public class GameGrid
    {
        public Dictionary<Tuple<int, int>, Square> Squares = new Dictionary<Tuple<int, int>, Square>();

        public uint[,] LibertyCache = new uint[14, 14];
        private bool libertyCacheValid = false;
        public uint[,] BlockedCache = new uint[14, 14];
        private bool blockedCacheValid = false;
        public uint[,] SquaresCache = new uint[14, 14];
        private bool squaresCacheValid = false;

        public LibertyType GetLibertyCache(int x, int y, Player pl)
        {
            if (libertyCacheValid)
            {
                if (pl == Player.PL1)
                    return (LibertyType)(LibertyCache[x, y] & 0x0f);
                else
                    return (LibertyType)((LibertyCache[x, y] & 0xf0) >> 4);
            }
            else
            {
                GenerateLibertyCache();
                return GetLibertyCache(x, y, pl);
            }
        }

        public void GenerateLibertyCache()
        {
            if (libertyCacheValid)
                return;

            LibertyCache = new uint[14, 14];
            for (int i = 0; i < 14; i++)
                for (int j = 0; j < 14; j++)
                {
                    LibertyCache[i, j] = (uint)getLibertyType(i, j, Player.PL1) |
                                         (uint)getLibertyType(i, j, Player.PL2) << 4;
                }
            libertyCacheValid = true;
        }

        public bool GetBlockedCache(int x, int y, Player pl)
        {
            if (blockedCacheValid)
            {
                if (pl == Player.PL1)
                    return (BlockedCache[x, y] & 1) > 0;
                else
                    return (BlockedCache[x, y] & 2) > 0;
            }
            else
            {
                GenerateBlockedCache();
                return GetBlockedCache(x, y, pl);
            }
        }

        public void GenerateBlockedCache()
        {
            if (blockedCacheValid)
                return;

            BlockedCache = new uint[14, 14];
            for (int i = 0; i < 14; i++)
                for (int j = 0; j < 14; j++)
                {
                    BlockedCache[i, j] = (uint)(
                        (Free(i, j, Player.PL1) ? 0 : 1) &
                        (Free(i, j, Player.PL2) ? 0 : 2));
                }
            blockedCacheValid = true;
        }

        public Player GetSquareCache(int x, int y)
        {
            if (squaresCacheValid)
                return (Player)SquaresCache[x, y];
            else
            {
                GenerateSquareCache();
                return GetSquareCache(x, y);
            }
        }

        public void GenerateSquareCache()
        {
            if (squaresCacheValid)
                return;

            SquaresCache = new uint[14, 14];
            for (int i = 0; i < 14; i++)
                for (int j = 0; j < 14; j++)
                {
                    var coords = new Tuple<int, int>(i, j);
                    if (Squares.ContainsKey(coords))
                        SquaresCache[i, j] = (uint)Squares[coords].Owner;
                    else
                        SquaresCache[i, j] = 0;
                }
            squaresCacheValid = true;
        }

        public bool Covered(int x, int y)
        {
            return GetSquareCache(x, y) != Player.None;
        }

        public bool Covered(int x, int y, Player pl)
        {
            return GetSquareCache(x, y) == pl;
        }

        public bool Free(int x, int y, Player pl)
        {
            return (
                    !Covered(x, y) &&
                    ((x < 13) ? !Covered(x + 1, y, pl) : true) &&
                    ((x > 0) ? !Covered(x - 1, y, pl) : true) &&
                    ((y < 13) ? !Covered(x, y + 1, pl) : true) &&
                    ((y > 0) ? !Covered(x, y - 1, pl) : true)
                   );
        }

        public bool isLiberty(int x, int y, Player pl)
        {
            return !GetBlockedCache(x, y, pl) && (
                                                  (
                                                   (x < 13) ? (
                                                               ((y < 13) ? Covered(x + 1, y + 1, pl)
                                                                : false) ||
                                                               ((y > 0) ? Covered(x + 1, y - 1, pl)
                                                                : false))
                                                   : false) ||
                                                  (
                                                   (x > 0) ? (
                                                               ((y < 13) ? Covered(x - 1, y + 1, pl)
                                                                : false) ||
                                                               ((y > 0) ? Covered(x - 1, y - 1, pl)
                                                                : false))
                                                   : false)
                                                  );
        }

        public LibertyType getLibertyType(int x, int y, Player pl)
        {
            LibertyType t = LibertyType.None;
            if (x < 13)
            {
                if ((y < 13) && Covered(x + 1, y + 1, pl))
                    t |= LibertyType.UR;
                if ((y > 0) && Covered(x + 1, y - 1, pl))
                    t |= LibertyType.UL;
            }
            if (x > 0)
            {
                if ((y < 13) && Covered(x - 1, y + 1, pl))
                    t |= LibertyType.LR;
                if ((y > 0) && Covered(x - 1, y - 1, pl))
                    t |= LibertyType.LL;
            }
            return t;
        }

        public bool Place(int x, int y, Piece p, Player pl, bool validate)
        {
            try
            {
                if (HardValidate(x, y, p))
                {
                    if (validate && !Validate(x, y, p, pl))
                        return false;
            
                    foreach (var s in p.coords)
                    {
                        var coords = new Tuple<int, int>(x + s[0], y + s[1]);
                        if (!Squares.ContainsKey(coords))
                            Squares.Add(coords, new Square(pl));
                    }
                }
                invalidateCache();
                return true;
            }
            catch (IndexOutOfRangeException)
            { 
                return false;
            }
        }

        private bool HardValidate(int x, int y, Piece p)
        {
            foreach (var s in p.coords)
            {
                var ax = s[0] + x;
                var ay = s[1] + y;
                if ((ax > 13) || (ax < 0) || (ay < 0) || (ay > 13))
                    return false;
            }
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

        public bool Place(Move m, Player pl)
        { return Place(m.Xcoord, m.Ycoord, m.Pc, pl); }

        public bool Validate(int x, int y, Piece p, Player pl)
        {
            try
            {
                if (!HardValidate(x, y, p))
                    return false;
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
            catch (IndexOutOfRangeException)
            { 
                return false;
            }
        }
    }
}
