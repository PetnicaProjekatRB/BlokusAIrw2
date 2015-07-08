using BlokusDll;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PTv2;

namespace AiCollection
{
    public class AiSmart : AiRandom
    {
        public override bool Play(ref GameGrid grid)
        {
            var a = HelperFunctions.GetAllMoves(grid, pieces, me);
            if (a.Count == 0)
                return false;
            List<Move> bests = new List<Move>();
            float maxed = -50;
            IBlokusAi oponentAi = (Form1.instance.ai1 == this) ? Form1.instance.ai2 : Form1.instance.ai1;

            Hand h = oponentAi.GetHand();
            List<Move> moves = HelperFunctions.GetAllMoves(grid, h.piecesAvailable, opponent);

            foreach (var m in a)
            {
                var c = evalFunc(m, grid, moves);
                if (Math.Abs(c - maxed) < 0.5f)
                    bests.Add(m);
                else if (c > maxed)
                { 
                    bests.Clear();
                    bests.Add(m);
                    maxed = c;
                }
            }
            Random r = new Random();
            var i = r.Next(bests.Count);
            grid.Place(bests.ToArray()[i], me);
            pieces[bests.ToArray()[i].Pc.id] = false;
            return true;
        }

        public AiSmart(float surf = 1, float lib = 7, float blk = 1.9f, float pen = 10f, float safe = 1.5f)
        {
            SRF_VAL = surf;
            LIB_VAL = lib;
            BLK_VAL = blk;
            PEN_VAL = pen;
            SAFE_VAL = safe;
        }

        public float SRF_VAL;
        public float LIB_VAL;
        public float BLK_VAL;
        public float PEN_VAL;
        public float SAFE_VAL;

        private float evalFunc(Move m, GameGrid g, List<Move> moves)
        {
            float val = 0.0f;

            val += m.Pc.coords.Length*SRF_VAL;

            val +=
                m.Pc.coords.Where(p => g.GetLibertyCache(m.Xcoord + p[0], m.Ycoord + p[1], opponent) > 0)
                    .Sum(p => BLK_VAL);

            HashSet<int> covered = new HashSet<int>();

            foreach (Move move in moves)
            {
                foreach (int[] coord in move.Pc.coords)
                {
                    covered.Add(coord[0] << 4 | coord[1]);
                }
            }


            foreach (var p in m.Pc.liberty)
            {
                if (g.GetSquareCache(m.Xcoord + p[0], m.Ycoord + p[1]) == Player.None)
                {
                    val += LIB_VAL;
                    if (g.GetSquareCache(m.Xcoord + p[0] + 1, m.Ycoord + p[1] + 1) == me &&
                        g.GetSquareCache(m.Xcoord + p[0] + 1, m.Ycoord + p[1]) == opponent &&
                        g.GetSquareCache(m.Xcoord + p[0], m.Ycoord + p[1] + 1) == opponent)
                    {
                        val += PEN_VAL;
                    }
                    if (g.GetSquareCache(m.Xcoord + p[0] - 1, m.Ycoord + p[1] + 1) == me &&
                        g.GetSquareCache(m.Xcoord + p[0] - 1, m.Ycoord + p[1]) == opponent &&
                        g.GetSquareCache(m.Xcoord + p[0], m.Ycoord + p[1] + 1) == opponent)
                    {
                        val += PEN_VAL;
                    }
                    if (g.GetSquareCache(m.Xcoord + p[0] + 1, m.Ycoord + p[1] - 1) == me &&
                        g.GetSquareCache(m.Xcoord + p[0] + 1, m.Ycoord + p[1]) == opponent &&
                        g.GetSquareCache(m.Xcoord + p[0], m.Ycoord + p[1] - 1) == opponent)
                    {
                        val += PEN_VAL;
                    }
                    if (g.GetSquareCache(m.Xcoord + p[0] - 1, m.Ycoord + p[1] - 1) == me &&
                        g.GetSquareCache(m.Xcoord + p[0] - 1, m.Ycoord + p[1]) == opponent &&
                        g.GetSquareCache(m.Xcoord + p[0], m.Ycoord + p[1] - 1) == opponent)
                    {
                        val += PEN_VAL;
                    }
                    if (!covered.Contains((m.Xcoord + p[0]) << 4 | m.Ycoord + p[1]))
                    {
                        val += SAFE_VAL;
                    }
                }
            }

            return val;
        }
    }
}
