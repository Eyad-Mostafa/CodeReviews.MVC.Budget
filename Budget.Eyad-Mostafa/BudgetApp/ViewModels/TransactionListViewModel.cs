using BudgetApp.Models;

namespace BudgetApp.ViewModels;

public class TransactionListViewModel
{
    public List<Transaction> Transactions { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string SearchString { get; set; }
}
