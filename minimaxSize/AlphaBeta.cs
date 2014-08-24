using BlokusDll;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimaxSize
{
    public class AlphaBeta : Odbacivalje
    {

        public virtual bool Play(ref GameGrid grid)
        {
            var s = new Stopwatch();
            bool retval;
            int depth = DEPTH;
            do
            {
                s.Reset();
                s.Start();
                {
                    potez++;
                    Move? move = chooseMove(grid.Clone());
                    if (!move.HasValue)
                    {
                        retval = false;
                    }
                    else
                    {
                        //this.getDeckForMove(potez).UsePiece(move.Value.Pc.id);
                        retval = grid.Place(move.Value, player);
                    }
                }
                s.Stop();
                depth++;
            } while (retval && false);
            return retval;
        }
        
        protected override Move? chooseMove(GameGrid grid)
        {
            var nodes = HelperFunctions.GetAllMoves(grid, this.getDeckForMove(potez), this.player);
            if (nodes.Count == 0)
                return null;
            Move m = nodes[0]; 
            List<Move> bests = new List<Move>();
            int bestVal = int.MinValue;
            foreach (var move in nodes)
            {
                var gr = grid.Clone();
                gr.Place(move, this.player);
                int val = minimax(gr, getDeckForMove(potez).Clone(), Hand.FullHand.Clone(), (DEPTH + ((potez > 9) ? 2 : 0)), false, potez);
                if (val > bestVal)
                {
                    bests = new List<Move>();
                    bests.Add(move);
                    bestVal = val;
                }
                else if (val == bestVal)
                {
                    bests.Add(move);
                }
            }
            if (bests.Any(mov => mov.Pc.name != "I1"))
                bests.RemoveAll(mov => mov.Pc.name == "I1");
            return bests.ElementAt(new Random().Next(bests.Count));
        }
        
        protected override int minimax(GameGrid grid, Hand myHand, Hand oponentHand, int depth, bool maxPlayer, int potez)
        {
            return minimax(grid, myHand, oponentHand, depth, maxPlayer, potez, int.MinValue, int.MaxValue);
        }
        
        protected int minimax(GameGrid grid, Hand myHand, Hand oponentHand, int depth, bool maxPlayer, int potez, int alpha, int beta)
        {
            grid.GenerateSquareCache();
            var t = DateTime.Now;
            var nodes = HelperFunctions.GetAllMoves(grid, (maxPlayer) ? myHand : oponentHand, (maxPlayer) ? this.player : PlayerHelper.other(this.player));

            if (nodes.Any((Move m) => vrednostPoteza(grid, m, maxPlayer, potez) >= (SEDAM - potez / 2)))
            {
                nodes = (from m in nodes where vrednostPoteza(grid, m, maxPlayer, potez) >= (SEDAM - potez / 2) select m).ToList();
            }

            if ((depth == 0) || (nodes.Count == 0))
            {
                if (!maxPlayer && (nodes.Count == 0) && (depth != 0))
                {
                    return minimax(grid, myHand, oponentHand, depth - 1, !maxPlayer, potez + 1, alpha, beta);
                }
                return score(grid);
            }
            
            if (maxPlayer)
            {
                var gamma = alpha;
                foreach (var node in nodes)
                {
                    var gr = grid.Clone();
                    gr.Place(node, this.player);
                    gamma = Math.Max(gamma, minimax(gr, myHand.Clone().UsePiece(node.Pc.id), oponentHand, depth - 1, false, potez + 1, alpha, beta));
                    if (beta <= gamma)
                        break;
                }
                return alpha;
            }
            else
            {
                var gamma = beta;
                foreach (var node in nodes)
                {
                    var gr = grid.Clone();
                    gr.Place(node, this.player);
                    gamma = Math.Min(gamma, minimax(gr, myHand, oponentHand.Clone().UsePiece(node.Pc.id), depth - 1, true, potez + 1, alpha, beta));
                    if (beta <= alpha)
                        break;
                }
                return beta;
            }
        }
    }
}
