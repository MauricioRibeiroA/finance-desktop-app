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

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
cd finance-desktop-app
```

2. Navegue até o projeto:
```bash
cd FinanceApp
```

3. Restaure as dependências:
```bash
dotnet restore
```

4. Execute o aplicativo:
```bash
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
