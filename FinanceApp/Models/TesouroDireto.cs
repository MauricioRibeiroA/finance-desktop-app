using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models;

public class TesouroDireto
{
    public int Id { get; set; }
    
    [Required]
    public DateTime DataOperacao { get; set; }
    
    [Required]
    public TipoMovimento TipoMovimento { get; set; }
    
    [Required]
    public ClasseTitulo ClasseTitulo { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string NomeTitulo { get; set; } = string.Empty;
    
    [Required]
    public DateTime DataVencimento { get; set; }
    
    [Required]
    public Indexador Indexador { get; set; }
    
    [Required]
    public decimal QuantidadeTitulos { get; set; }
    
    [Required]
    public decimal PULimpo { get; set; } // Preço Unitário Limpo
    
    public decimal JurosAcumulados { get; set; }
    
    [Required]
    public decimal PUSujo { get; set; } // Preço Unitário Sujo
    
    [Required]
    public decimal ValorBruto { get; set; }
    
    public decimal TaxasEmolumentos { get; set; }
    
    public decimal IRRetido { get; set; }
    
    public decimal OutrosCustos { get; set; }
    
    [Required]
    public decimal ValorLiquido { get; set; }
    
    // Campos de auditoria
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataAtualizacao { get; set; } = DateTime.Now;
    
    [MaxLength(500)]
    public string? Observacoes { get; set; }
}

// Enums para os dropdowns
public enum TipoMovimento
{
    Compra,
    Venda,
    Cupom,
    Amortizacao,
    Taxa,
    Custodia,
    IR,
    Outros
}

public enum ClasseTitulo
{
    TesouroPrefixado,
    TesouroPrefixadoComJS,
    TesouroSelic,
    TesouroIPCAMais,
    TesouroIPCAMaisComJS
}

public enum Indexador
{
    Prefixado,
    Selic,
    IPCA
}
