using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.IRep
{
    public interface IRepository : IDisposable
    {
        List<Car_Tbl> GetCasrList();
        Car_Tbl GetCar(int id);
        void Create(Car_Tbl item);
        void Update(Car_Tbl item);
        void Delete(int id);
        void Save();
    }

}