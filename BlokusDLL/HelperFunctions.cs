using System;
using System.Collections.Generic;
using System.Linq;

namespace BlokusDll
{
    public static class HelperFunctions
    {
        public static List<Move> GetAllMoves(GameGrid grid, Hand hand, Player pl)
        {
            var retVal = new List<Move>();
            grid.GenerateLibertyCache();
            for (int j = 0; j < 14; j++)
            {
                for (int k = 0; k < 14; k++)
                {
                    var gridLiberty = grid.LibertyCache[j, k];
                    uint liber;

                    if (pl == Player.PL1)
                        liber = (gridLiberty & 0x0f);
                    else
                        liber = ((gridLiberty & 0xf0) >> 4);

                    if (liber > 0)
                    {
                        for (int i = 0; i < PieceLoader.matricaSvega.orbitale.Length; i++)
                        {
                            if (hand[i])
                            {
                                for (int l = 0; i < PieceLoader.matricaSvega.orbitale[i].Length; i++)
                                {
                                    foreach (var pieceLiberty in PieceLoader.matricaSvega.orbitale[i][l].liberty)
                                    {
                                        if ((pieceLiberty[2] & liber) > 0)
                                        { 
                                            var ax = pieceLiberty[0];
                                            var ay = pieceLiberty[1];
                                            if (grid.Validate(j - ax, k - ay, PieceLoader.matricaSvega.orbitale[i][l], pl))
                                                retVal.Add(new Move(j - ax, k - ay, PieceLoader.matricaSvega.orbitale[i][l]));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return retVal;
        }
    }
}
