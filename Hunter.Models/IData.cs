using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models
{
    public interface IData<T>
    {
        T Data { get; set; }
    }
}
