using System;

namespace Moon.Asyncs.Internal
{
    [Flags]
    internal enum AsyncLayer
    {
        None = 0x0,
        Loading = 0x1,
        CutScene = 0x2,
        Game = 0x4,
    }
}
