#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsAndCategories.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "is required")]
    public string Description { get; set; }
    [Required(ErrorMessage = "is required")]
    public Decimal Price { get; set; }
    [Required(ErrorMessage = "is required")]
    public DateTime Created_At { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "is required")]
    public DateTime Updated_At { get; set; } = DateTime.Now;

    public List<Association> Associations { get; set; } = new List<Association>();
}