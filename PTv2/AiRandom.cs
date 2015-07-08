using System;
using System.Linq;
using BlokusDll;

namespace AiCollection
{
    public class AiRandom : IBlokusAi
    {
        public bool[] pieces = new bool[21]
        {
            true, true, true,
            true, true, true,
            true, true, true,
            true, true, true,
            true, true, true,
            true, true, true,
            true, true, true
        };

        protected Player me = Player.None;
        protected Player opponent = Player.None;

        public bool Validate()
        {
            return true;
        }

        public bool Start(Player player)
        {
            me = player;
            opponent = PlayerHelper.other(player);
            return true;
        }

        public int Modify(string name, string value)
        {
            throw new NotImplementedException();
        }

        public virtual bool Play(ref GameGrid grid)
        {
            var r = new Random();
            var a = HelperFunctions.GetAllMoves(grid, pieces, me);
            if (a.Count == 0)
                return false;
            var i = r.Next(a.Count);
            pieces[a[i].Pc.id] = false;
            grid.Place(a[i], me);
            return true;
        }

        public Hand GetHand()
        {
            return new Hand(null, pieces);
        }
    }
}
