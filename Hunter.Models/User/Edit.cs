using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hunter.Models.User
{
    public class Edit
    {

        public string ID { get; set; }

        [DisplayName("帐号")]
        [Required]
        public string Account { get; set; }

        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("用户名")]
        [Required]
        public string Name { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

    }
}
