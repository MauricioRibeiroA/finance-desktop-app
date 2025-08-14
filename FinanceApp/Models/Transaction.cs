using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models;

public class Transaction
{
    public int Id { get; set; }
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public TransactionType Type { get; set; }
    
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public enum TransactionType
{
    Income = 1,    // Receita
    Expense = 2    // Despesa
}
