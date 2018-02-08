using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models.DataAnnotations
{
    public class MinLengthAttribute : System.ComponentModel.DataAnnotations.MinLengthAttribute
    {
        public MinLengthAttribute(int length) : base(length)
        {
            this.ErrorMessage = "{0}最少{1}个字符";
        }
    }
}
