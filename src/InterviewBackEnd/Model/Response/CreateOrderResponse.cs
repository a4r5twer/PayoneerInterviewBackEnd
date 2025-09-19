using System.Text.Json.Serialization;

namespace InterviewBackEnd.Model.Response
{
    public class CreateOrderResponse :ResponseBase
    {
        [JsonPropertyName("OrderId")]
        public Guid OrderId { get; set; }
    }
}
