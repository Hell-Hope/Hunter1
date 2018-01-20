using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models
{
    public enum QueryComparer
    {
        Equal = 0,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Like,
        BothLike,
        LeftLike,
        RightLike
    }
}
