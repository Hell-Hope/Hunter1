using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models.DataAnnotations
{
    public class MaxLengthAttribute : System.ComponentModel.DataAnnotations.MaxLengthAttribute
    {
        public MaxLengthAttribute()
        {
            this.ErrorMessage = "{0}最多{1}个字符";
        }

        public MaxLengthAttribute(int length) : base(length)
        {
            this.ErrorMessage = "{0}最多{1}个字符";
        }
    }
}
