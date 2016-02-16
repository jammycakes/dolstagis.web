using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Tests.Objects
{
    [Flags]
    public enum When
    {
        Neither = 0,
        Before = 1,
        After = 2,
        Both = Before | After
    }
}
