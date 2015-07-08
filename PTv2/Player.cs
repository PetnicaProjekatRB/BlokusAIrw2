using System;
using System.Linq;

namespace BlokusDll
{
    public enum Player
    {
        None = 0,
        PL1,
        PL2,
        Both
    }

    public static class PlayerHelper
    {
        public static Player other(Player p)
        {
            switch (p)
            {
                case Player.Both: return Player.None;
                case Player.None: return Player.Both;
                case Player.PL1: return Player.PL2;
                case Player.PL2: return Player.PL1;
                default: return Player.None;
            }
        }
    }
}
