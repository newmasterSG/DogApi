using Dogs.API.Controllers;
using Dogs.Application.DTO;
using Dogs.Application.Interfaces;
using Dogs.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockUnitTest.Controller
{
    public class DogControllerTests
    {
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

        [Fact]
        public async Task GetAllDogsAsync_ReturnsListOfDogs()
        {
            // Arrange
            var dogServiceMock = new Mock<IDogService>();
            dogServiceMock.Setup(service => service.GetAllDogsAsync(1, 10, "name", "asc"))
                .ReturnsAsync(new List<DogDTO> { new DogDTO() });

            var controller = new DogController(dogServiceMock.Object);

            // Act
            var result = await controller.GetAllDogsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<List<DogDTO>>(okResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task GetDogAsync_WithValidId_ReturnsDog()
        {
            // Arrange
            var dogServiceMock = new Mock<IDogService>();
            dogServiceMock.Setup(service => service.GetDogByIdAsync(1))
                .ReturnsAsync(new DogDTO());

            var controller = new DogController(dogServiceMock.Object);

            // Act
            var result = await controller.GetDogAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<DogDTO>(okResult.Value);
        }

        [Theory]
        [MemberData(nameof(DogData))]
        public async Task AddDogAsync_WithValidModel_ReturnsCreatedResponse(DogDTO dogToAdd)
        {
            // Arrange
            var dogServiceMock = new Mock<IDogService>();

            dogServiceMock.Setup(service => service.GetDogByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((DogEntity)null);

            dogServiceMock.Setup(service => service.AddSync(It.IsAny<DogDTO>()))
                        .Returns(Task.CompletedTask).Callback((DogDTO dogToAdds) =>
                        {
                            var dogAdded = new DogEntity
                            {
                                Name = dogToAdd.Name,
                                Color = dogToAdd.Color,
                                Id = 1,
                                TailLength = dogToAdd.TailLength,
                                Weight = dogToAdd.Weight,
                            };

                            dogServiceMock.Setup(service => service.GetDogByNameAsync(It.IsAny<string>()))
                                        .ReturnsAsync(dogAdded);
                        });

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            
            var controller = new DogController(dogServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.Url = mockUrlHelper.Object;

            // Act
            var result = await controller.AddDogAsync(dogToAdd);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var model = Assert.IsType<DogDTO>(createdResult.Value);
            Assert.Equal(dogToAdd.Name, model.Name);
        }
    }
}
