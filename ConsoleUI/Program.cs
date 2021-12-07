using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            CarTest();
          //  BrandTest();


            Console.WriteLine("Hello world!");
        }

        private static void BrandTest()
        {
            BrandManager brandManager = new BrandManager(new EfBrandDal());
            var result = brandManager.GetAll();
            if (result.Success)
            {
                foreach (var brand in result.Data)
                {
                    Console.WriteLine(brand.BrandName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
            
        }

        private static void CarTest()
        {
            //CarManager carManager = new CarManager(new InMemoryCarDal());

            CarManager carManager = new CarManager(new EfCarDal());
            // ColorManager colorManager = new ColorManager(new EfColorDal());
            // BrandManager brandManager = new BrandManager(new EfBrandDal());

            //carManager.Add(new Car { CarId = 1, BrandId = 1, ColorId = 3, DailyPrice = 250, ModelYear = 2014, Description = "Renault Megane" });
            //carManager.Add(new Car { CarId = 2, BrandId = 2, ColorId = 3, DailyPrice = 500, ModelYear = 2019, Description = "BMW" });
            var result = carManager.GetCarDetails();

            if (result.Success)
            {
                foreach (var car in result.Data)
                {
                    Console.WriteLine(car.CarName + " / " + car.BrandName + " / " + car.ColorName + " / " + car.DailyPrice);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

           
        }
    }
}
