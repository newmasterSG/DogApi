using Dogs.Application.DTO;
using Dogs.Application.Interfaces;
using Dogs.Domain.Entity;
using Dogs.Infrastructure.Interfaces;

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

        public async Task<List<DogDTO>> GetAllDogs()
        {
            var dogsDb = await _unitOfWork.GetRepository<DogEntity>().GetAllAsync();
            List<DogDTO> dogs = new List<DogDTO>();

            if(dogsDb.Any())
            {
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

    }
}
