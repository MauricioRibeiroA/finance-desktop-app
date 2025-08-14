using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Models;

namespace FinanceApp.Data;

public class FinanceDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }

    public FinanceDbContext()
    {
    }

    public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Obtém o diretório onde o executável está rodando
            var databasePath = GetDatabasePath();
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações para Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            // Relacionamento com Category
            entity.HasOne(t => t.Category)
                  .WithMany(c => c.Transactions)
                  .HasForeignKey(t => t.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Configurações para Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Color).HasMaxLength(7); // Para hex colors (#FFFFFF)
            entity.Property(e => e.Icon).HasMaxLength(10);
        });

        // Seed data - Categorias padrão
        SeedDefaultCategories(modelBuilder);
    }

    private static string GetDatabasePath()
    {
        // Para desenvolvimento no Linux e produção no Windows
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        
        if (string.IsNullOrEmpty(appDataPath))
        {
            // Fallback para sistemas Linux/macOS durante desenvolvimento
            appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            appDataPath = Path.Combine(appDataPath, ".financeapp");
        }
        else
        {
            // Windows: AppData\Local\FinanceApp
            appDataPath = Path.Combine(appDataPath, "FinanceApp");
        }

        // Cria o diretório se não existir
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }

        return Path.Combine(appDataPath, "finance.db");
    }

    private static void SeedDefaultCategories(ModelBuilder modelBuilder)
    {
        // Categorias de Receita
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Salário", Type = CategoryType.Income, Color = "#4CAF50", Icon = "💰" },
            new Category { Id = 2, Name = "Freelance", Type = CategoryType.Income, Color = "#8BC34A", Icon = "💻" },
            new Category { Id = 3, Name = "Investimentos", Type = CategoryType.Income, Color = "#2196F3", Icon = "📈" },
            new Category { Id = 4, Name = "Outros", Type = CategoryType.Income, Color = "#9E9E9E", Icon = "💵" }
        );

        // Categorias de Despesa
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 5, Name = "Alimentação", Type = CategoryType.Expense, Color = "#FF5722", Icon = "🍽️" },
            new Category { Id = 6, Name = "Transporte", Type = CategoryType.Expense, Color = "#607D8B", Icon = "🚗" },
            new Category { Id = 7, Name = "Moradia", Type = CategoryType.Expense, Color = "#795548", Icon = "🏠" },
            new Category { Id = 8, Name = "Saúde", Type = CategoryType.Expense, Color = "#E91E63", Icon = "🏥" },
            new Category { Id = 9, Name = "Educação", Type = CategoryType.Expense, Color = "#9C27B0", Icon = "📚" },
            new Category { Id = 10, Name = "Lazer", Type = CategoryType.Expense, Color = "#FF9800", Icon = "🎮" },
            new Category { Id = 11, Name = "Roupas", Type = CategoryType.Expense, Color = "#E91E63", Icon = "👕" },
            new Category { Id = 12, Name = "Outros", Type = CategoryType.Expense, Color = "#9E9E9E", Icon = "💸" }
        );
    }

    public async Task InitializeDatabaseAsync()
    {
        try
        {
            // Cria o banco se não existir e aplica migrations
            await Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            // Log do erro - em produção você pode usar um logger real
            Console.WriteLine($"Erro ao inicializar banco de dados: {ex.Message}");
            throw;
        }
    }
}
