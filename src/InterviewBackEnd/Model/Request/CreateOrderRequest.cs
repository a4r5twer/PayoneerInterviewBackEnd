using InterviewBackEnd.Model.POCOS;
using System.Text.Json.Serialization;
namespace InterviewBackEnd.Model.Request
{
    public class CreateOrderRequest : RequestBase
    {
        [JsonPropertyName("OrderId")]
        public Guid OrderId { get; set; }
        [JsonPropertyName("CustomerName")]
        public string CustomerName { get; set; }
        [JsonPropertyName("Items")]
        public List<Items> Items { get; set; }
        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
