using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{
    public static class Helper
    {

        public static bool IsStartTypeNode(string type)
        {
            return "start".Equals(type, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsEndTypeNode(string type)
        {
            return "end".Equals(type, StringComparison.OrdinalIgnoreCase);
        }
    }
}
