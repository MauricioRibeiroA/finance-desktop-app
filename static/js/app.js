// Finance Web App - Main JavaScript
class FinanceApp {
    constructor() {
        this.charts = {};
        this.currentPage = 1;
        this.categories = [];
        
        this.init();
    }

    async init() {
        await this.loadCategories();
        this.setupEventListeners();
        this.setupTheme();
        this.setTodayDate();
        await this.loadDashboard();
        await this.loadTransactions();
    }

    // API Methods
    async apiRequest(endpoint, options = {}) {
        try {
            const response = await fetch(`/api${endpoint}`, {
                headers: {
                    'Content-Type': 'application/json',
                },
                ...options
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API request failed:', error);
            this.showNotification('Erro na comunicação com o servidor', 'error');
            throw error;
        }
    }

    async loadCategories() {
        try {
            this.categories = await this.apiRequest('/categories');
            this.populateCategorySelect();
        } catch (error) {
            console.error('Failed to load categories:', error);
        }
    }

    async loadDashboard() {
        try {
            const data = await this.apiRequest('/dashboard');
            this.updateDashboard(data);
        } catch (error) {
            console.error('Failed to load dashboard:', error);
        }
    }

    async loadTransactions(page = 1) {
        try {
            this.showLoading('transactionsList');
            const data = await this.apiRequest(`/transactions?page=${page}&per_page=10`);
            this.renderTransactions(data);
            this.renderPagination(data);
        } catch (error) {
            console.error('Failed to load transactions:', error);
            this.hideLoading('transactionsList');
        }
    }

    async addTransaction(transactionData) {
        try {
            const result = await this.apiRequest('/transactions', {
                method: 'POST',
                body: JSON.stringify(transactionData)
            });

            if (result.success) {
                this.showNotification('Transação adicionada com sucesso!', 'success');
                await this.loadDashboard();
                await this.loadTransactions(this.currentPage);
                document.getElementById('transactionForm').reset();
                this.setTodayDate();
            }
        } catch (error) {
            console.error('Failed to add transaction:', error);
        }
    }

    async deleteTransaction(id) {
        if (!confirm('Tem certeza que deseja excluir esta transação?')) return;

        try {
            await this.apiRequest(`/transactions/${id}`, { method: 'DELETE' });
            this.showNotification('Transação excluída com sucesso!', 'success');
            await this.loadDashboard();
            await this.loadTransactions(this.currentPage);
        } catch (error) {
            console.error('Failed to delete transaction:', error);
        }
    }

    // UI Methods
    setupEventListeners() {
        // Form submission
        document.getElementById('transactionForm').addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleFormSubmit(e);
        });

        // Theme toggle
        document.getElementById('themeToggle').addEventListener('click', () => {
            this.toggleTheme();
        });

        // Refresh button
        document.getElementById('refreshTransactions').addEventListener('click', () => {
            this.loadTransactions(this.currentPage);
        });

        // Auto-refresh dashboard every 30 seconds
        setInterval(() => {
            this.loadDashboard();
        }, 30000);
    }

    setupTheme() {
        const savedTheme = localStorage.getItem('financeApp-theme') || 'light';
        document.documentElement.setAttribute('data-theme', savedTheme);
        this.updateThemeIcon(savedTheme);
    }

    toggleTheme() {
        const currentTheme = document.documentElement.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        
        document.documentElement.setAttribute('data-theme', newTheme);
        localStorage.setItem('financeApp-theme', newTheme);
        this.updateThemeIcon(newTheme);
    }

    updateThemeIcon(theme) {
        const icon = document.querySelector('#themeToggle i');
        icon.className = theme === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
    }

    setTodayDate() {
        const today = new Date().toISOString().split('T')[0];
        document.getElementById('date').value = today;
    }

    populateCategorySelect() {
        const select = document.getElementById('category');
        select.innerHTML = '<option value=\"\">Selecione uma categoria</option>';
        
        this.categories.forEach(category => {
            const option = document.createElement('option');
            option.value = category.name;
            option.textContent = `${category.icon} ${category.name}`;
            select.appendChild(option);
        });
    }

    handleFormSubmit(e) {
        const formData = new FormData(e.target);
        const transactionData = {
            description: formData.get('description'),
            amount: parseFloat(formData.get('amount')),
            category: formData.get('category'),
            transaction_type: formData.get('transaction_type'),
            date: formData.get('date')
        };

        // Validate data
        if (!transactionData.description || !transactionData.amount || 
            !transactionData.category || !transactionData.date) {
            this.showNotification('Por favor, preencha todos os campos', 'error');
            return;
        }

        if (transactionData.amount <= 0) {
            this.showNotification('O valor deve ser maior que zero', 'error');
            return;
        }

        this.addTransaction(transactionData);
    }

    updateDashboard(data) {
        // Update cards
        document.getElementById('totalIncome').textContent = this.formatCurrency(data.total_income);
        document.getElementById('totalExpense').textContent = this.formatCurrency(data.total_expense);
        document.getElementById('balance').textContent = this.formatCurrency(data.balance);
        document.getElementById('totalTransactions').textContent = data.transaction_count;

        // Update balance card color based on positive/negative
        const balanceCard = document.querySelector('.balance-card');
        if (data.balance < 0) {
            balanceCard.style.setProperty('--gradient-balance', 'linear-gradient(135deg, #f56565 0%, #e53e3e 100%)');
        } else {
            balanceCard.style.setProperty('--gradient-balance', 'linear-gradient(135deg, #4299e1 0%, #3182ce 100%)');
        }

        // Update charts
        this.updateExpenseChart(data.expense_by_category);
        this.updateFlowChart(data);
    }

    renderTransactions(data) {
        const container = document.getElementById('transactionsList');
        this.currentPage = data.current_page;

        if (!data.transactions || data.transactions.length === 0) {
            container.innerHTML = `
                <div class=\"loading\">
                    <i class=\"fas fa-inbox\"></i>
                    <p>Nenhuma transação encontrada</p>
                </div>
            `;
            return;
        }

        container.innerHTML = data.transactions.map(transaction => {
            const category = this.categories.find(cat => cat.name === transaction.category);
            const categoryIcon = category ? category.icon : '💰';
            const date = new Date(transaction.date).toLocaleDateString('pt-BR');
            
            return `
                <div class=\"transaction-item\">
                    <div class=\"transaction-info\">
                        <div class=\"transaction-description\">${transaction.description}</div>
                        <div class=\"transaction-meta\">
                            <span class=\"transaction-category\">
                                <span>${categoryIcon}</span>
                                ${transaction.category}
                            </span>
                            <span><i class=\"fas fa-calendar\"></i> ${date}</span>
                        </div>
                    </div>
                    <div class=\"transaction-amount ${transaction.transaction_type}\">
                        ${transaction.transaction_type === 'income' ? '+' : '-'} ${this.formatCurrency(Math.abs(transaction.amount))}
                    </div>
                    <div class=\"transaction-actions\">
                        <button class=\"btn btn-danger btn-small\" onclick=\"app.deleteTransaction(${transaction.id})\">
                            <i class=\"fas fa-trash\"></i>
                        </button>
                    </div>
                </div>
            `;
        }).join('');
    }

    renderPagination(data) {
        const container = document.getElementById('pagination');
        
        if (data.pages <= 1) {
            container.innerHTML = '';
            return;
        }

        let paginationHTML = '';

        // Previous button
        paginationHTML += `
            <button ${data.current_page <= 1 ? 'disabled' : ''} 
                    onclick=\"app.loadTransactions(${data.current_page - 1})\">
                <i class=\"fas fa-chevron-left\"></i>
            </button>
        `;

        // Page numbers
        const startPage = Math.max(1, data.current_page - 2);
        const endPage = Math.min(data.pages, startPage + 4);

        for (let i = startPage; i <= endPage; i++) {
            paginationHTML += `
                <button class=\"${i === data.current_page ? 'active' : ''}\" 
                        onclick=\"app.loadTransactions(${i})\">
                    ${i}
                </button>
            `;
        }

        // Next button
        paginationHTML += `
            <button ${data.current_page >= data.pages ? 'disabled' : ''} 
                    onclick=\"app.loadTransactions(${data.current_page + 1})\">
                <i class=\"fas fa-chevron-right\"></i>
            </button>
        `;

        container.innerHTML = paginationHTML;
    }

    // Chart Methods
    updateExpenseChart(expenseData) {
        const ctx = document.getElementById('expenseChart').getContext('2d');

        if (this.charts.expenseChart) {
            this.charts.expenseChart.destroy();
        }

        if (!expenseData || Object.keys(expenseData).length === 0) {
            this.charts.expenseChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Nenhum dado'],
                    datasets: [{
                        data: [1],
                        backgroundColor: ['#e2e8f0'],
                        borderWidth: 0
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            display: false
                        }
                    }
                }
            });
            return;
        }

        const labels = Object.keys(expenseData);
        const values = Object.values(expenseData);
        const colors = [
            '#ff6b6b', '#4ecdc4', '#45b7d1', '#f39c12', '#9b59b6',
            '#1abc9c', '#27ae60', '#3498db', '#e74c3c', '#95a5a6'
        ];

        this.charts.expenseChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: values,
                    backgroundColor: colors.slice(0, labels.length),
                    borderWidth: 2,
                    borderColor: getComputedStyle(document.documentElement).getPropertyValue('--bg-primary')
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            padding: 15,
                            color: getComputedStyle(document.documentElement).getPropertyValue('--text-primary'),
                            font: {
                                size: 12
                            }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: (context) => {
                                const label = context.label || '';
                                const value = this.formatCurrency(context.parsed);
                                return `${label}: ${value}`;
                            }
                        }
                    }
                }
            }
        });
    }

    updateFlowChart(data) {
        const ctx = document.getElementById('flowChart').getContext('2d');

        if (this.charts.flowChart) {
            this.charts.flowChart.destroy();
        }

        // Generate last 7 days data (simplified for demo)
        const last7Days = [];
        const incomeData = [];
        const expenseData = [];
        
        for (let i = 6; i >= 0; i--) {
            const date = new Date();
            date.setDate(date.getDate() - i);
            last7Days.push(date.toLocaleDateString('pt-BR', { weekday: 'short' }));
            
            // Simulate daily data based on monthly totals
            incomeData.push(data.total_income / 30 + (Math.random() - 0.5) * 100);
            expenseData.push(data.total_expense / 30 + (Math.random() - 0.5) * 100);
        }

        this.charts.flowChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: last7Days,
                datasets: [
                    {
                        label: 'Receitas',
                        data: incomeData,
                        borderColor: '#48bb78',
                        backgroundColor: 'rgba(72, 187, 120, 0.1)',
                        fill: true,
                        tension: 0.4
                    },
                    {
                        label: 'Despesas',
                        data: expenseData,
                        borderColor: '#f56565',
                        backgroundColor: 'rgba(245, 101, 101, 0.1)',
                        fill: true,
                        tension: 0.4
                    }
                ]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            color: getComputedStyle(document.documentElement).getPropertyValue('--text-secondary'),
                            callback: (value) => this.formatCurrency(value)
                        },
                        grid: {
                            color: getComputedStyle(document.documentElement).getPropertyValue('--border-color')
                        }
                    },
                    x: {
                        ticks: {
                            color: getComputedStyle(document.documentElement).getPropertyValue('--text-secondary')
                        },
                        grid: {
                            color: getComputedStyle(document.documentElement).getPropertyValue('--border-color')
                        }
                    }
                },
                plugins: {
                    legend: {
                        labels: {
                            color: getComputedStyle(document.documentElement).getPropertyValue('--text-primary'),
                            font: {
                                size: 12
                            }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: (context) => {
                                return `${context.dataset.label}: ${this.formatCurrency(context.parsed.y)}`;
                            }
                        }
                    }
                }
            }
        });
    }

    // Utility Methods
    formatCurrency(value) {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(value || 0);
    }

    showLoading(containerId) {
        const container = document.getElementById(containerId);
        container.innerHTML = `
            <div class=\"loading\">
                <i class=\"fas fa-spinner fa-spin\"></i>
                Carregando...
            </div>
        `;
    }

    hideLoading(containerId) {
        const container = document.getElementById(containerId);
        if (container.querySelector('.loading')) {
            container.innerHTML = '';
        }
    }

    showNotification(message, type = 'info') {
        // Create notification element
        const notification = document.createElement('div');
        notification.className = `notification notification-${type}`;
        notification.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            padding: 16px 20px;
            border-radius: 8px;
            color: white;
            font-weight: 600;
            z-index: 10000;
            transform: translateX(100%);
            transition: transform 0.3s ease;
            max-width: 300px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        `;

        // Set background color based on type
        const colors = {
            success: '#48bb78',
            error: '#f56565',
            warning: '#ed8936',
            info: '#4299e1'
        };
        notification.style.backgroundColor = colors[type] || colors.info;
        notification.textContent = message;

        document.body.appendChild(notification);

        // Show notification
        setTimeout(() => {
            notification.style.transform = 'translateX(0)';
        }, 100);

        // Hide notification after 3 seconds
        setTimeout(() => {
            notification.style.transform = 'translateX(100%)';
            setTimeout(() => {
                document.body.removeChild(notification);
            }, 300);
        }, 3000);
    }

    // Keyboard shortcuts
    setupKeyboardShortcuts() {
        document.addEventListener('keydown', (e) => {
            // Ctrl/Cmd + Enter to submit form
            if ((e.ctrlKey || e.metaKey) && e.key === 'Enter') {
                const form = document.getElementById('transactionForm');
                if (document.activeElement && form.contains(document.activeElement)) {
                    e.preventDefault();
                    form.dispatchEvent(new Event('submit'));
                }
            }
            
            // F5 to refresh transactions
            if (e.key === 'F5' && !e.ctrlKey && !e.metaKey) {
                e.preventDefault();
                this.loadTransactions(this.currentPage);
            }
        });
    }
}

// Initialize app when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.app = new FinanceApp();
});

// Service Worker registration (for offline capabilities)
if ('serviceWorker' in navigator) {
    window.addEventListener('load', () => {
        navigator.serviceWorker.register('/sw.js')
            .then((registration) => {
                console.log('SW registered: ', registration);
            })
            .catch((registrationError) => {
                console.log('SW registration failed: ', registrationError);
            });
    });
}
