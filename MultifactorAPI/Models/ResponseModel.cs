using MultifactorAPI.Domain;
using System.Text.Json.Serialization;

namespace MultifactorAPI.DTOModels
{
    public class ResponseModel
    {
        [JsonPropertyName("resource")]
        public string? Resource { get; set; }

        [JsonPropertyName("decision")]
        public CredentialStatus? Decision { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; } = string.Empty;

        public ResponseModel() { }

        public ResponseModel(RequestCredential credential)
        {
            Resource = credential.Resource;
            Reason = credential.Reason;
            Decision = credential.Status == CredentialStatus.Granted
                ? CredentialStatus.Granted
                : CredentialStatus.Denied;
        }
    }
}
