# Finance Desktop App

Um aplicativo desktop para controle de finanças pessoais offline, desenvolvido em C# com Avalonia UI.

## Recursos

- 💰 Controle de receitas e despesas
- 📊 Visualização de relatórios financeiros
- 🏷️ Categorização de transações
- 💾 Armazenamento local com SQLite
- 🖥️ Interface moderna com Avalonia UI
- 📱 Multiplataforma (Windows, Linux, macOS)

## Tecnologias

- C# / .NET 8.0
- Avalonia UI (Framework de interface multiplataforma)
- Entity Framework Core com SQLite
- Padrão MVVM com CommunityToolkit.Mvvm
- Newtonsoft.Json para serialização

## Como executar

### 🪟 **Windows (SUPER FÁCIL)**:

**Método 1 - Duplo clique:**
1. **Duplo clique** no arquivo `EXECUTAR_WINDOWS.bat`
2. **Pronto!** ✨ (ele verifica tudo automaticamente)

**Método 2 - Manual:**
1. Baixe .NET Runtime: https://dotnet.microsoft.com/download
2. Abra PowerShell na pasta do projeto
3. Execute: `cd FinanceApp` e `dotnet run`

📖 **Guias**: 
- [INICIO_RAPIDO.md](INICIO_RAPIDO.md) - 3 passos rápidos
- [COMO_USAR_NO_WINDOWS.md](COMO_USAR_NO_WINDOWS.md) - Guia completo

### 🐧 **Linux/macOS**:
```bash
git clone https://github.com/MauricioRibeiroA/finance-desktop-app.git
cd finance-desktop-app/FinanceApp
dotnet restore
dotnet run
```

## Estrutura do Projeto

```
FinanceApp/
├── Models/          # Modelos de dados
├── ViewModels/      # ViewModels (MVVM)
├── Views/           # Telas da interface
├── Services/        # Serviços e lógica de negócio
└── Data/            # Contexto do banco de dados
```

## Contribuição

Este é um projeto pessoal para controle financeiro offline. Sugestões e melhorias são bem-vindas!

## Licença

MIT License
