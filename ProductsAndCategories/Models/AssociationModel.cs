#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsAndCategories.Models;

public class Association
{
    [Key]
    public int AssociationId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public Category? Category { get; set; }
}