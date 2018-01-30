using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{
    public class Line
    {
        public string ID { get; set; }

        public double M { get; set; }

        public bool Alt { get; set; }

        public bool Marked { get; set; }

        public bool Dash { get; set; }

        public string Name { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Type { get; set; }

    }
}
