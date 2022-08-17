#pragma warning disable CS8618

using Microsoft.EntityFrameworkCore;
namespace ProductsAndCategories.Models;

public class ProductsAndCategoriesContext : DbContext
{
    public ProductsAndCategoriesContext(DbContextOptions options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Association> Associations { get; set; }
}