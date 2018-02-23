using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunter.Models
{
    public class ApplicationUser
    {

        public string ID { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Permits { get; set; }

        public bool HasPermit(IEnumerable<string> permits)
        {
            if (this.Permits == null)
                return false;
            if (permits == null)
                return true;
            return permits.All(p => this.Permits.Contains(p));
        }

    }
}
