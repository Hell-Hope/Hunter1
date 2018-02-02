using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models
{
    public class DataResult<T> : Result, IData<T>
    {
        public T Data { get; set; }
    }
}
