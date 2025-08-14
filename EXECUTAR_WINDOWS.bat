@echo off
echo.
echo ======================================
echo    Finance Control - Desktop App
echo ======================================
echo.
echo Iniciando o aplicativo...
echo.

REM Verificar se o .NET esta instalado
where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo ❌ .NET nao encontrado!
    echo.
    echo Por favor, instale o .NET Runtime:
    echo https://dotnet.microsoft.com/download
    echo.
    echo Baixe: ".NET 8.0 Runtime"
    echo.
    pause
    start "" "https://dotnet.microsoft.com/download"
    exit /b 1
)

REM Navegar para a pasta do projeto
if exist "FinanceApp" (
    cd FinanceApp
) else (
    echo ❌ Pasta FinanceApp nao encontrada!
    echo Certifique-se de que voce esta na pasta correta.
    echo.
    pause
    exit /b 1
)

echo ✅ .NET encontrado!
echo 🚀 Iniciando Finance Control...
echo.
echo 💡 Aguarde 10-15 segundos na primeira execucao
echo.

REM Executar o aplicativo
dotnet run

REM Se chegou aqui, provavelmente houve um erro
echo.
echo ⚠️  O aplicativo foi fechado.
echo.
pause
