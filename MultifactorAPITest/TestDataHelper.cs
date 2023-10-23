using MultifactorAPI.DTOModels;
using System.Text.Json;

namespace MultifactorAPITest
{
    internal class TestDataHelper
    {
        internal static string RequestUri => "/api/requests";
        internal static string AccessUri => "/api/access";
        internal static int Timeout => 20000;

        internal static StringContent CreateRequestContent(string resource) =>
            new StringContent(
                JsonSerializer.Serialize(
                    new RequestModel { Resource = resource }),
                    System.Text.Encoding.UTF8,
                    "application/json");

        internal static StringContent CreateGrantAccessRequest(string resource) =>
            new StringContent(
                JsonSerializer.Serialize(
                    new AccessModel
                    {
                        Resource = resource,
                        Action = AccessAction.Grant,
                    }),
                    System.Text.Encoding.UTF8,
                    "application/json");

        internal static StringContent CreateDenyAccessRequest(string resource) =>
            new StringContent(
                JsonSerializer.Serialize(
                    new AccessModel
                    {
                        Resource = resource,
                        Action = AccessAction.Deny,
                    }),
                    System.Text.Encoding.UTF8,
                    "application/json");

    }
}
