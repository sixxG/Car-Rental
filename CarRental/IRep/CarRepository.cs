using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.IRep
{
    public class CarRepository : IRepository
    {
        private CarRentalMVCEntities1 db;

        public CarRepository()
        {
            this.db = new CarRentalMVCEntities1();
        }

        public void Create(Car_Tbl item)
        {
            db.Car_Tbl.Add(item);
        }

        public void Delete(int id)
        {
            Car_Tbl car_Tbl = db.Car_Tbl.Find(id);

            if (car_Tbl != null)
            {
                db.Car_Tbl.Remove(car_Tbl);
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Car_Tbl GetCar(int id)
        {
            return db.Car_Tbl.Find(id);
        }

        public List<Car_Tbl> GetCasrList()
        {
            return db.Car_Tbl.ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Car_Tbl item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}