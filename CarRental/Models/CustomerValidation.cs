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
            public int Id { get; set; }
            [DisplayName("ФИО")]
            public string FIO { get; set; }
            [DisplayName("Дата рождения")]
            public System.DateTime BirthDate { get; set; }
            [DisplayName("Паспортные данные")]
            public string Passport_Data { get; set; }
            [DisplayName("Номер ВУ")]
            public string Drivers_License { get; set; }
            [DisplayName("Адресс")]
            public string Address { get; set; }
        }

    }
}