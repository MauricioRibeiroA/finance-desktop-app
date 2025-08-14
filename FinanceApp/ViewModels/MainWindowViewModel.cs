namespace FinanceApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // ViewModel para a tela inicial
    // Por enquanto, não precisamos de propriedades específicas
    // Os botões são tratados diretamente no code-behind
    
    public string AppName { get; } = "Finance Control";
    public string AppDescription { get; } = "Controle Financeiro Pessoal";
}
