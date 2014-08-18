using System;
using System.Linq;

namespace BlokusDll
{
    public struct Hand
    {
        private bool[] piecesAvailable;

        public Hand Clone()
        {
            Hand h;
            h.piecesAvailable = this.piecesAvailable.ToArray<bool>();
            return h;
        }

        public bool this[int i]
        {
            get
            {
                try
                { return piecesAvailable[i]; }
                catch (Exception e)
                { return false; }
            }
        }

        public Hand UsePiece(int i)
        {
            try
            { piecesAvailable[i] = false; }
            catch (Exception e)
            { }
            return this;
        }

        public Hand(params bool[] pcs)
	    {
            piecesAvailable = pcs;
	    }

        public static Hand FullHand = new Hand(
            true,true,false,
            true,true,false,
            true,true,false,
            true,true,false,
            true,true,false,
            true,true,false,
            true,true,false
            );
    }
}
