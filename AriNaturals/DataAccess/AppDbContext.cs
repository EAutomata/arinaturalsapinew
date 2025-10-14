using AriNaturals.Entity;
using Microsoft.EntityFrameworkCore;

namespace AriNaturals.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductHighlight> ProductHighlights { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductId)
                .HasDefaultValueSql("NEWID()");

            // Relationships
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variants)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Highlights)
                .WithOne(h => h.Product)
                .HasForeignKey(h => h.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductVariant
            modelBuilder.Entity<ProductVariant>()
                .HasKey(v => v.VariantId);

            modelBuilder.Entity<ProductVariant>()
                .Property(v => v.VariantId)
                .HasDefaultValueSql("NEWID()");

            // ProductImage
            modelBuilder.Entity<ProductImage>()
                .HasKey(i => i.ImageId);

            modelBuilder.Entity<ProductImage>()
                .Property(i => i.ImageId)
                .HasDefaultValueSql("NEWID()");

            // ProductHighlight
            modelBuilder.Entity<ProductHighlight>()
                .HasKey(h => h.HighlightId);

            modelBuilder.Entity<ProductHighlight>()
                .Property(h => h.HighlightId)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<ProductHighlight>()
                .HasMany(h => h.HighlightSections)
                .WithOne(s => s.Highlight)
                .HasForeignKey(s => s.HighlightId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductHighlightSection
            modelBuilder.Entity<ProductHighlightSection>()
                .HasKey(h => h.SectionId);

            modelBuilder.Entity<ProductHighlightSection>()
                .Property(h => h.SectionId)
                .HasDefaultValueSql("NEWID()");

            // ProductReview
            modelBuilder.Entity<ProductReview>()
                .HasKey(r => r.ReviewId);

            modelBuilder.Entity<ProductReview>()
                .Property(r => r.ReviewId)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);

                entity.HasOne(p => p.Order)
                      .WithMany(o => o.Payments)
                      .HasForeignKey(p => p.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            });
        }
    }
}
