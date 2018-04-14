using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models.DataAnnotations
{
    public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {

        public RequiredAttribute()
        {
            this.ErrorMessage = "{0}必填";
        }

    }
}
