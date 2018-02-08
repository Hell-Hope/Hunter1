using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hunter.Models.DataAnnotations
{
    public class DataTypeAttribute : System.ComponentModel.DataAnnotations.DataTypeAttribute
    {
        public DataTypeAttribute(DataType dataType) : base(dataType.ToString())
        {
        }

        public DataTypeAttribute(string customDataType) : base(customDataType)
        {
        }
    }
}
