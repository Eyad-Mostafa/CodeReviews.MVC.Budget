using BudgetApp.Data;
using BudgetApp.Models;
using BudgetApp.Models.Enums;
using BudgetApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers;

public class TransactionsController(AppDbContext context) : Controller
{
    private readonly AppDbContext _context = context;

    public IActionResult Index(string? searchString, DateTime? startDate, DateTime? endDate, int? categoryId, int page = 1)
    {
        const int PageSize = 20;
        var query = _context.Transactions.Include(t => t.Category).AsQueryable();

        // Search
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(t => t.Description.Contains(searchString) || t.Category.Name.Contains(searchString));
        }

        // Date filter
        if (startDate.HasValue)
            query = query.Where(t => t.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(t => t.Date <= endDate.Value);

        // Category filter
        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }
        var categories = _context.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, "CategoryId", "Name", categoryId);

        // Pagination
        var totalItems = query.Count();
        var transactions = query
            .OrderByDescending(t => t.Date)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        var vm = new TransactionListViewModel
        {
            Transactions = transactions,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize),
            SearchString = searchString,
            StartDate = startDate,
            EndDate = endDate,
            CategoryId = categoryId
        };

        return View(vm);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = _context.Categories.ToList();
        var model = new Transaction
        {
            Date = DateTime.Now
        };
        return PartialView("_CreateEdit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        ViewBag.Categories = _context.Categories.ToList();
        return PartialView("_CreateEdit", transaction);
    }

    public IActionResult Edit(int id)
    {
        var transaction = _context.Transactions.Find(id);
        if (transaction == null) return NotFound();

        ViewBag.Categories = _context.Categories.ToList();
        return PartialView("_CreateEdit", transaction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        ViewBag.Categories = _context.Categories.ToList();
        return PartialView("_CreateEdit", transaction);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TransactionId == id);

        if (transaction == null) return NotFound();

        return PartialView("_Delete", transaction);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return NotFound();

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return Json(new { success = true });
    }
}