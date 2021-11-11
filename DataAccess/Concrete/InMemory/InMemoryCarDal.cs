using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryCarDal : ICarDal

    {
        List<Car> _cars;
        public InMemoryCarDal()
        {
            _cars = new List<Car> {
            new Car{CarId=1,BrandId=1,ColorId=3,ModelYear=2008,DailyPrice=57500,Description="Renault Megane"},
            new Car{CarId=2,BrandId=1,ColorId=3,ModelYear=2020,DailyPrice=158700,Description="Reanult Clio"},
            new Car{CarId=3,BrandId=5,ColorId=8,ModelYear=2020,DailyPrice=260000,Description="Nissan Juke"},
            new Car{CarId=4,BrandId=9,ColorId=9,ModelYear=2015,DailyPrice=164000,Description="Audi A3"},
            new Car{CarId=5,BrandId=1,ColorId=9,ModelYear=2005,DailyPrice=72500,Description="Renault Symbol"}

            };
        }
        public void Add(Car car)
        {
            _cars.Add(car);
        }

        public void Delete(Car car)
        {
            Car carToDelete = _cars.SingleOrDefault(c => c.CarId == car.CarId);
            _cars.Remove(carToDelete);
        }

        public List<Car> GetAll()
        {
            return _cars;
        }

        public List<Car> GetById(int carId)
        {
            return _cars.Where(c => c.CarId == carId).ToList();
        }

        public void Update(Car car)
        {
            Car carToUpdate = _cars.SingleOrDefault(c => c.CarId==car.CarId);
            carToUpdate.BrandId = car.BrandId;
            carToUpdate.ColorId = car.ColorId;
            carToUpdate.ModelYear = car.ModelYear;
            carToUpdate.DailyPrice = car.DailyPrice;
            carToUpdate.Description = car.Description;
        }
    }
}
