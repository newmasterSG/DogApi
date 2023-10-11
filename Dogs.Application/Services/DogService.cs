using Dogs.Application.DTO;
using Dogs.Application.Interfaces;
using Dogs.Domain.Entity;
using Dogs.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dogs.Application.Services
{
    public class DogService : IDogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddSync(DogDTO dog)
        {
            var dogDb = new DogEntity
            {
                Name = dog.Name,
                Color = dog.Color,
                TailLength = dog.TailLength,
                Weight = dog.Weight,
            };

            await _unitOfWork.GetRepository<DogEntity>().AddAsync(dogDb);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dog = await _unitOfWork.GetRepository<DogEntity>().GetEntityAsync(id);
            _unitOfWork.GetRepository<DogEntity>().Delete(dog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(DogEntity dog)
        {
            _unitOfWork.GetRepository<DogEntity>().Update(dog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<DogDTO>> GetAllDogsAsync(int pageNumber, int pageSize, string attribute = "", string order = "asc")
        {
            // Calculate the number of elements to skip based on the page number and page size
            int skipElements = (pageNumber - 1) * pageSize;

            var dogsDb = await _unitOfWork.GetRepository<DogEntity>().TakeAsync(skipElements, pageSize);
            List<DogDTO> dogs = new List<DogDTO>();

            if(dogsDb.Any())
            {
                dogsDb = OrderingDogs(dogsDb, attribute, order);

                foreach (var item in dogsDb)
                {
                    dogs.Add(new DogDTO
                    {
                        Color = item.Color,
                        Name = item.Name,
                        TailLength = item.TailLength,
                        Weight = item.Weight
                    });
               }
            }

            return dogs;
        }

        public async Task<DogEntity> GetDogByName(string name)
        {
            var dogRep = _unitOfWork.GetRepository<DogEntity>() as IDogRepository;

            var dbDog = dogRep.GetByName(name);

            return dbDog;
        }

        private IEnumerable<DogEntity> OrderingDogs(IEnumerable<DogEntity> dogs,string attribute, string order)
        {
            Expression<Func<DogEntity, object>> orderByExpression = null;

            switch (attribute.ToLower())
            {
                case "color":
                    orderByExpression = dog => dog.Color;
                    break;
                case "name":
                    orderByExpression = dog => dog.Name;
                    break;
                case "taillength":
                    orderByExpression = dog => dog.TailLength;
                    break;
                case "weight":
                    orderByExpression = dog => dog.Weight;
                    break;
                default:
                    orderByExpression = dog => dog.Name;
                    break;
            }


            if (order.ToLower() == "desc")
            {
                dogs = dogs.AsQueryable().OrderByDescending(orderByExpression).ToList();
            }
            else
            {
                dogs = dogs.AsQueryable().OrderBy(orderByExpression).ToList();
            }

            return dogs;
        }

    }
}
