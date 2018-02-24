using Hunter.Models.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hunter.Models.Permit
{
    public class Edit
    {
        [DisplayName("ID")]
        [Required]
        public string ID { get; set; }

        [DisplayName("权限码")]
        [Required]
        public string Code { get; set; }

        [DisplayName("名称")]
        [Required]
        public string Name { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

    }
}
