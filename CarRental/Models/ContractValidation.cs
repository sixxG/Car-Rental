using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    [MetadataType(typeof(ContractMeteData))]
    public partial class Contract
    {

        public class ContractMeteData
        {
            [DisplayName("ID")]
            [Column(TypeName = "int")]
            public int id { get; set; }

            [DisplayName("ФИО Клиента")]
            [Column(TypeName = "string")]
            public string FIO_Customer { get; set; }

            [DisplayName("ФИО Менеджера")]
            [Column(TypeName = "string")]
            public string FIO_Manager { get; set; }

            [DisplayName("Марка авто")]
            [Column(TypeName = "string")]
            public string Car_Brand { get; set; }

            [DisplayName("Модель авто")]
            [Column(TypeName = "string")]
            public string Car_Model { get; set; }

            [DisplayName("WIN номер")]
            [Column(TypeName = "string")]
            public string Car_WIN_Number { get; set; }

            [DisplayName("Доп. опции")]
            [Column(TypeName = "string")]
            public string Additional_Options { get; set; }

            [DisplayName("Дата начала аренды")]
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy hh-mm}", ApplyFormatInEditMode = true)]
            public System.DateTime Date_Start { get; set; }

            [DisplayName("Дата окончания аренды")]
            [Column(TypeName = "DateTime")]
            public System.DateTime Date_End { get; set; }

            [DisplayName("Цена")]
            [Column(TypeName = "double")]
            public double Price { get; set; }


            [DisplayName("Состояние")]
            [Column(TypeName = "string")]
            public string Condition { get; set; }

            [DisplayName("Примечания")]
            [Column(TypeName = "string")]
            public string Notes { get; set; }
        }
    }
}