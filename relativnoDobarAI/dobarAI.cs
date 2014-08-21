using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlokusDll;

namespace relativnoDobarAI
{
    public class dobarAI : IBlokusAi
    {
        StabloPretrage istorija;
        Player player;
        Hand komadi;
        int potez;

        public bool Validate()
        {
            return true;
        }

        public bool Start(Player player)
        {
            this.player = player;
            potez = 1;
            komadi = Hand.FullHand;
            istorija = null;
            return true;
        }

        public int Modify(string name, string value)
        {
            throw new NotImplementedException();
        }

        public bool Play(ref GameGrid grid)
        {
            if (istorija == null)
                istorija = new StabloPretrage(grid);
            potez++;
            Move? move = chooseMove(grid);
        }

        private Move? chooseMove(GameGrid grid)
        {
        }
    }
}
