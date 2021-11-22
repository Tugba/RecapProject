﻿using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
   public class BrandManager :IBrandService
    {
        IBrandDal _brandDal;
        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public void Add(Brand brand)
        {
            if (brand.BrandName.Length < 2)
            { 
                Console.WriteLine(" Araba ismi minimum 2 karakter olmalıdır! "); 
            }
            _brandDal.Add(brand);
        }

        public List<Brand> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
