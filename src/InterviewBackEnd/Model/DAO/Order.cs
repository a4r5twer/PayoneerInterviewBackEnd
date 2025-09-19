namespace InterviewBackEnd.Model.DAO
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderedItem> OrderedItems { get; set; } = new();
        public byte[] CreatedAt { get; set; }
    }
}
