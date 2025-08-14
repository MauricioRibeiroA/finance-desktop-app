#!/bin/bash

# Script para compilar o app para Windows
echo "🚀 Compilando Finance Desktop App para Windows..."

cd FinanceApp

# Limpar builds anteriores
echo "🧹 Limpando builds anteriores..."
dotnet clean

# Publicar para Windows x64
echo "📦 Publicando para Windows x64..."
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

if [ $? -eq 0 ]; then
    echo "✅ Build concluído com sucesso!"
    echo ""
    echo "📂 Arquivo executável criado em:"
    echo "   $(pwd)/bin/Release/net8.0/win-x64/publish/FinanceApp.exe"
    echo ""
    echo "💡 Para usar no Windows:"
    echo "   1. Copie o arquivo FinanceApp.exe para seu PC Windows"
    echo "   2. Execute o arquivo (duplo clique)"
    echo "   3. O banco SQLite será criado automaticamente!"
    echo ""
    echo "📊 O banco de dados ficará em:"
    echo "   C:\\Users\\[Usuario]\\AppData\\Local\\FinanceApp\\finance.db"
else
    echo "❌ Erro na compilação!"
    exit 1
fi
