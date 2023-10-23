using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MultifactorAPI.DTOModels
{
    public class RequestModel
    {
        [JsonPropertyName("resource")]
        [Required(ErrorMessage = "Не заполнено поле resource")]
        public string? Resource { get; set; }
    }
}
