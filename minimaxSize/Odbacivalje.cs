//#define KURAC
using BlokusDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace minimaxSize
{
    public class Odbacivalje : Ai
    {
        protected const int SEDAM = 18;
        
        protected override int minimax(GameGrid grid, Hand myHand, Hand oponentHand, int depth, bool maxPlayer, int potez)
        {
            grid.GenerateSquareCache();
            var t = DateTime.Now;
            var nodes = HelperFunctions.GetAllMoves(grid, (maxPlayer) ? myHand : oponentHand, (maxPlayer) ? this.player : PlayerHelper.other(this.player));
#if KURAC
            Console.WriteLine("2: {0}.{1}", (DateTime.Now - t).Seconds, (DateTime.Now - t).Milliseconds);
            t = DateTime.Now;
#endif
            /*if (nodes.Any((Move m) => vrednostPoteza(grid, m, maxPlayer, potez) <= (SEDAM - (float)potez / 1.5)))
            {
                nodes = (from m in nodes where vrednostPoteza(grid, m, maxPlayer, potez) <= (SEDAM - potez / 1.5) select m).ToList();
            }*/
#if KURAC
            Console.WriteLine("3: {0}.{1}", (DateTime.Now - t).Seconds, (DateTime.Now - t).Milliseconds);
            t = DateTime.Now;
#endif
            var k = Math.Pow(nodes.Count, depth + 1);

#if KURAC
            Console.WriteLine("4: {0}.{1}", (DateTime.Now - t).Seconds, (DateTime.Now - t).Milliseconds);
            t = DateTime.Now;
#endif
            if (k > 100)
            {
                var c = nodes.Count;
                var p = (int)(nodes.Count * PROCENAT);
#if KURAC
            Console.WriteLine("3: {0}.{1}", (DateTime.Now - t).Seconds, (DateTime.Now - t).Milliseconds);
            t = DateTime.Now;
#endif
                for (int i = p; i < c; i++)
                    nodes.RemoveAt(p);
#if KURAC
            Console.WriteLine("3: {0}.{1}", (DateTime.Now - t).Seconds, (DateTime.Now - t).Milliseconds);
            t = DateTime.Now;
#endif
            }
            if ((depth == 0) || (nodes.Count == 0))
            {
                if (!maxPlayer && (nodes.Count == 0) && (depth != 0))
                {
                    minimax(grid, myHand, oponentHand, depth - 1, !maxPlayer, potez + 1);
                }
                return score(grid);
            }
            if (maxPlayer)
            {
                int bestVal = int.MinValue;
                foreach (var node in nodes)
                {
                    var gr = grid.Clone();
                    gr.Place(node, this.player);
                    int val = minimax(gr, (potez == 5) ? getDeckForMove(6).Clone() : (myHand.Clone().UsePiece(node.Pc.id)), oponentHand, depth - 1, false, potez + 1);
                    bestVal = Math.Max(val, bestVal);
                }
                return bestVal;
            }
            else
            {
                int bestVal = int.MaxValue;
                foreach (var node in nodes)
                {
                    var gr = grid.Clone();
                    gr.Place(node, PlayerHelper.other(this.player));
                    int val = minimax(gr, myHand, oponentHand.Clone().UsePiece(node.Pc.id), depth - 1, true, potez + 1);
                    bestVal = Math.Min(val, bestVal);
                }
                return bestVal;
            }
        }

        
        protected int min4 (int a, int b, int c, int d)
        {
            return Math.Max(Math.Max(a, b), Math.Max(c, d));
        }

        protected int vrednostPoteza(GameGrid grid, Move m, bool maxPlayer, int potez)
        {
            float vrednost = 0;

            vrednost += m.Pc.coords.Length * 2.0f;

            if (vrednost > (SEDAM - potez / 2))
                return (int)vrednost;

         // vrednost += m.Pc.liberty.Length * 1.5f;

            foreach (int[] lib in m.Pc.liberty)
            {
                if (((lib[2] & 1) > 0) && (!grid.Covered(m.Xcoord + lib[0] + 1, m.Ycoord + lib[1] + 1)))
                {
                    vrednost += 7f * min4((m.Xcoord + lib[0] + 1) + 1, 14 - (m.Xcoord + lib[0] + 1), (m.Ycoord + lib[1] + 1) + 1, 14 - (m.Ycoord + lib[1] + 1)) / 7;
                    //vrednost += 1.5f;
                    if (vrednost > (SEDAM - potez / 2))
                        return (int)vrednost;
                }
                if (((lib[2] & 2) > 0) && (!grid.Covered(m.Xcoord + lib[0] - 1, m.Ycoord + lib[1] + 1)))
                {
                    vrednost += 7f * min4((m.Xcoord + lib[0] - 1) + 1, 14 - (m.Xcoord + lib[0] - 1), (m.Ycoord + lib[1] + 1) + 1, 14 - (m.Ycoord + lib[1] + 1)) / 7;
                    //vrednost += 1.5f;
                    if (vrednost > (SEDAM - potez / 2))
                        return (int)vrednost;
                }
                if (((lib[2] & 4) > 0) && (!grid.Covered(m.Xcoord + lib[0] - 1, m.Ycoord + lib[1] - 1)))
                {
                    vrednost += 7f * min4((m.Xcoord + lib[0] - 1) + 1, 14 - (m.Xcoord + lib[0] - 1), (m.Ycoord + lib[1] - 1) + 1, 14 - (m.Ycoord + lib[1] - 1)) / 7;
                    //vrednost += 1.5f;
                    if (vrednost > (SEDAM - potez / 2))
                        return (int)vrednost;
                }
                if (((lib[2] & 8) > 0) && (!grid.Covered(m.Xcoord + lib[0] + 1, m.Ycoord + lib[1] - 1)))
                {
                    vrednost += 7f * min4((m.Xcoord + lib[0] + 1) + 1, 14 - (m.Xcoord + lib[0] + 1), (m.Ycoord + lib[1] - 1) + 1, 14 - (m.Ycoord + lib[1] - 1)) / 7;
                    //vrednost += 1.5f;
                    if (vrednost > (SEDAM - potez / 2))
                        return (int)vrednost;
                }

            }

            foreach (int[] sq in m.Pc.coords)
            {
                if(grid.isLiberty(m.Xcoord + sq[0],m.Ycoord + sq[1],(!maxPlayer) ? player : PlayerHelper.other(player)))
                {
                    vrednost += 5.5f;
                    if (vrednost > (SEDAM - potez / 2))
                        return (int)vrednost;
                }

                if (grid.Covered(m.Xcoord + sq[0], m.Ycoord + sq[1], (maxPlayer) ? player : PlayerHelper.other(player)))
                {
                    vrednost -= 0.75f;
                    if (vrednost > (SEDAM - potez / 2))
                        return (int)vrednost;
                }
            }
            
            return (int)vrednost;
        }
    }
}
