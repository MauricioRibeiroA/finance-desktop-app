using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Data;
using FinanceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.ViewModels;

public class TesouroDiretoViewModel : INotifyPropertyChanged
{
    private readonly FinanceDbContext _context;
    
    // Campos privados para armazenar valores
    private DateTime _dataOperacao = DateTime.Today;
    private TipoMovimento _tipoMovimento = TipoMovimento.Compra;
    private ClasseTitulo _classeTitulo = ClasseTitulo.TesouroPrefixado;
    private string _nomeTitulo = string.Empty;
    private DateTime? _dataVencimento;
    private Indexador _indexador = Indexador.Prefixado;
    private decimal _quantidadeTitulos;
    private decimal _puLimpo;
    private decimal _jurosAcumulados;
    private decimal _puSujo;
    private decimal _valorBruto;
    private decimal _taxasEmolumentos;
    private decimal _irRetido;
    private decimal _outrosCustos;
    private decimal _valorLiquido;
    private string _observacoes = string.Empty;

    // Flag para controlar mudanças não salvas
    private bool _hasUnsavedChanges = false;

    public TesouroDiretoViewModel()
    {
        _context = new FinanceDbContext();
        
        // Inicializar listas para dropdowns
        InitializeDropdowns();
        
        // Configurar eventos para cálculo automático
        SetupAutoCalculations();
    }

    #region Properties com NotifyPropertyChanged

    public DateTime DataOperacao
    {
        get => _dataOperacao;
        set
        {
            if (_dataOperacao != value)
            {
                _dataOperacao = value;
                OnPropertyChanged(nameof(DataOperacao));
                _hasUnsavedChanges = true;
            }
        }
    }

    public TipoMovimento TipoMovimento
    {
        get => _tipoMovimento;
        set
        {
            if (_tipoMovimento != value)
            {
                _tipoMovimento = value;
                OnPropertyChanged(nameof(TipoMovimento));
                _hasUnsavedChanges = true;
            }
        }
    }

    public ClasseTitulo ClasseTitulo
    {
        get => _classeTitulo;
        set
        {
            if (_classeTitulo != value)
            {
                _classeTitulo = value;
                OnPropertyChanged(nameof(ClasseTitulo));
                _hasUnsavedChanges = true;
            }
        }
    }

    public string NomeTitulo
    {
        get => _nomeTitulo;
        set
        {
            if (_nomeTitulo != value)
            {
                _nomeTitulo = value;
                OnPropertyChanged(nameof(NomeTitulo));
                _hasUnsavedChanges = true;
            }
        }
    }

    public DateTime? DataVencimento
    {
        get => _dataVencimento;
        set
        {
            if (_dataVencimento != value)
            {
                _dataVencimento = value;
                OnPropertyChanged(nameof(DataVencimento));
                _hasUnsavedChanges = true;
            }
        }
    }

    public Indexador Indexador
    {
        get => _indexador;
        set
        {
            if (_indexador != value)
            {
                _indexador = value;
                OnPropertyChanged(nameof(Indexador));
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal QuantidadeTitulos
    {
        get => _quantidadeTitulos;
        set
        {
            if (_quantidadeTitulos != value)
            {
                _quantidadeTitulos = value;
                OnPropertyChanged(nameof(QuantidadeTitulos));
                CalculateValues();
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal PULimpo
    {
        get => _puLimpo;
        set
        {
            if (_puLimpo != value)
            {
                _puLimpo = value;
                OnPropertyChanged(nameof(PULimpo));
                CalculateValues();
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal JurosAcumulados
    {
        get => _jurosAcumulados;
        set
        {
            if (_jurosAcumulados != value)
            {
                _jurosAcumulados = value;
                OnPropertyChanged(nameof(JurosAcumulados));
                CalculateValues();
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal PUSujo
    {
        get => _puSujo;
        set
        {
            if (_puSujo != value)
            {
                _puSujo = value;
                OnPropertyChanged(nameof(PUSujo));
                CalculateValues();
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal ValorBruto
    {
        get => _valorBruto;
        set
        {
            if (_valorBruto != value)
            {
                _valorBruto = value;
                OnPropertyChanged(nameof(ValorBruto));
                CalculateNetValue();
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal TaxasEmolumentos
    {
        get => _taxasEmolumentos;
        set
        {
            if (_taxasEmolumentos != value)
            {
                _taxasEmolumentos = value;
                OnPropertyChanged(nameof(TaxasEmolumentos));
                CalculateNetValue();
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal IRRetido
    {
        get => _irRetido;
        set
        {
            if (_irRetido != value)
            {
                _irRetido = value;
                OnPropertyChanged(nameof(IRRetido));
                CalculateNetValue();
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal OutrosCustos
    {
        get => _outrosCustos;
        set
        {
            if (_outrosCustos != value)
            {
                _outrosCustos = value;
                OnPropertyChanged(nameof(OutrosCustos));
                CalculateNetValue();
                _hasUnsavedChanges = true;
            }
        }
    }

    public decimal ValorLiquido
    {
        get => _valorLiquido;
        private set
        {
            if (_valorLiquido != value)
            {
                _valorLiquido = value;
                OnPropertyChanged(nameof(ValorLiquido));
            }
        }
    }

    public string Observacoes
    {
        get => _observacoes;
        set
        {
            if (_observacoes != value)
            {
                _observacoes = value;
                OnPropertyChanged(nameof(Observacoes));
                _hasUnsavedChanges = true;
            }
        }
    }

    #endregion

    #region Dropdown Collections

    public ObservableCollection<TipoMovimento> TiposMovimento { get; } = new();
    public ObservableCollection<ClasseTitulo> ClassesTitulo { get; } = new();
    public ObservableCollection<Indexador> Indexadores { get; } = new();

    #endregion

    #region Métodos

    private void InitializeDropdowns()
    {
        // Popular TiposMovimento
        foreach (TipoMovimento tipo in Enum.GetValues<TipoMovimento>())
        {
            TiposMovimento.Add(tipo);
        }

        // Popular ClassesTitulo
        foreach (ClasseTitulo classe in Enum.GetValues<ClasseTitulo>())
        {
            ClassesTitulo.Add(classe);
        }

        // Popular Indexadores
        foreach (Indexador indexador in Enum.GetValues<Indexador>())
        {
            Indexadores.Add(indexador);
        }
    }

    private void SetupAutoCalculations()
    {
        // Os cálculos são feitos nos setters das propriedades
        // Este método pode ser usado para configurações adicionais se necessário
    }

    private void CalculateValues()
    {
        // Calcular PU Sujo = PU Limpo + Juros Acumulados
        if (PULimpo > 0)
        {
            _puSujo = PULimpo + JurosAcumulados;
            OnPropertyChanged(nameof(PUSujo));
        }

        // Calcular Valor Bruto = Quantidade * PU Sujo
        if (QuantidadeTitulos > 0 && PUSujo > 0)
        {
            _valorBruto = QuantidadeTitulos * PUSujo;
            OnPropertyChanged(nameof(ValorBruto));
            CalculateNetValue();
        }
    }

    private void CalculateNetValue()
    {
        // Calcular Valor Líquido
        decimal totalCosts = TaxasEmolumentos + IRRetido + OutrosCustos;
        
        if (TipoMovimento == TipoMovimento.Compra)
        {
            // Em compra: Valor Líquido = Valor Bruto + Custos
            ValorLiquido = ValorBruto + totalCosts;
        }
        else
        {
            // Em venda: Valor Líquido = Valor Bruto - Custos - IR
            ValorLiquido = ValorBruto - totalCosts;
        }
    }

    public bool ValidateForm()
    {
        var errors = new List<string>();

        // Validações obrigatórias
        if (DataOperacao == default)
            errors.Add("Data da operação é obrigatória");

        if (string.IsNullOrWhiteSpace(NomeTitulo))
            errors.Add("Nome do título é obrigatório");

        if (DataVencimento == null)
            errors.Add("Data de vencimento é obrigatória");

        if (QuantidadeTitulos <= 0)
            errors.Add("Quantidade de títulos deve ser maior que zero");

        if (PULimpo <= 0)
            errors.Add("PU Limpo deve ser maior que zero");

        if (PUSujo <= 0)
            errors.Add("PU Sujo deve ser maior que zero");

        if (ValorBruto <= 0)
            errors.Add("Valor Bruto deve ser maior que zero");

        // Validações lógicas
        if (DataVencimento.HasValue && DataVencimento.Value <= DataOperacao)
            errors.Add("Data de vencimento deve ser posterior à data da operação");

        if (PUSujo < PULimpo)
            errors.Add("PU Sujo não pode ser menor que PU Limpo");

        // TODO: Mostrar erros na interface
        return !errors.Any();
    }

    public async Task<bool> SaveOperationAsync()
    {
        if (!ValidateForm())
            return false;

        try
        {
            var tesouroDireto = new TesouroDireto
            {
                DataOperacao = DataOperacao,
                TipoMovimento = TipoMovimento,
                ClasseTitulo = ClasseTitulo,
                NomeTitulo = NomeTitulo,
                DataVencimento = DataVencimento!.Value,
                Indexador = Indexador,
                QuantidadeTitulos = QuantidadeTitulos,
                PULimpo = PULimpo,
                JurosAcumulados = JurosAcumulados,
                PUSujo = PUSujo,
                ValorBruto = ValorBruto,
                TaxasEmolumentos = TaxasEmolumentos,
                IRRetido = IRRetido,
                OutrosCustos = OutrosCustos,
                ValorLiquido = ValorLiquido,
                Observacoes = Observacoes,
                DataCriacao = DateTime.Now
            };

            _context.TesouroDireto.Add(tesouroDireto);
            await _context.SaveChangesAsync();

            _hasUnsavedChanges = false;
            return true;
        }
        catch (Exception ex)
        {
            // TODO: Implementar logging
            Console.WriteLine($"Erro ao salvar: {ex.Message}");
            return false;
        }
    }

    public bool HasUnsavedChanges()
    {
        return _hasUnsavedChanges;
    }

    public void ClearForm()
    {
        DataOperacao = DateTime.Today;
        TipoMovimento = TipoMovimento.Compra;
        ClasseTitulo = ClasseTitulo.TesouroPrefixado;
        NomeTitulo = string.Empty;
        DataVencimento = null;
        Indexador = Indexador.Prefixado;
        QuantidadeTitulos = 0;
        PULimpo = 0;
        JurosAcumulados = 0;
        PUSujo = 0;
        ValorBruto = 0;
        TaxasEmolumentos = 0;
        IRRetido = 0;
        OutrosCustos = 0;
        ValorLiquido = 0;
        Observacoes = string.Empty;

        _hasUnsavedChanges = false;
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
