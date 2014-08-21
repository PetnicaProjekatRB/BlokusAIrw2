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

        public Hand Presek(Hand druga)
        {
            Hand retval = new Hand();
            bool[] aaa = (bool[])piecesAvailable.Clone();
            for(int i = 0; i < piecesAvailable.Length; i++)
            {
                aaa[i] = aaa[i] || druga.piecesAvailable[i];
            }
            retval.piecesAvailable = aaa;
            return retval;
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

        public static Hand FullHand {
            get
            {
                return new Hand(
                    true, true, true,
                    true, true, true,
                    true, true, true,
                    true, true, true,
                    true, true, true,
                    true, true, true,
                    true, true, true
                    );
            }
        }

        public static Hand StartDeck
        {
            get
            {
                return new Hand(
                    true,true,false,
                    false,false,false,
                    false,true,false,
                    true,false,true,
                    false,false,false,
                    false,false,false,
                    false,false,false
                    );
            }
        }
    }
}
