using BudgetApp.Data;
using BudgetApp.Models;
using BudgetApp.Models.Enums;

namespace BudgetApp.DataSeeder;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Food" },
                new() { Name = "Rent" },
                new() { Name = "Salary" },
                new() { Name = "Transport" },
                new() { Name = "Shopping" },
                new() { Name = "Health" },
                new() { Name = "Entertainment" },
                new() { Name = "Utilities" },
                new() { Name = "Education" },
                new() { Name = "Other" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            var rnd = new Random();
            var transactions = new List<Transaction>();
            for (int i = 0; i < 1000; i++)
            {
                transactions.Add(new Transaction
                {
                    Amount = rnd.Next(10, 500),
                    Date = DateTime.Now.AddDays(-rnd.Next(0, 365)),
                    Description = $"Transaction {i + 1}",
                    Type = rnd.Next(0, 2) == 0 ? TransactionType.Income : TransactionType.Expense,
                    CategoryId = categories[rnd.Next(categories.Count)].CategoryId
                });
            }

            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }
    }
}
