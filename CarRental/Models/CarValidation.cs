using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    [MetadataType(typeof(CarMeteData))]
    public partial class Car_Tbl
    {

        public class CarMeteData
        {
            [DisplayName("ID")]
            [Column(TypeName = "int")]
            public int id { get; set; }

            [DisplayName("WIN")]
            [Required]
            [Column(TypeName = "varchar(50)")]
            public string WIN_Number { get; set; }

            [DisplayName("Марка")]
            [Required]
            public string Brand { get; set; }

            [DisplayName("Модель")]
            [Required]
            public string Model { get; set; }

            [DisplayName("Кузов")]
            [Required]
            public string Type_Body { get; set; }

            [DisplayName("Класс")]
            [Required]
            public string Class { get; set; }

            [DisplayName("Год выпуска")]
            [Required]
            public int Year_Release { get; set; }

            [DisplayName("Пробег")]
            [Required]
            public int Mileage { get; set; }

            [DisplayName("Цвет")]
            [Required]
            public string Color { get; set; }

            [DisplayName("КП")]
            [Required]
            public string Type_Transmission { get; set; }

            [DisplayName("Привод")]
            [Required]
            public string Type_Drive { get; set; }

            [DisplayName("Мощность")]
            [Required]
            public int Power { get; set; }

            [DisplayName("Цена в день")]
            [Required]
            public int Price_Per_Day { get; set; }

            [DisplayName("Состояние")]
            [Required]
            public string Contition { get; set; }

            [DisplayName("Описание")]
            [Required]
            public string Description { get; set; }

            [DisplayName("Фото")]
            [Required]
            public string Image { get; set; }
        }
    }
}