
using Microsoft.EntityFrameworkCore;
using CafeteriaCodeFirst.Models;

namespace CafeteriaCodeFirst.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(c => c.Name).HasMaxLength(100);

            modelBuilder.Entity<Category>()
                .Property(c => c.Description).HasMaxLength(255);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Name).HasMaxLength(100);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Email).HasMaxLength(100);

            modelBuilder.Entity<Supplier>()
                .Property(s => s.Phone).HasMaxLength(20);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name).HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Product>()
                .ToTable(t => t.HasCheckConstraint("CK_Product_Price", "[Price] >= 0"));

            modelBuilder.Entity<Product>()
                .ToTable(t => t.HasCheckConstraint("CK_Product_Quantity", "[Quantity] >= 0"));

            modelBuilder.Entity<PurchaseOrder>()
                .Property(p => p.Status).HasMaxLength(50);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.UnitPrice).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<PurchaseOrderItem>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0"));

            modelBuilder.Entity<PurchaseOrderItem>()
                .ToTable(t => t.HasCheckConstraint("CK_OrderItem_Price", "[UnitPrice] >= 0"));


            modelBuilder.Entity<Product>()
        .HasOne(p => p.Category)
        .WithMany()
        .HasForeignKey(p => p.CategoryId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
        .HasOne(p => p.Supplier)
        .WithMany()
        .HasForeignKey(p => p.SupplierId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PurchaseOrder>()
        .HasOne(p => p.Supplier)
        .WithMany()
        .HasForeignKey(p => p.SupplierId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PurchaseOrderItem>()
        .HasOne(p => p.PurchaseOrder)
        .WithMany()
        .HasForeignKey(p => p.PurchaseOrderId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PurchaseOrderItem>()
        .HasOne(p => p.Product)
        .WithMany()
        .HasForeignKey(p => p.ProductId)
        .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
