using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Hunter.Models
{
    public class PageParam<T>
    {
        public PageParam()
        {
            this.Index = 1;
            this.Size = 10;
        }

        [Range(1, int.MaxValue)]
        public int Index { get; set; }

        [Range(1, 100)]
        public int Size { get; set; }

        public T Condition { get; set; }

        public Sort Sort { get; set; }


        
    }
}
