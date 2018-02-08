using Hunter.Models.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hunter.Models.User
{
    public class Login
    {
        [DisplayName("帐号")]
        [Required()]
        [DataType(DataType.Text)]
        public string Account { get; set; }

        [DisplayName("密码")]
        [Required()]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
