using System.Text.Json.Serialization;

namespace InterviewBackEnd.Model.Response
{
    public class ResponseBase
    {
        [JsonPropertyName("ResponseId")]
        public Guid ResponseId { get; set; }
        [JsonPropertyName("ResponseMessage")]
        public string ResponseMessage { get; set; }
    }
}
