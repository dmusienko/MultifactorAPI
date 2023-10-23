using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MultifactorAPI.DTOModels
{
    public class AccessModel
    {
        [JsonPropertyName("resource")]
        [Required(ErrorMessage = "Не заполнено поле resource")]
        public string? Resource { get; set; }

        [JsonPropertyName("action")]
        [Required(ErrorMessage = "Не заполнено поле action")]
        public AccessAction Action { get; set; }
    }
}
