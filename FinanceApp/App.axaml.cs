using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using FinanceApp.ViewModels;
using FinanceApp.Views;
using FinanceApp.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FinanceApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // Inicializa o banco de dados
            await InitializeDatabaseAsync();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private static async Task InitializeDatabaseAsync()
    {
        try
        {
            using var context = new FinanceDbContext();
            await context.InitializeDatabaseAsync();
            
            Console.WriteLine($"Banco de dados inicializado com sucesso!");
            Console.WriteLine($"Localização do banco: SQLite local criado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao inicializar banco de dados: {ex.Message}");
            // Em produção, você pode mostrar uma mensagem para o usuário ou usar logging
        }
    }
}
