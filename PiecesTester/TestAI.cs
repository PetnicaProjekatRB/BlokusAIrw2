using System;
using System.Linq;
using BlokusDll;
using System.Collections.Generic;
using System.Threading;

namespace PiecesTester
{
    class TestAI : IBlokusAi
    {
        public Hand hand;
        private Player player;
        private Random rand = new Random();

        public bool Validate()
        {
            return true;
        }

        public bool Start(Player player)
        {
            this.player = player;
            this.hand = Hand.FullHand;
            return true;
        }

        public int Modify(string name, string value)
        {
            throw new NotImplementedException();
        }

        public bool Play(ref GameGrid grid)
        {
            Thread.Sleep(1000);
            List<Move> list = HelperFunctions.GetAllMoves(grid, hand, player).ToList<Move>();
            if (list.Count == 0)
                return false;

            Move move = list[rand.Next(list.Count)];
            grid.Place(move, player);
            hand.UsePiece(move.Pc.id);
            return true;
        }
    }
}
