# 🪟 Como usar o Finance Control no Windows

## 📥 **PASSO 1: Baixar .NET (se necessário)**

Se você não tem o .NET instalado no Windows:

1. Vá em: https://dotnet.microsoft.com/download
2. Baixe o **.NET 8.0 Runtime** (não precisa do SDK)
3. Instale normalmente (próximo, próximo, instalar)

## 🚀 **PASSO 2: Executar o aplicativo**

### **Opção A: Execução rápida (recomendada)**
1. Abra o **Terminal** ou **PowerShell** no Windows:
   - Pressione `Win + R`
   - Digite `cmd` ou `powershell`
   - Pressione Enter

2. Navegue até a pasta do projeto:
   ```cmd
   cd C:\caminho\para\finance-desktop-app\FinanceApp
   ```

3. Execute o comando:
   ```cmd
   dotnet run
   ```

4. **Pronto!** O aplicativo abrirá automaticamente! 🎉

### **Opção B: Criar executável (.exe)**
1. No Terminal/PowerShell, na pasta do projeto:
   ```cmd
   cd finance-desktop-app
   dotnet publish FinanceApp -c Release -o PublicApp
   ```

2. Vá na pasta `PublicApp` que foi criada
3. **Duplo clique** no arquivo `FinanceApp.exe`

## 🎯 **O que vai acontecer:**

### **Na primeira execução:**
1. ✅ Janela preta com **ícone dourado** aparecerá
2. ✅ Interface dark com botões amarelos
3. ✅ Banco SQLite criado automaticamente em:
   ```
   C:\Users\[SeuNome]\AppData\Local\FinanceApp\finance.db
   ```

### **Visual esperado:**
```
💰 FINANCE CONTROL
Controle Financeiro Pessoal

┌─────────────────────────────────────────┐
│                                         │
│    O que você gostaria de fazer hoje?  │
│                                         │
│   ┌─────────────┐   ┌─────────────┐    │
│   │📝 LANÇAMENTO│   │📊 DASHBOARD │    │
│   │             │   │             │    │
│   └─────────────┘   └─────────────┘    │
│                                         │
└─────────────────────────────────────────┘
```

## 🔧 **Se der algum problema:**

### **Erro: "dotnet não é reconhecido"**
**Solução**: Instale o .NET Runtime (Passo 1)

### **Erro: "Porta já em uso" ou similar**
**Solução**: 
```cmd
dotnet run --urls="http://localhost:5001"
```

### **App não abre (tela branca)**
**Solução**: Aguarde 10-15 segundos na primeira execução

## 📱 **Como usar o aplicativo:**

1. **Botão LANÇAMENTO** (amarelo): 
   - Clique para registrar receitas e despesas
   - Por enquanto mostra mensagem de "Em breve"

2. **Botão DASHBOARD** (contorno):
   - Clique para ver relatórios
   - Por enquanto mostra mensagem de "Em breve"

## 💾 **Seus dados ficam em:**
```
C:\Users\[SeuNome]\AppData\Local\FinanceApp\
├── finance.db          # Banco de dados SQLite
└── (logs futuros)
```

## 🔄 **Para usar novamente:**
- **Opção A**: Execute `dotnet run` na pasta FinanceApp
- **Opção B**: Duplo clique no FinanceApp.exe (se criou)

## 🎨 **Recursos funcionando:**
- ✅ Interface dark profissional
- ✅ Tema financeiro (preto + dourado)
- ✅ Banco SQLite funcionando
- ✅ Ícone personalizado na taskbar
- ✅ Botões interativos
- ✅ Layout responsivo

## ❓ **Dúvidas frequentes:**

**P: Preciso instalar SQLite?**
R: Não! Já está incluído.

**P: Precisa de internet?**
R: Não! Funciona 100% offline.

**P: Como fazer backup?**
R: Copie o arquivo `finance.db` da pasta AppData.

**P: Posso usar em outro PC?**
R: Sim! Copie a pasta do projeto ou o .exe gerado.

---

## 🚀 **RESUMO RÁPIDO:**
1. Instale .NET Runtime (se necessário)
2. Abra Terminal na pasta do projeto
3. Execute: `dotnet run`
4. **PRONTO!** App funcionando! 

**Seu Finance Control está pronto para uso! 💰✨**
