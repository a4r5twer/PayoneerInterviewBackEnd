namespace InterviewBackEnd.Model.DAO
{
    public class OrderedItem
    {
        public int OrderedItemKey { get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        public Stock Stock { get; set; }
        public Guid OrderId { get; set; }
    }
}
