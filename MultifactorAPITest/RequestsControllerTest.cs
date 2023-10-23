using Microsoft.AspNetCore.Mvc.Testing;
using MultifactorAPI.DTOModels;
using MultifactorAPI.Domain;
using Newtonsoft.Json;
using System.Net;

namespace MultifactorAPITest
{
    public class RequestsControllerTest
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public RequestsControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task PostRequestAsync_ReturnedBadRequestIfNotValidResource(string resource)
        {
            // Arrange
            var client = _factory.CreateClient();
            var content = TestDataHelper.CreateRequestContent(resource);

            // Act
            var response = await client.PostAsync(TestDataHelper.RequestUri, content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
