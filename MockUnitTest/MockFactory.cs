using Dogs.Domain.Entity;
using Dogs.Infrastructure.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockUnitTest
{
    public class MockFactory
    {
        public static Mock<IUnitOfWork> CreateUnitOfWorkMock()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            return unitOfWorkMock;
        }

        public static Mock<IDogRepository> CreateDogRepositoryMock()
        {
            var dogRepositoryMock = new Mock<IDogRepository>();
            return dogRepositoryMock;
        }

        public static DogEntity CreateDogEntity(int dogId, string name, string color, int tailLength, int weight)
        {
            return new DogEntity
            {
                Id = dogId,
                Name = name,
                Color = color,
                TailLength = tailLength,
                Weight = weight
            };
        }
    }
}
