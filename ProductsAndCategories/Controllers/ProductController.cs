using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Controllers;

public class ProductController : Controller
{
    private ProductsAndCategoriesContext _context;
    public ProductController(ProductsAndCategoriesContext context)
    {
        _context = context;
    }
    [HttpGet("/products")]
    public IActionResult All()
    {
        List<Product> AllProducts = _context.Products.ToList();
        return View("AllProducts");
    }
    [HttpGet("/products/{ProductId}")]
    public IActionResult ViewProduct(int productId)
    {
        Product? product = _context.Products.FirstOrDefault(product => product.ProductId == productId);
        if (product == null)
        {
            return RedirectToAction("All");
        }

        ViewBag.Cats = _context.Categories
            .Include(Prod => Prod.Associations)
                .ThenInclude(assoc => assoc.Product)
            .Where(Product => Product.Associations.Any(p => p.ProductId == productId))
            .ToList();

        ViewBag.UnrelatedCats = _context.Categories
        .Include(c => c.Associations)
        .Where(c => !c.Associations.Any(p => p.ProductId == productId));

        return View("OneProduct", product);
    }
    [HttpGet("/products/new")]
    public IActionResult New()
    {
        return View("AllProducts");
    }
    [HttpPost("/products/create")]
    public IActionResult Create(Product newProduct)
    {
        if (ModelState.IsValid == false)
        {
            return New();
        }
        _context.Products.Add(newProduct);
        _context.SaveChanges();
        return RedirectToAction("All");
    }
}

