using Avalonia.Controls;
using Avalonia.Interactivity;
using FinanceApp.ViewModels;

namespace FinanceApp.Views;

public partial class InvestmentTypeView : Window
{
    public InvestmentTypeView()
    {
        InitializeComponent();
        DataContext = new InvestmentTypeViewModel();
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        // Volta para a tela principal
        Close();
    }

    private void OnRendaFixaClick(object? sender, RoutedEventArgs e)
    {
        // Abrir tela de Renda Fixa
        var rendaFixaView = new FixedIncomeView();
        rendaFixaView.Show();
        Close();
    }

    private void OnRendaVariavelClick(object? sender, RoutedEventArgs e)
    {
        // TODO: Implementar Renda Variável no futuro
        ShowMessage("Renda Variável", 
                   "🚀 Funcionalidade em desenvolvimento!\n\n" +
                   "Em breve você poderá registrar:\n" +
                   "• Ações\n" +
                   "• FIIs (Fundos Imobiliários)\n" +
                   "• ETFs\n" +
                   "• Opções\n" +
                   "• E muito mais!");
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
        catch (System.Exception ex)
        {
            System.Console.WriteLine($"Erro ao exibir mensagem: {ex.Message}");
        }
    }
}
