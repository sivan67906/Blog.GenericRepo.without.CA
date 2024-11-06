using Blog.API.Models; 
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data;

public class BlogDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Blogs> Blogs { get; set; }

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Data Source=LAPTOP-5K3IK67M;Initial Catalog=NorthWind;Integrated Security=True;Trust Server Certificate=True;MultipleActiveResultSets=true");
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.ProductId);
    }
}
