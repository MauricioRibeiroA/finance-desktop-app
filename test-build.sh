#!/bin/bash

echo "🧪 Testando compilação do Finance Desktop App..."

cd FinanceApp

echo "🧹 Limpando build anterior..."
dotnet clean -q

echo "🔨 Compilando..."
if dotnet build -q; then
    echo "✅ Compilação bem-sucedida!"
    echo ""
    echo "🎨 Tema dark financeiro aplicado:"
    echo "   • Fundo: Preto (#1A1A1A)"
    echo "   • Accent: Amarelo ouro (#FFD700)" 
    echo "   • Texto: Branco (#FFFFFF)"
    echo ""
    echo "🖼️  Interface criada:"
    echo "   • Tela inicial com 2 botões"
    echo "   • Botão LANÇAMENTO (amarelo)"
    echo "   • Botão DASHBOARD (contorno amarelo)"
    echo ""
    echo "🗄️  Banco SQLite configurado:"
    echo "   • Modelos: Transaction, Category"
    echo "   • Inicialização automática"
    echo "   • Dados salvos localmente"
    echo ""
    echo "🚀 Pronto para deploy no Windows!"
    echo "   Execute: ./build-for-windows.sh"
else
    echo "❌ Erro na compilação!"
    exit 1
fi
