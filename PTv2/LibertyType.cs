using System;
using System.Linq;

namespace BlokusDll
{
    [Flags]
    public enum LibertyType
    {
        None = 0,
        UR = 1 << 0,
        UL = 1 << 1,
        LL = 1 << 2,
        LR = 1 << 3
    }
}
