using BudgetApp.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

public class Transaction
{
    public int TransactionId { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [StringLength(250, ErrorMessage = "Description cannot be longer than 250 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Transaction type is required.")]
    public TransactionType Type { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [ValidateNever]
    public Category Category { get; set; } = null!;
}
