using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    [MetadataType(typeof(AdminMeteData))]
    public partial class AdminValidation
    {
        public class AdminMeteData
        {
            [DisplayName("ID")]
            [Column(TypeName = "int")]
            public int id { get; set; }
            [DisplayName("FIO")]
            [Required]
            public string FIO { get; set; }
            [DisplayName("Login")]
            [Required]
            public string Login { get; set; }
            [DisplayName("Password")]
            [Required]
            public string Password { get; set; }
            public string user_ID { get; set; }
        }
    }
}