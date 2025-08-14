# 🗄️ Configuração do Banco de Dados SQLite para Windows

## ✅ **Por que SQLite é perfeito para seu app:**

1. **Zero instalação**: SQLite não precisa ser instalado separadamente no Windows
2. **Portável**: Um único arquivo .db que fica junto com o executável
3. **Automático**: O app cria o banco automaticamente na primeira execução
4. **Simples**: Não precisa configurar servidor, usuário, senha, nada!

## 📍 **Onde o banco será salvo no Windows:**

O aplicativo criará o arquivo `finance.db` em:
```
C:\Users\[SeuUsuario]\AppData\Local\FinanceApp\finance.db
```

Este local é:
- ✅ **Privado** para seu usuário
- ✅ **Seguro** (não será perdido em atualizações)
- ✅ **Padrão do Windows** para dados de aplicativo

## 🚀 **Como funciona na primeira execução:**

1. Você roda o FinanceApp.exe no Windows
2. O app automaticamente:
   - Cria a pasta `C:\Users\[Usuário]\AppData\Local\FinanceApp\`
   - Cria o arquivo `finance.db`
   - Cria todas as tabelas (Transactions, Categories)
   - Insere categorias padrão (Salário, Alimentação, etc.)

## 🛠️ **Como rodar no Windows:**

### Opção 1: Compilar localmente no Windows
Se você tem .NET no Windows:
```bash
dotnet run
```

### Opção 2: Publicar do Linux para Windows
No Linux, você pode compilar para Windows:
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

Isso criará um executável que roda no Windows sem precisar instalar .NET.

## 📊 **Estrutura do Banco de Dados:**

### Tabela: Categories (Categorias)
- Id, Name, Type (Income/Expense), Color, Icon
- **Categorias pré-criadas:**
  - 💰 Receitas: Salário, Freelance, Investimentos
  - 💸 Despesas: Alimentação, Transporte, Moradia, Saúde, etc.

### Tabela: Transactions (Transações)
- Id, Description, Amount, Date, Type, CategoryId, Notes
- **Relacionamento**: Cada transação tem uma categoria

## 🔧 **Ferramentas para visualizar o banco (opcional):**

Se quiser ver os dados diretamente:

1. **DB Browser for SQLite** (gratuito)
   - Download: https://sqlitebrowser.org/
   - Abra o arquivo `finance.db`

2. **Visual Studio Code** com extensão SQLite
   - Instale a extensão "SQLite"
   - Abra o arquivo .db

## 🔄 **Backup e Portabilidade:**

Para fazer backup dos dados:
1. Vá em `C:\Users\[Usuário]\AppData\Local\FinanceApp\`
2. Copie o arquivo `finance.db`
3. Para restaurar: cole o arquivo na mesma pasta em outro PC

## 🐛 **Solução de Problemas:**

**Se der erro de banco:**
1. Feche o aplicativo
2. Delete o arquivo `finance.db`
3. Abra o app novamente (recriará o banco limpo)

**Para resetar dados:**
- Mesmo processo acima - delete o arquivo .db

## 🎯 **Próximos Passos de Desenvolvimento:**

1. **Criar telas** para adicionar receitas/despesas
2. **Implementar relatórios** mensais/anuais
3. **Adicionar gráficos** de gastos por categoria
4. **Importar/Exportar** dados (CSV, Excel)

## 💡 **Dicas:**

- O banco é thread-safe (pode ser usado por múltiplas partes do app)
- SQLite suporta até TBs de dados (mais que suficiente para finanças pessoais)
- Os dados ficam criptografados se você usar BitLocker no Windows
- Não precisa de conexão com internet
- Funciona offline 100%

---

**Resumo**: Você não precisa instalar NADA no Windows! 
O app gerencia tudo automaticamente. 🚀
