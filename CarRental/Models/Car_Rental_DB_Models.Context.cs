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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarRentalMVCEntities1 : DbContext
    {
        public CarRentalMVCEntities1()
            : base("name=CarRentalMVCEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Customer_Tbl> Customer_Tbl { get; set; }
        public DbSet<Car_Tbl> Car_Tbl { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Manager_Tbl> Manager_Tbl { get; set; }
        public DbSet<Admin_Tbl> Admin_Tbl { get; set; }
    }
}
