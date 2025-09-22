using BudgetApp.Models;
using BudgetApp.Models.Enums;

namespace BudgetApp.ViewModels;

public class TransactionListViewModel
{
    public IEnumerable<Transaction> Transactions { get; set; } = [];

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public string? SearchString { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? CategoryId { get; set; }
}
