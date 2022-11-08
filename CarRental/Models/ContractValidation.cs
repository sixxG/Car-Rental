using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    [MetadataType(typeof(ContractMetaData))]
    public partial class Contract
    {
        public class ContractMetaData
        {
            [DisplayName("ID")]
            public int Id { get; set; }

            [DisplayName("ФИО Клиента")]
            [Required]
            public string FIO_Customer { get; set; }

            [DisplayName("ФИО Менеджера")]
            public string FIO_Manager { get; set; }

            [DisplayName("Марка авто")]
            [Required]
            public string Car_Brand { get; set; }

            [DisplayName("Модель")]
            [Required]
            public string Car_Model { get; set; }

            [DisplayName("WIN Номер")]
            [Required]
            public string Car_WIN_Number { get; set; }

            [DisplayName("Доп. опции")]
            [Required]
            public string Additional_Options { get; set; }

            [DisplayName("Дата начала аренды")]
            [Required]
            public System.DateTime Date_Start { get; set; }

            [DisplayName("Дата конца аренды")]
            [Required]
            public System.DateTime Date_End { get; set; }

            [DisplayName("Цена аренды")]
            [Required]
            public int Price { get; set; }

            [DisplayName("Состояние")]
            [Required]
            public string Condition { get; set; }

            [DisplayName("Примечания")]
            [Required]
            public string Notes { get; set; }
        }
    }
}