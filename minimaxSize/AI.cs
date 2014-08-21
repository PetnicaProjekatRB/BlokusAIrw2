using System;
using BlokusDll;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimaxSize
{
    public class Ai : IBlokusAi
    {
        public Hand hand = new Hand
           (false, false, true,
            true, true, true,
            true, false, true,
            false, true, false,
            true, true, true,
            true, true, true,
            true, true, true);
        public Hand lesserHand = new Hand
           (true, true, false,
            false, false, false,
            false, true, false,
            true, false, true,
            false, false, false,
            false, false, false,
            false, false, false);
        public Hand oponentHand;
        private Player player;
        private int DEPTH = 1;
        private int potez = 1;

        public bool Validate()
        {
            return true;
        }

        public bool Start(Player player)
        {
            hand = Hand.FullHand.Clone();
            this.player = player;
            return true;
        }

        public int Modify(string name, string value)
        {
            if (name.ToLower() == "depth")
            {
                if (!int.TryParse(value, out DEPTH))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 2;
            }
        }

        public bool Play(ref GameGrid grid)
        {
            potez++;
            Move? move = chooseMove(grid.Clone());
            if (!move.HasValue)
            {
                return false;
            }
            else
            {
                this.getDeckForMove(potez).UsePiece(move.Value.Pc.id);
                return grid.Place(move.Value, player);
            }
        }

        private Move? chooseMove(GameGrid grid)
        {
            var nodes = HelperFunctions.GetAllMoves(grid, this.getDeckForMove(potez), this.player);
            if (nodes.Count == 0)
                return null;
            Move m = nodes[0];
            int bestVal = int.MinValue;
            foreach (var move in nodes)
            {
                var gr = grid.Clone();
                gr.Place(move, this.player);
                int val = minimax(gr, getDeckForMove(potez).Clone(), Hand.FullHand.Clone(), DEPTH, false, potez);
                if (val > bestVal)
                {
                    m = move;
                    bestVal = val;
                }
            }
            return m;
        }


        private int minimax(GameGrid grid, Hand myHand, Hand oponentHand, int depth, bool maxPlayer, int potez)
        {
            var nodes = HelperFunctions.GetAllMoves(grid, (maxPlayer) ? myHand : oponentHand, (maxPlayer) ? this.player : PlayerHelper.other(this.player));
            if ((depth == 0) || (nodes.Count == 0))
                return score(grid);
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

        private int score(GameGrid grid)
        {
            int sc = 0;
            foreach (var sq in grid.Squares)
            {
                if (sq.Value.Owner == this.player)
                    sc++;
                else
                    sc--;
            }
            return sc;
        }

        private Hand getDeckForMove(int move)
        {
            if (move <= 5)
                return lesserHand;
            return hand;
        }
    }
}
