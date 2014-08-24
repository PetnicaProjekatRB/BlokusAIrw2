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
           (false, true, false,
            false, false, false,
            false, false, false,
            false, false, false,
            false, false, false,
            false, false, false,
            false, false, false);
        public Hand oponentHand;
        protected Player player;
        protected const int DEPTH = 1;
        protected decimal PROCENAT = 0.2M;
        public int potez = 1;
        protected Random r = new Random();

        public bool Validate()
        {
            return true;
        }

        public bool Start(Player player)
        {
            //hand = Hand.FullHand.Clone();
            this.player = player;
            return true;
        }

        public virtual bool Play(ref GameGrid grid)
        {
            potez++;
            Move? move = chooseMove(grid.Clone());
            if (!move.HasValue)
            {
                return false;
            }
            else
            {
                //this.getDeckForMove(potez).UsePiece(move.Value.Pc.id);
                return grid.Place(move.Value, player);
            }
        }

        protected virtual Move? chooseMove(GameGrid grid)
        {
            var nodes = HelperFunctions.GetAllMoves(grid, this.getDeckForMove(potez), this.player);
            if (nodes.Count == 0)
                return null;
            List<Move> bests = new List<Move>();
            Move m = nodes[0];
            int bestVal = int.MinValue;
            foreach (var move in nodes)
            {
                var gr = grid.Clone();
                gr.Place(move, this.player);
                int val = minimax(gr, getDeckForMove(potez).Clone(), Hand.FullHand.Clone(), DEPTH + ((potez > 9) ? 3 : (potez > 4)? 1 : 0), false, potez);
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
            return bests.ElementAt(new Random().Next(bests.Count));
        }


        protected virtual int minimax(GameGrid grid, Hand myHand, Hand oponentHand, int depth, bool maxPlayer, int potez)
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

        

        protected int score(GameGrid grid)
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

        protected Hand getDeckForMove(int move)
        {
            return hand;
        }




        public int Modify(string name, string value)
        {
            throw new NotImplementedException();
        }
    }
}
