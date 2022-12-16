using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    [MetadataType(typeof(CustomerMetaData))]
    public partial class Customer_Tbl
    {

        public class CustomerMetaData
        {
            [DisplayName("ID")]
            [Required]
            public int Id { get; set; }
            [DisplayName("ФИО")]
            [Required]
            public string FIO { get; set; }
            [DisplayName("Дата рождения")]
            [Required]
            public System.DateTime BirthDate { get; set; }
            [DisplayName("Паспортные данные")]
            [Required]
            public string Passport_Data { get; set; }
            [DisplayName("Номер ВУ")]
            [Required]
            public string Drivers_License { get; set; }
            [DisplayName("Адресс")]
            [Required]
            public string Address { get; set; }
            [DisplayName("Login")]
            public string Login { get; set; }
            [DisplayName("Телефон")]
            [Required]
            public string Phone { get; set; }
            public string user_ID { get; set; }
        }

    }
}