using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models
{
    public class QueryAttribute : System.Attribute
    {
        public QueryAttribute(string column, QueryComparer comparer)
        {
            this.Column = column;
            this.Comparer = comparer;
        }

        public string Column { get; set; }

        public QueryComparer Comparer { get; set; }

    }
}
