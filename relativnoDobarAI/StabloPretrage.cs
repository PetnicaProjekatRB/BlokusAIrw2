using BlokusDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace relativnoDobarAI
{
    class StabloPretrage
    {
        public Pozicija root;
        public StabloPretrage(GameGrid grid)
        {
            root = new Pozicija(grid, null);
        }
    }

    class Pozicija
    {
        GameGrid grid;
        public GameGrid Grid
        {
            get 
            {
                if (grid == null)
                {
                    var g = parent.Grid;
                    g.Place(potez, igrac);
                    grid = g;
                }
                return grid;
            }
        }
        public Move potez { get; private set; }
        public Player igrac { get; private set; }
        public Pozicija parent { get; set; }
        public List<Pozicija> deca { get; set; }

        public Pozicija(GameGrid grid, Pozicija parent)
        {
            this.grid = grid;
            this.parent = parent;
        }

        public Pozicija(Move move, Player pl, Pozicija parent)
        {
            this.potez = move;
            this.igrac = pl;
            this.parent = parent;
        }

        public void addNext(Move move, Player pl, Pozicija parent)
        {
            deca.Add(new Pozicija(move, pl, parent));
        }
    }
}
