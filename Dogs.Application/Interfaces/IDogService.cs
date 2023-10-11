using Dogs.Application.DTO;
using Dogs.Domain.Entity;

namespace Dogs.Application.Interfaces
{
    public interface IDogService
    {
        Task AddSync(DogEntity dog);
        Task DeleteAsync(int id);
        Task<List<DogDTO>> GetAllDogs();
        Task UpdateAsync(DogEntity dog);
    }
}