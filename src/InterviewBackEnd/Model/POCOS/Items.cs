using System.Text.Json.Serialization;

namespace InterviewBackEnd.Model.POCOS
{
    public class Items
    {
        [JsonPropertyName("ProductId")]
        public int ProductId { get; set; }
        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }
    }
}
