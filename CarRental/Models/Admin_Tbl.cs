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
    
    public partial class Admin_Tbl
    {
        public int id { get; set; }
        public string FIO { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string user_ID { get; set; }

        public Admin_Tbl () { }

        public Admin_Tbl(string FIO, string Login, string Password, string user_ID)
        {
            this.FIO = FIO;
            this.Login = Login;
            this.Password = Password;
            this.user_ID = user_ID;
        }
    }
}
