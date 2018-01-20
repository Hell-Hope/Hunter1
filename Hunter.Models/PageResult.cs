using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models
{
    public class PageResult<T> : Result, IData<List<T>>
    {

        public long Total { get; set; }

        public List<T> Data { get; set; }
    }
}
