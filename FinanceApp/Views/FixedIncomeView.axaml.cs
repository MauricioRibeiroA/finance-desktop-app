using Avalonia.Controls;
using Avalonia.Interactivity;
using FinanceApp.ViewModels;

namespace FinanceApp.Views;

public partial class FixedIncomeView : Window
{
    public FixedIncomeView()
    {
        InitializeComponent();
        DataContext = new FixedIncomeViewModel();
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        // Volta para a tela de seleção de tipo de investimento
        var investmentTypeView = new InvestmentTypeView();
        investmentTypeView.Show();
        Close();
    }

    private void OnTesouroClick(object? sender, RoutedEventArgs e)
    {
        // Abrir formulário do Tesouro Direto
        var tesouroView = new TesouroDiretoView();
        tesouroView.Show();
        Close();
    }

    private void OnCDBClick(object? sender, RoutedEventArgs e)
    {
        // TODO: Implementar CDB/LCI/LCA no futuro
        ShowMessage("CDB / LCI / LCA", 
                   "🚀 Funcionalidade em desenvolvimento!\n\n" +
                   "Em breve você poderá registrar:\n" +
                   "• CDB - Certificado de Depósito Bancário\n" +
                   "• LCI - Letra de Crédito Imobiliário\n" +
                   "• LCA - Letra de Crédito do Agronegócio\n" +
                   "• Debêntures\n" +
                   "• CRI/CRA\n" +
                   "• E outros títulos privados!");
    }

    private async void ShowMessage(string title, string message)
    {
        try
        {
            var messageBox = new Window
            {
                Title = title,
                Width = 400,
                Height = 350,
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
