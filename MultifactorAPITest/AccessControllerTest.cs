using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace MultifactorAPITest
{
    public class AccessControllerTest
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AccessControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task PostAccessAsync_ReturnedBadRequestIfNotValidResource(string resource)
        {
            // Arrange
            var client = _factory.CreateClient();
            var content = TestDataHelper.CreateGrantAccessRequest(resource);

            // Act
            var response = await client.PostAsync(TestDataHelper.AccessUri, content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}