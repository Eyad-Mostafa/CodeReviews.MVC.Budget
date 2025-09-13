using BudgetApp.Data;
using BudgetApp.Models;
using BudgetApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers;

public class TransactionsController(AppDbContext context) : Controller
{
    private readonly AppDbContext _context = context;

    public async Task<IActionResult> Index(int page = 1, string searchString = "")
    {
        int pageSize = 20;

        IQueryable<Transaction> query = _context.Transactions
            .AsNoTracking()
            .Include(t => t.Category)
            .OrderByDescending(t => t.Date);

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(t => t.Description.ToUpper().Contains(searchString.ToUpper()));
        }

        var totalCount = await query.CountAsync();

        var transactions = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var vm = new TransactionListViewModel
        {
            Transactions = transactions,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            SearchString = searchString
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