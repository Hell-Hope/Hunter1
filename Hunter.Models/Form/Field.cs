using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models.Form
{
    public class Field
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public override int GetHashCode()
        {
            if (this == null)
                return 0;
            if (this.Name == null)
                return 0;
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this != null && obj is Field temp)
                return this.Name == temp.Name;
            return false;
        }
    }

}
