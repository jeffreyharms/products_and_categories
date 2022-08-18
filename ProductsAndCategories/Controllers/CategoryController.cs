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
        ViewBag.AllCategories = _context.Categories.ToList();
        return View("AllCategories");
    }
    [HttpGet("/categories/{CategoryId}")]
    public IActionResult ViewCategory(int categoryId)
    {

    Category? category = _context.Categories
        .Include(f => f.Associations)
        .ThenInclude(assoc => assoc.Product)
        .FirstOrDefault(category => category.CategoryId == categoryId);
        if (category == null)
        {
            return RedirectToAction("All");
        }

        ViewBag.UnrelatedProds = _context.Products
        .Include(c => c.Associations)
        .ThenInclude(a => a.Category)
        .Where(c => !c.Associations
        .Any(p => p.CategoryId == categoryId));

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
    [HttpPost("/categories/{categoryId}/add")]
    public IActionResult AddProds(Association newAssoc, int categoryId)
    {
        if (ModelState.IsValid == false)
        {
            return RedirectToAction("All");
        }
        _context.Associations.Add(newAssoc);
        _context.SaveChanges();
        return RedirectToAction("ViewCategory", new {categoryId = categoryId});
    }
}

