//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarRental.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Contract
    {
        public int id { get; set; }
        [Required]
        public string FIO_Customer { get; set; }
        public string FIO_Manager { get; set; }
        public string Car_Brand { get; set; }
        public string Car_Model { get; set; }
        public string Car_WIN_Number { get; set; }
        public string Additional_Options { get; set; }
        public System.DateTime Date_Start { get; set; }
        [Required]
        public System.DateTime Date_End { get; set; }
        public double Price { get; set; }
        public string Condition { get; set; }
        public string Notes { get; set; }
        public string id_client { get; set; }
    }
}
