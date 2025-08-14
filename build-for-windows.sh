#!/bin/bash

# Script para compilar o Finance Desktop App para Windows com ícone
echo "🚀 Compilando Finance Desktop App para Windows..."
echo "💰 Com ícone financeiro personalizado"
echo ""

cd FinanceApp

# Verificar se o ícone existe
if [ ! -f "Assets/finance-icon.ico" ]; then
    echo "❌ Ícone não encontrado! Executando conversão..."
    cd Assets
    if [ -f "finance-icon.svg" ]; then
        convert finance-icon.svg -resize 256x256 -background transparent finance-icon.png
        convert finance-icon.png finance-icon.ico
        echo "✅ Ícone convertido com sucesso!"
    else
        echo "❌ Arquivo SVG não encontrado!"
        exit 1
    fi
    cd ..
fi

# Limpar builds anteriores
echo "🧹 Limpando builds anteriores..."
dotnet clean -q

# Publicar para Windows x64 com todas as otimizações
echo "📦 Publicando para Windows x64..."
echo "   • Arquivo único (single file)"
echo "   • Auto-contido (self-contained)"
echo "   • Com ícone personalizado"
echo "   • Metadados da aplicação"
echo ""

dotnet publish -c Release -r win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:EnableCompressionInSingleFile=true \
  -p:DebugType=None \
  -p:DebugSymbols=false

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Build concluído com sucesso!"
    echo ""
    echo "📂 Executável criado:"
    echo "   $(pwd)/bin/Release/net8.0/win-x64/publish/FinanceApp.exe"
    echo ""
    # Verificar tamanho do arquivo
    SIZE=$(du -h "bin/Release/net8.0/win-x64/publish/FinanceApp.exe" | cut -f1)
    echo "💾 Tamanho do executável: $SIZE"
    echo ""
    echo "🎨 Recursos incluídos:"
    echo "   ✅ Ícone financeiro personalizado ($ dourado)"
    echo "   ✅ Tema dark profissional"
    echo "   ✅ Banco SQLite integrado"
    echo "   ✅ Interface responsiva"
    echo ""
    echo "💡 Como usar no Windows:"
    echo "   1. Copie FinanceApp.exe para seu PC Windows"
    echo "   2. Execute com duplo clique"
    echo "   3. O ícone aparecerá na taskbar!"
    echo "   4. Banco de dados criado automaticamente"
    echo ""
    echo "📊 Localização dos dados no Windows:"
    echo "   C:\\Users\\[Usuario]\\AppData\\Local\\FinanceApp\\finance.db"
    echo ""
    echo "🚀 Pronto para usar! Seu app tem visual profissional."
else
    echo "❌ Erro na compilação!"
    exit 1
fi
