using System;
using System.Linq;

namespace BlokusDll
{
    class Square
    {
        public Player Owner;

        public bool Covered(Player pl)
        {
            return Owner == pl;
        }

        public Square(Player owner)
        {
            this.Owner = owner;
        }
    }
}
