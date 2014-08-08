using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlokusDLL
{
    [Flags]
    enum LibertyType
    {
        None = 0,
        UR = 1,
        UL = 2,
        LL = 4,
        LR = 8
    }
}
