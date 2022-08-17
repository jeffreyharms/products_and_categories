using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Controllers;

public class CategoryController : Controller
{
    private ProductsAndCategoriesContext _context;
    public CategoryController(ProductsAndCategoriesContext context)
    {
        _context = context;
    }
    [HttpGet("/categories")]
    public IActionResult All()
    {
        List<Category> AllCategories = _context.Categories.ToList();
        return View("AllCategories");
    }
    [HttpGet("/categories/{CategoryId}")]
    public IActionResult ViewCategory(int categoryId)
    {
        Category? category = _context.Categories.FirstOrDefault(cat => cat.CategoryId == categoryId);
        if (category == null)
        {
            return RedirectToAction("All");
        }

        ViewBag.Prods = _context.Products
            .Include(cat => cat.Associations)
                .ThenInclude(assoc => assoc.Category)
            .Where(Category => Category.Associations.Any(p => p.CategoryId == categoryId))
            .ToList();

        ViewBag.UnrelatedCats = _context.Categories
        .Include(c => c.Associations)
        .Where(c => !c.Associations.Any(p => p.CategoryId == categoryId));

        return View("OneCategory", category);
    }
    [HttpGet("/categories/new")]
    public IActionResult New()
    {
        return View("AllCategories");
    }
    [HttpPost("/categories/create")]
    public IActionResult Create(Category newCategory)
    {
        if (ModelState.IsValid == false)
        {
            return New();
        }
        _context.Categories.Add(newCategory);
        _context.SaveChanges();
        return RedirectToAction("All");
    }
}

