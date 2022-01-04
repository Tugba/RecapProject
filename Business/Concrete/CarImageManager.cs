using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;

        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }
        static string directory = Directory.GetCurrentDirectory() + @"\wwwroot\";
        static string path = @"Images\";
        public IResult Add(IFormFile file, CarImage carImage)
        {
            var result = BusinessRules.Run(CheckIfCarImageCountOfCarCorrect(carImage.CarId), CheckCarImageExtensionValid(file));
            if (result != null)
            {
                return result;
            }
            carImage.ImagePath = FileHelper.Add(file);
            carImage.Date = DateTime.Now;
            _carImageDal.Add(carImage);
            return new SuccessResult(Messages.CarImageAdded);
        }


        public IResult AddCollective(IFormFile[] files, CarImage carImage)
        {
            foreach (var file in files)
            {
                carImage = new CarImage { CarId = carImage.CarId };
                Add(file, carImage);
            }
            return new SuccessResult();
        }
        public IResult Delete(CarImage carImage)
        {
            string oldPath = GetById(carImage.ImageId).Data.ImagePath;
            FileHelper.Delete(oldPath);
            _carImageDal.Delete(carImage);
            return new SuccessResult(Messages.CarImageDeleted);
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(), Messages.CarImagesListed);
        }

        public IDataResult<CarImage> GetById(int carImageId)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(c => c.CarId == carImageId));
        }

        public IDataResult<List<CarImage>> GetImagesByCarId(int carId)
        {
            var result = BusinessRules.Run(CheckCarImage(carId));
            if (result != null)
            {
                return new ErrorDataResult<List<CarImage>>(GetDefaultImage(carId).Data);
            }
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(c => c.CarId == carId));
        }

        private IDataResult<List<CarImage>> GetDefaultImage(int carId)
        {
            List<CarImage> carImages = new List<CarImage>();
            carImages.Add(new CarImage() { CarId = carId, Date = DateTime.Now, ImagePath = "defaultImage.png" });
            return new SuccessDataResult<List<CarImage>>(carImages);
        }

        private IResult CheckCarImage(int carId)
        {
            var result = _carImageDal.GetAll(c => c.CarId == carId).Count;
            if (result > 0)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public IResult Update(IFormFile file, CarImage carImage)
        {
            var result = BusinessRules.Run(CheckCarImageExtensionValid(file));
            if (result != null)
            {
                return result;
            }
            CarImage oldCarImage = GetById(carImage.CarId).Data;
            carImage.ImagePath = FileHelper.Update(file, oldCarImage.ImagePath);
            carImage.Date = DateTime.Now;
            carImage.CarId = oldCarImage.CarId;
            _carImageDal.Update(carImage);

            return new SuccessResult(Messages.CarImageUpdated);
        }

        private IResult CheckCarImageExtensionValid(IFormFile file)
        {
            string[] validCarImageFileTypes = { ".JPG", ".JPEG", ".PNG", ".GIF", ".TIFF", ".TIF", ".BMP", ".ICO", ".WEBP" };
            var result = validCarImageFileTypes.Any(t => t == Path.GetExtension(file.FileName).ToUpper());
            if (!result)
            {
                return new ErrorResult("Geçersiz uzantı");
            }
            return new SuccessResult();
        }

        private IResult CheckIfCarImageCountOfCarCorrect(int carId)
        {
            var result = _carImageDal.GetAll(c => c.CarId == carId).Count;
            if (result >= 5)
            {
                return new ErrorResult(Messages.CarImageCountOfCarError);
            }
            return new SuccessResult();
        }
    }
}
