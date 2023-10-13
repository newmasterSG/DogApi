using Dogs.Application.DTO;
using Dogs.Application.Services;
using Dogs.Domain.Entity;
using Dogs.Infrastructure.Context;
using Dogs.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        #region DataForTest
        public static IEnumerable<object[]> DogData()
        {
            yield return new object[]
            {
                new DogDTO
                {
                    Name = "Buddy",
                    Color = "Brown",
                    TailLength = 12,
                    Weight = 60
                }
            };

            yield return new object[]
            {
                new DogDTO
                {
                    Name = "Max",
                    Color = "Black",
                    TailLength = 8,
                    Weight = 45
                }
            };
        }

        public static IEnumerable<object[]> DogsData()
        {
            yield return new object[] 
            { 
                1, 10, "name", "asc", new List<DogEntity>
                {
                    new DogEntity { Id = 2, Name = "Buddy", Color = "Black", TailLength = 12, Weight = 60 },
                    new DogEntity { Id = 1, Name = "Fido", Color = "Brown", TailLength = 10, Weight = 50 },
                }
            };
            yield return new object[]
            {
                1, 20, "name", "desc", new List<DogEntity>
                {
                    new DogEntity { Id = 3, Name = "Max", Color = "White", TailLength = 8, Weight = 45 }
                }
            };
            yield return new object[] { 3, 30, "color", "asc", new List<DogEntity> { } };
        }

        #endregion

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

        [Theory]
        [MemberData(nameof(DogsData))]
        public async Task GetAllDogsAsync_ShouldReturnDogs(
        int pageNumber, int pageSize, string attribute, string order, List<DogEntity> expectedDogs)
        {
            // Arrange
            var unitOfWorkMock = MockFactory.CreateUnitOfWorkMock();
            var dogRepositoryMock = MockFactory.CreateDogRepositoryMock();

            unitOfWorkMock.Setup(uow => uow.GetRepository<DogEntity>()).Returns(dogRepositoryMock.Object);

            dogRepositoryMock.Setup(repo => repo.TakeAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int skip, int take) => expectedDogs.Skip(skip).Take(take));

            var dogService = new DogService(unitOfWorkMock.Object);

            // Act
            var result = await dogService.GetAllDogsAsync(pageNumber, pageSize, attribute, order);

            // Assert
            Assert.Equal(expectedDogs.Count, result.Count);

            for (int i = 0; i < expectedDogs.Count; i++)
            {
                Assert.Equal(expectedDogs[i].Name, result[i].Name);
                Assert.Equal(expectedDogs[i].Color, result[i].Color);
                Assert.Equal(expectedDogs[i].TailLength, result[i].TailLength);
                Assert.Equal(expectedDogs[i].Weight, result[i].Weight);
            }
        }

        [Theory]
        [MemberData(nameof(DogData))]
        public async Task AddSync_ShouldAddDog(DogDTO dogToAdd)
        {
            // Arrange
            var unitOfWorkMock = MockFactory.CreateUnitOfWorkMock();
            var dogRepositoryMock = MockFactory.CreateDogRepositoryMock();

            unitOfWorkMock.Setup(uow => uow.GetRepository<DogEntity>()).Returns(dogRepositoryMock.Object);

            var dogService = new DogService(unitOfWorkMock.Object);

            // Act
            await dogService.AddSync(dogToAdd);

            // Assert
            dogRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<DogEntity>()), Times.Once);

            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public async Task DeleteAsync_ShouldDeleteDog(int dogId, bool expectedResult)
        {
            // Arrange
            var unitOfWorkMock = MockFactory.CreateUnitOfWorkMock();
            var dogRepositoryMock = MockFactory.CreateDogRepositoryMock();

            unitOfWorkMock.Setup(uow => uow.GetRepository<DogEntity>()).Returns(dogRepositoryMock.Object);

            var dogToDelete = new DogEntity
            {
                Id = dogId,
                Name = "Buddy",
                Color = "Brown",
                TailLength = 12,
                Weight = 60
            };

            dogRepositoryMock.Setup(repo => repo.GetEntityAsync(dogId)).ReturnsAsync(dogToDelete);
            dogRepositoryMock.Setup(repo => repo.Delete(It.IsAny<DogEntity>())).Callback((DogEntity deletedDog) =>
            {
                dogRepositoryMock.Setup(repo => repo.GetEntityAsync(It.IsAny<int>()))
                   .ReturnsAsync((int id) => null);
            });


            unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dogService = new DogService(unitOfWorkMock.Object);

            // Act
            var result = await dogService.DeleteAsync(1);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task UpdateAsync_ShouldUpdateDog(int dogId)
        {
            // Arrange
            var unitOfWorkMock = MockFactory.CreateUnitOfWorkMock();
            var dogRepositoryMock = MockFactory.CreateDogRepositoryMock();

            unitOfWorkMock.Setup(uow => uow.GetRepository<DogEntity>()).Returns(dogRepositoryMock.Object);

            var dogToUpdate = new DogEntity
            {
                Id = dogId,
                Name = "Fido",
                Color = "Brown",
                TailLength = 10,
                Weight = 50
            };

            var updatedDog = new DogDTO
            {
                Name = "Max",
                Color = "Black",
                TailLength = 8,
                Weight = 45
            };

            dogRepositoryMock.Setup(repo => repo.GetEntityAsync(dogId)).ReturnsAsync(dogToUpdate);

            unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dogService = new DogService(unitOfWorkMock.Object);

            // Act
            await dogService.UpdateAsync(dogId, updatedDog);

            // Assert
            Assert.Equal(updatedDog.Name, dogToUpdate.Name);
            Assert.Equal(updatedDog.Color, dogToUpdate.Color);
            Assert.Equal(updatedDog.TailLength, dogToUpdate.TailLength);
            Assert.Equal(updatedDog.Weight, dogToUpdate.Weight);
        }
    }
}
