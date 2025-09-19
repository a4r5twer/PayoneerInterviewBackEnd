using InterviewBackEnd.Model.DAO;
using Microsoft.EntityFrameworkCore;
namespace InterviewBackEnd.DataAccess
{
    public class OrderProcessContext : DbContext
    {
        public OrderProcessContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Stock> ProductStock { get; set; }
        public DbSet<OrderedItem> OrderedItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(z => z.OrderId);
                entity.Property(z => z.CustomerName).IsRequired().HasMaxLength(50);
                entity.Property(z => z.CreatedAt).IsRowVersion();
                entity.HasMany(z => z.OrderedItems)
                      .WithOne(z => z.Order)
                      .HasForeignKey(z => z.OrderId);
            });
            modelBuilder.Entity<OrderedItem>(entity =>
            {
                entity.ToTable("OrderedItems");
                entity.ToTable(z => z.HasCheckConstraint("CK_OrderedItems_Quantity_Positive", "[Quantity] > 0"));
                entity.HasKey(z => z.Id);
                entity.Property(x => x.Quantity)
                      .IsRequired();
            });
            modelBuilder.Entity<OrderedItem>(entity =>
            {
                entity.ToTable("OrderedItems");
                entity.ToTable(z => z.HasCheckConstraint("CK_OrderedItems_Quantity_Positive", "[Quantity] > 0"));
                entity.HasKey(z => z.Id);
                entity.Property(x => x.Quantity)
                      .IsRequired();
                // Relationship: Each OrderedItem references a Stock (Product)
                entity.HasOne(z => z.Stock)
                      .WithMany()
                      .HasForeignKey(z => z.Id)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("ProductStock");
                entity.HasKey(z => z.Id);
                entity.ToTable(z => z.HasCheckConstraint("CK_ProductStock_Inventory_Positive", "[Inventory] >= 0"));
                entity.Property(z => z.ProductName).IsRequired().HasMaxLength(20);
            });
            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Replace with your actual connection string
        //    var connectionString = "Data Source=SHA-YFZHAO-L1\\SQLEXPRESS;Initial Catalog=BackEnd;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=False;Trust Server Certificate=False;Command Timeout=0";
        //    optionsBuilder.UseSqlServer(connectionString);
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
