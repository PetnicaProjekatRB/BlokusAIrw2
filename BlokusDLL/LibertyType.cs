using System;
using System.Linq;

namespace BlokusDll
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
