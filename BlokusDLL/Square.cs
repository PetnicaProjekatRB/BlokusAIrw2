using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlokusDLL
{
    class Square
    {
        public Player owner;

        public bool Covered(Player pl)
        {
            return owner == pl;
        }

        public Square(Player owner)
        {
            this.owner = owner;
        }
    }
}
