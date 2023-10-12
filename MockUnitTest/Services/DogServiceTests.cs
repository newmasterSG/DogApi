using Dogs.Application.Services;
using Dogs.Domain.Entity;
using Dogs.Infrastructure.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockUnitTest.Services
{
    public class DogServiceTests
    {
        [Theory]
        [InlineData(1, "Fido", "Brown", 10, 50)]
        [InlineData(2, "Buddy", "Black", 12, 60)]
        [InlineData(3, "Max", "White", 8, 45)]
        public async Task GetDogByIdAsync_ShouldReturnDog(int dogId, string name, string color, int tailLength, int weight)
        {
            // Arrange
            var unitOfWorkMock = MockFactory.CreateUnitOfWorkMock();
            var dogRepositoryMock = MockFactory.CreateDogRepositoryMock();

            unitOfWorkMock.Setup(uow => uow.GetRepository<DogEntity>()).Returns(dogRepositoryMock.Object);

            var dogService = new DogService(unitOfWorkMock.Object);

            var expectedDog = MockFactory.CreateDogEntity(dogId, name, color, tailLength, weight);

            dogRepositoryMock.Setup(repo => repo.GetEntityAsync(dogId)).ReturnsAsync(expectedDog);

            // Act
            var result = await dogService.GetDogByIdAsync(dogId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDog.Name, result.Name);
            Assert.Equal(expectedDog.Color, result.Color);
            Assert.Equal(expectedDog.TailLength, result.TailLength);
            Assert.Equal(expectedDog.Weight, result.Weight);
        }
    }
}
