using Avalonia.Controls;
using Avalonia.Interactivity;
using FinanceApp.ViewModels;

namespace FinanceApp.Views;

public partial class TesouroDiretoView : Window
{
    private readonly TesouroDiretoViewModel _viewModel;

    public TesouroDiretoView()
    {
        InitializeComponent();
        _viewModel = new TesouroDiretoViewModel();
        DataContext = _viewModel;
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        // Voltar para FixedIncomeView
        var fixedIncomeView = new FixedIncomeView();
        fixedIncomeView.Show();
        this.Close();
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        // Limpar formulário ou voltar
        if (_viewModel.HasUnsavedChanges())
        {
            // TODO: Mostrar dialog de confirmação
            // Por enquanto apenas volta
            OnBackClick(sender, e);
        }
        else
        {
            OnBackClick(sender, e);
        }
    }

    private async void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        if (await _viewModel.SaveOperationAsync())
        {
            // Sucesso - mostrar mensagem e voltar ou limpar formulário
            // TODO: Implementar notificação de sucesso
            OnBackClick(sender, e);
        }
        else
        {
            // Erro na validação ou salvamento
            // TODO: Mostrar mensagem de erro
        }
    }
}
