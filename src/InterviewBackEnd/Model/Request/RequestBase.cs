using System.Text.Json.Serialization;
namespace InterviewBackEnd.Model.Request
{
    public class RequestBase
    {
        [JsonPropertyName("RequestId")]
        public Guid RequestId { get; set; }
    }
}
