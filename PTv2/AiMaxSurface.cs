using BlokusDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollection
{
    class AiMaxSurface : AiRandom
    {
        public override bool Play(ref BlokusDll.GameGrid grid)
        {
            var r = new Random();
            var b = HelperFunctions.GetAllMoves(grid, pieces, me);
            var max = b.Max(z => z.Pc.coords.Length);
            var a = b.FindAll(z => z.Pc.coords.Length == max);
            if (a.Count == 0)
                return false;
            var i = r.Next(a.Count);
            pieces[a[i].Pc.id] = false;
            grid.Place(a[i], me);
            return true;
        }
    }
}
