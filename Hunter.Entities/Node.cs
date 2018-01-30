using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{
    public class Node
    {
        public string ID { get; set; }

        public bool Alt { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public List<string> Fields { get; set; }
    }
}
