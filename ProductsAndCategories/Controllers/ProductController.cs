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
        ViewBag.AllProducts = _context.Products.ToList();
        return View("AllProducts");
    }
    [HttpGet("/products/{ProductId}")]
    public IActionResult ViewProduct(int productId)
    {
        Product? product = _context.Products
        .Include(f => f.Associations)
        .ThenInclude(assoc => assoc.Category)
        .FirstOrDefault(product => product.ProductId == productId);

        if (product == null)
        {
            return RedirectToAction("All");
        }

        ViewBag.UnrelatedCats = _context.Categories
        .Include(p => p.Associations)
        .ThenInclude(a => a.Product)
        .Where(c => !c.Associations
        .Any(p => p.ProductId == productId));


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
    [HttpPost("/products/{productId}/add")]
    public IActionResult AddCats(Association newAssoc, int productId)
    {
        if (ModelState.IsValid == false)
        {
            return RedirectToAction("All");
        }
        _context.Associations.Add(newAssoc);
        _context.SaveChanges();
        return RedirectToAction("ViewProduct", new {productId = productId});
    }
}

