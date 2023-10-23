using Microsoft.AspNetCore.Mvc.Testing;
using MultifactorAPI.DTOModels;
using MultifactorAPI.Domain;
using Newtonsoft.Json;
using System.Net;

namespace MultifactorAPITest
{
    public class RequestsChainTest
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public RequestsChainTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostRequestAsync_Granted()
        {
            // Arrange
            var resource = Guid.NewGuid().ToString();
            var client = _factory.CreateClient();
            var requestContent = TestDataHelper.CreateRequestContent(resource);
            var accessContent = TestDataHelper.CreateGrantAccessRequest(resource);
            HttpResponseMessage? requestResponse = null;
            HttpResponseMessage? accessResponse = null;

            // Act
            var task1 = client.PostAsync(TestDataHelper.RequestUri, requestContent)
                .ContinueWith(task => { requestResponse = task.Result; });

            await Task.Delay(500);

            var task2 = client.PostAsync(TestDataHelper.AccessUri, accessContent)
                .ContinueWith(task => { accessResponse = task.Result; });

            await Task.WhenAll(new Task[] { task1, task2 });

            // Assert
            Assert.NotNull(requestResponse);
            Assert.NotNull(accessResponse);
            Assert.Equal(HttpStatusCode.OK, accessResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, requestResponse.StatusCode);
            var respObj = JsonConvert.DeserializeObject<ResponseModel>(
                await requestResponse.Content.ReadAsStringAsync());
            Assert.NotNull(respObj);
            Assert.Equal(resource, respObj.Resource);
            Assert.Equal(CredentialStatus.Granted, respObj.Decision);
            Assert.Empty(respObj.Reason);
        }

        [Fact]
        public async Task PostRequestAsync_GrantedNotAllowedIfTimeoutExpired()
        {
            // Arrange
            var resource = Guid.NewGuid().ToString();
            var client = _factory.CreateClient();
            var requestContent = TestDataHelper.CreateRequestContent(resource);
            var accessContent = TestDataHelper.CreateGrantAccessRequest(resource);
            HttpResponseMessage? requestResponse = null;
            HttpResponseMessage? accessResponse = null;

            // Act
            var task1 = client.PostAsync(TestDataHelper.RequestUri, requestContent)
                .ContinueWith(task => { requestResponse = task.Result; });

            await Task.Delay(TestDataHelper.Timeout + 500); // таймаут истек

            var task2 = client.PostAsync(TestDataHelper.AccessUri, accessContent)
                .ContinueWith(task => { accessResponse = task.Result; });

            await Task.WhenAll(new Task[] { task1, task2 });

            // Assert
            Assert.NotNull(requestResponse);
            Assert.NotNull(accessResponse);
            Assert.Equal(HttpStatusCode.OK, accessResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, requestResponse.StatusCode);

            var respObj = JsonConvert.DeserializeObject<ResponseModel>(
                await requestResponse.Content.ReadAsStringAsync());

            Assert.NotNull(respObj);
            Assert.Equal(resource, respObj.Resource);
            Assert.Equal(CredentialStatus.Denied, respObj.Decision);
            Assert.Equal("Timeout expired", respObj.Reason);
        }

        [Fact]
        public async Task PostRequestAsync_DenyByUser()
        {
            // Arrange
            var resource = Guid.NewGuid().ToString();
            var client = _factory.CreateClient();
            var requestContent = TestDataHelper.CreateRequestContent(resource);
            var accessContent = TestDataHelper.CreateDenyAccessRequest(resource);
            HttpResponseMessage? requestResponse = null;
            HttpResponseMessage? accessResponse = null;

            // Act
            var task1 = client.PostAsync(TestDataHelper.RequestUri, requestContent)
                .ContinueWith(task => { requestResponse = task.Result; });

            await Task.Delay(500);

            var task2 = client.PostAsync(TestDataHelper.AccessUri, accessContent)
                .ContinueWith(task => { accessResponse = task.Result; });

            await Task.WhenAll(new Task[] { task1, task2 });

            // Assert
            Assert.NotNull(requestResponse);
            Assert.NotNull(accessResponse);
            Assert.Equal(HttpStatusCode.OK, accessResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, requestResponse.StatusCode);
            var respObj = JsonConvert.DeserializeObject<ResponseModel>(
                await requestResponse.Content.ReadAsStringAsync());
            Assert.NotNull(respObj);
            Assert.Equal(resource, respObj.Resource);
            Assert.Equal(CredentialStatus.Denied, respObj.Decision);
            Assert.Equal("Denied by user", respObj.Reason);
        }

        [Fact]
        public async Task PostRequestAsync_DenyByTimeout()
        {
            // Arrange
            var resource = Guid.NewGuid().ToString();
            var client = _factory.CreateClient();
            var requestContent = TestDataHelper.CreateRequestContent(resource);
            HttpResponseMessage? requestResponse = null;

            // Act
            var task1 = client.PostAsync(TestDataHelper.RequestUri, requestContent)
                .ContinueWith(task => { requestResponse = task.Result; });

            await Task.Delay(TestDataHelper.Timeout + 500); // таймаут истек

            await task1;

            // Assert
            Assert.Equal(HttpStatusCode.OK, requestResponse.StatusCode);
            var respObj = JsonConvert.DeserializeObject<ResponseModel>(
                await requestResponse.Content.ReadAsStringAsync());
            Assert.NotNull(respObj);
            Assert.Equal(resource, respObj.Resource);
            Assert.Equal(CredentialStatus.Denied, respObj.Decision);
            Assert.Equal("Timeout expired", respObj.Reason);
        }

    }
}
