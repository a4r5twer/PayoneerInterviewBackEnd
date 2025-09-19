namespace InterviewBackEnd.Model.DAO
{
    public class Stock
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Inventory { get; set; }
        public bool IsOutOfStock { get; set; }
    }
}
