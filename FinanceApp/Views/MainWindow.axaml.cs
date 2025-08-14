using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace FinanceApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnLaunchButtonClick(object? sender, RoutedEventArgs e)
    {
        // TODO: Abrir tela de lançamento
        // Por enquanto, vamos mostrar uma mensagem
        ShowMessage("Funcionalidade de Lançamento", 
                   "🚀 Esta funcionalidade permitirá:\n\n" +
                   "• Adicionar novas receitas\n" +
                   "• Registrar despesas\n" +
                   "• Categorizar transações\n" +
                   "• Adicionar notas e detalhes\n\n" +
                   "Em breve!");
    }

    private void OnDashboardButtonClick(object? sender, RoutedEventArgs e)
    {
        // TODO: Abrir tela de dashboard
        // Por enquanto, vamos mostrar uma mensagem
        ShowMessage("Dashboard Financeiro", 
                   "📊 Esta funcionalidade permitirá:\n\n" +
                   "• Visualizar relatórios mensais\n" +
                   "• Gráficos por categoria\n" +
                   "• Análise de gastos\n" +
                   "• Histórico de transações\n" +
                   "• Exportar dados\n\n" +
                   "Em breve!");
    }

    private async void ShowMessage(string title, string message)
    {
        try
        {
            var messageBox = new Window
            {
                Title = title,
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false
            };

            var panel = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Spacing = 20
            };

            var messageText = new TextBlock
            {
                Text = message,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                TextAlignment = Avalonia.Media.TextAlignment.Left,
                FontSize = 14
            };

            var okButton = new Button
            {
                Content = "OK",
                Classes = { "PrimaryButton" },
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                MinWidth = 80
            };

            okButton.Click += (s, e) => messageBox.Close();

            panel.Children.Add(messageText);
            panel.Children.Add(okButton);
            messageBox.Content = panel;

            await messageBox.ShowDialog(this);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao exibir mensagem: {ex.Message}");
        }
    }
}
