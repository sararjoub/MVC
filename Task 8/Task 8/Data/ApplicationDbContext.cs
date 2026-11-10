using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_8.Models;

namespace Task_8.Data
{
    // معناه إن الـ DbContext رح يتعامل مع جداول Identity الموجودة، مثل:
    //- AspNetUsers
    //- AspNetRoles
    //- AspNetUserRoles
    //  بنفس الوقت يستخدم ApplicationUser اللي أضفنا فيه FullName
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
           DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<SupplyRequest> SupplyRequests { get; set; }

        public DbSet<RequestItem> RequestItems { get; set; }


        // ليش ما أضفنا DbSet للـ Users والـ Roles؟

        // IdentityDbContext  أصلًا بجهزهم إلنا، وما بدنا ننشئ جداول مكررةلان 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SupplyRequest>()
                .Property(s => s.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RequestItem>()
                .Property(r => r.Price)
                .HasPrecision(18, 2);
        }
    }
}