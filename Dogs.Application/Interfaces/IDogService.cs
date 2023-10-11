using Dogs.Application.DTO;
using Dogs.Domain.Entity;

namespace Dogs.Application.Interfaces
{
    public interface IDogService
    {
        Task AddSync(DogDTO dog);
        Task DeleteAsync(int id);
        Task<List<DogDTO>> GetAllDogs(string attribute = "", string order = "asc");
        Task UpdateAsync(DogEntity dog);
        Task<DogEntity> GetDogByName(string name);
    }
}