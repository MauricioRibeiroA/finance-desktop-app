const Database = require('better-sqlite3');
const path = require('path');
const { app } = require('electron');

let db;

function initializeDatabase() {
    try {
        // Get user data directory for database storage
        const userDataPath = app.getPath('userData');
        const dbPath = path.join(userDataPath, 'finance.db');
        
        console.log('Database path:', dbPath);
        
        // Create database connection
        db = new Database(dbPath);
        
        // Enable foreign keys
        db.pragma('foreign_keys = ON');
        
        // Create tables
        createTables();
        
        console.log('Database initialized successfully');
    } catch (error) {
        console.error('Error initializing database:', error);
        throw error;
    }
}

function createTables() {
    // Create Tesouro Direto table
    const createTesouroDiretoTable = `
        CREATE TABLE IF NOT EXISTS tesouro_direto (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            data_operacao TEXT NOT NULL,
            tipo_movimento TEXT NOT NULL,
            classe_titulo TEXT,
            nome_titulo TEXT,
            data_vencimento TEXT,
            indexador TEXT,
            quantidade REAL DEFAULT 0.0,
            pu_limpo REAL DEFAULT 0.0,
            juros_acumulados REAL DEFAULT 0.0,
            pu_sujo REAL DEFAULT 0.0,
            valor_bruto REAL DEFAULT 0.0,
            taxas_emolumentos REAL DEFAULT 0.0,
            ir_retido REAL DEFAULT 0.0,
            outros_custos REAL DEFAULT 0.0,
            valor_liquido REAL DEFAULT 0.0,
            observacoes TEXT,
            created_at TEXT DEFAULT CURRENT_TIMESTAMP
        )
    `;
    
    db.exec(createTesouroDiretoTable);
    
    // Create indexes for better performance
    const createIndexes = `
        CREATE INDEX IF NOT EXISTS idx_tesouro_data_operacao ON tesouro_direto(data_operacao);
        CREATE INDEX IF NOT EXISTS idx_tesouro_tipo_movimento ON tesouro_direto(tipo_movimento);
        CREATE INDEX IF NOT EXISTS idx_tesouro_nome_titulo ON tesouro_direto(nome_titulo);
    `;
    
    db.exec(createIndexes);
}

// Investment CRUD operations
function getAllInvestments() {
    try {
        const stmt = db.prepare(`
            SELECT * FROM tesouro_direto 
            ORDER BY data_operacao DESC, created_at DESC
        `);
        return stmt.all();
    } catch (error) {
        console.error('Error getting investments:', error);
        throw error;
    }
}

function addInvestment(investmentData) {
    try {
        const stmt = db.prepare(`
            INSERT INTO tesouro_direto (
                data_operacao, tipo_movimento, classe_titulo, nome_titulo,
                data_vencimento, indexador, quantidade, pu_limpo, juros_acumulados,
                pu_sujo, valor_bruto, taxas_emolumentos, ir_retido, outros_custos,
                valor_liquido, observacoes
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        `);
        
        const result = stmt.run(
            investmentData.dataOperacao,
            investmentData.tipoMovimento,
            investmentData.classeTitulo,
            investmentData.nomeTitulo,
            investmentData.dataVencimento,
            investmentData.indexador,
            investmentData.quantidade || 0,
            investmentData.puLimpo || 0,
            investmentData.jurosAcumulados || 0,
            investmentData.puSujo || 0,
            investmentData.valorBruto || 0,
            investmentData.taxasEmolumentos || 0,
            investmentData.irRetido || 0,
            investmentData.outrosCustos || 0,
            investmentData.valorLiquido || 0,
            investmentData.observacoes
        );
        
        return { id: result.lastInsertRowid, success: true };
    } catch (error) {
        console.error('Error adding investment:', error);
        throw error;
    }
}

function updateInvestment(id, investmentData) {
    try {
        const stmt = db.prepare(`
            UPDATE tesouro_direto SET
                data_operacao = ?, tipo_movimento = ?, classe_titulo = ?, nome_titulo = ?,
                data_vencimento = ?, indexador = ?, quantidade = ?, pu_limpo = ?,
                juros_acumulados = ?, pu_sujo = ?, valor_bruto = ?, taxas_emolumentos = ?,
                ir_retido = ?, outros_custos = ?, valor_liquido = ?, observacoes = ?
            WHERE id = ?
        `);
        
        const result = stmt.run(
            investmentData.dataOperacao,
            investmentData.tipoMovimento,
            investmentData.classeTitulo,
            investmentData.nomeTitulo,
            investmentData.dataVencimento,
            investmentData.indexador,
            investmentData.quantidade || 0,
            investmentData.puLimpo || 0,
            investmentData.jurosAcumulados || 0,
            investmentData.puSujo || 0,
            investmentData.valorBruto || 0,
            investmentData.taxasEmolumentos || 0,
            investmentData.irRetido || 0,
            investmentData.outrosCustos || 0,
            investmentData.valorLiquido || 0,
            investmentData.observacoes,
            id
        );
        
        return { success: result.changes > 0 };
    } catch (error) {
        console.error('Error updating investment:', error);
        throw error;
    }
}

function deleteInvestment(id) {
    try {
        const stmt = db.prepare('DELETE FROM tesouro_direto WHERE id = ?');
        const result = stmt.run(id);
        return { success: result.changes > 0 };
    } catch (error) {
        console.error('Error deleting investment:', error);
        throw error;
    }
}

// Get database statistics (optional)
function getDatabaseStats() {
    try {
        const stmt = db.prepare(`
            SELECT 
                COUNT(*) as total_investments,
                SUM(CASE WHEN tipo_movimento = 'Compra' THEN valor_liquido ELSE 0 END) as total_invested,
                SUM(CASE WHEN tipo_movimento = 'Venda' THEN valor_liquido ELSE 0 END) as total_redeemed
            FROM tesouro_direto
        `);
        return stmt.get();
    } catch (error) {
        console.error('Error getting database stats:', error);
        throw error;
    }
}

module.exports = {
    initializeDatabase,
    getAllInvestments,
    addInvestment,
    updateInvestment,
    deleteInvestment,
    getDatabaseStats
};
