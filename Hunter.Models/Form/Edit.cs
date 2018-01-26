using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hunter.Models.Form
{
    public class Edit
    {

        [DisplayName("主键")]
        [DataType(DataType.Text)]
        public string ID { get; set; }

        [DisplayName("表单名称")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DisplayName("Html")]
        [DataType(DataType.Html)]
        public string Html { get; set; }

        [DisplayName("备注")]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

    }
}
