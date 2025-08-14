using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    [Required]
    public CategoryType Type { get; set; }
    
    public string Color { get; set; } = "#FF6B6B"; // Cor padrão para UI
    
    public string Icon { get; set; } = "💰"; // Ícone emoji padrão
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Relacionamento com transações
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

public enum CategoryType
{
    Income = 1,    // Para receitas
    Expense = 2    // Para despesas
}
