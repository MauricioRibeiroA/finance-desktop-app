# Finance Desktop App 💰

Aplicação desktop para gestão financeira com foco em investimentos de renda fixa (Tesouro Direto).

## 🚀 Características

- ✅ Interface moderna e intuitiva com navegação por slides
- ✅ Banco de dados SQLite local
- ✅ Cálculos automáticos para operações do Tesouro Direto
- ✅ Persistência de dados offline
- ✅ Aplicação desktop nativa (Electron)
- ✅ Executável para Windows (.exe)

## 📋 Pré-requisitos

Para executar em modo de desenvolvimento:
- Node.js 18+ ([Download](https://nodejs.org/))
- npm (incluído com Node.js)

## 🔧 Instalação e Execução

### 1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/finance-desktop-app.git
cd finance-desktop-app
```

### 2. Instale as dependências
```bash
npm install
```

### 3. Execute a aplicação
```bash
npm start
```

## 🏗️ Gerar Executável para Windows

### Para gerar o executável .exe:
```bash
npm run build-win
```

O executável será criado na pasta `dist/` e pode ser distribuído independentemente.

## 📂 Estrutura do Projeto

```
finance-desktop-app/
├── main.js              # Processo principal do Electron
├── preload.js           # Script de preload (ponte entre main e renderer)
├── database.js          # Gerenciamento do banco SQLite
├── package.json         # Configurações e dependências
├── templates/
│   └── index.html      # Interface principal
├── static/
│   ├── css/
│   │   └── style.css   # Estilos da aplicação
│   └── js/
│       └── app.js      # Lógica da interface
└── dist/               # Executáveis gerados (após build)
```

## 💾 Banco de Dados

A aplicação usa SQLite para armazenar os dados localmente. O banco é criado automaticamente na primeira execução em:

- **Windows**: `%APPDATA%/finance-desktop-app/finance.db`
- **macOS**: `~/Library/Application Support/finance-desktop-app/finance.db`
- **Linux**: `~/.config/finance-desktop-app/finance.db`

### Tabelas:
- `tesouro_direto`: Armazena todas as operações de Tesouro Direto

## 🎯 Funcionalidades

### 1. Tela de Boas-vindas
- Entrada do nome do usuário
- Persistência do nome no localStorage

### 2. Menu Principal
- Lançar Operação
- Consultar Dashboard (em desenvolvimento)

### 3. Tipos de Investimento
- Renda Fixa
- Renda Variável (futuro)

### 4. Renda Fixa
- Títulos do Tesouro
- CDB (futuro)

### 5. Formulário Tesouro Direto
- ✅ Campos completos para operações
- ✅ Cálculos automáticos (PU Sujo, Valor Bruto, Valor Líquido)
- ✅ Validação de dados
- ✅ Persistência no banco SQLite

## 🔄 Cálculos Automáticos

O sistema calcula automaticamente:
- **PU Sujo** = PU Limpo + Juros Acumulados
- **Valor Bruto** = Quantidade × PU Sujo
- **Valor Líquido**:
  - Compra: Valor Bruto + Taxas + Outros Custos
  - Venda: Valor Bruto - Taxas - IR - Outros Custos

## 🎨 Interface

- Design moderno com tema escuro (preto e amarelo)
- Navegação suave entre telas
- Notificações visuais
- Layout responsivo

## 🚀 Próximas Funcionalidades

- Dashboard com gráficos
- Relatórios de investimentos
- Exportação de dados
- Suporte a CDB e outros produtos
- Calculadora de rentabilidade

## 🐛 Resolução de Problemas

### A aplicação não inicia:
1. Verifique se o Node.js está instalado: `node --version`
2. Instale as dependências: `npm install`
3. Tente executar: `npm start`

### Erro ao gerar executável:
1. Certifique-se de ter as dependências de desenvolvimento: `npm install`
2. Execute: `npm run build-win`

## 📝 Licença

MIT License - veja o arquivo LICENSE para detalhes.

## 👤 Autor

Finance App - Gestão Financeira Desktop

---

## 🔧 Scripts Disponíveis

- `npm start` - Executa a aplicação em modo de desenvolvimento
- `npm run dev` - Executa com DevTools aberto
- `npm run build` - Gera executável para plataforma atual
- `npm run build-win` - Gera executável para Windows (.exe)
