using Application.Use_Cases.Queries;
using Application.DTOs;
using IntelligentOnlineLearningManagementSystem;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTests
{
    public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IMediator> _mediatorMock;

        public UsersControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _mediatorMock = new Mock<IMediator>();
        }
        /*
        [Fact]
        public async Task GetAllUsers_ReturnsOkResult()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = Guid.NewGuid(),
                    Email = "john.dohe@example.com",
                    PasswordHash = "hashedpassword",
                    Admin = true
                }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUsersQuery>(), default)).ReturnsAsync(users);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_mediatorMock.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/users");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("john.dohe@example.com", responseString);
        }*/
    }
}