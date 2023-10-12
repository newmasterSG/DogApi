using Dogs.Application.DTO;
using Dogs.Domain.Entity;

namespace Dogs.Application.Interfaces
{
    public interface IDogService
    {
        Task AddSync(DogDTO dog);
        Task<bool> DeleteAsync(int id);
        Task<List<DogDTO>> GetAllDogsAsync(int pageNumber, int pageSize, string attribute = "", string order = "asc");
        Task UpdateAsync(int id, DogDTO dTO);
        Task<DogEntity> GetDogByNameAsync(string name);
        Task<DogDTO> GetDogByIdAsync(int id);
    }
}