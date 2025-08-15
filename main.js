const { app, BrowserWindow, ipcMain } = require('electron');
const path = require('path');
const { initializeDatabase, getAllInvestments, addInvestment, deleteInvestment, updateInvestment } = require('./database');

let mainWindow;

function createWindow() {
    mainWindow = new BrowserWindow({
        width: 1200,
        height: 800,
        minWidth: 1000,
        minHeight: 600,
        webPreferences: {
            nodeIntegration: false,
            contextIsolation: true,
            preload: path.join(__dirname, 'preload.js')
        },
        icon: path.join(__dirname, 'static', 'icon.ico'),
        show: false,
        titleBarStyle: 'default'
    });

    // Load the HTML file
    mainWindow.loadFile(path.join(__dirname, 'templates', 'index.html'));

    // Show window when ready
    mainWindow.once('ready-to-show', () => {
        mainWindow.show();
    });

    // Open DevTools in development
    if (process.argv.includes('--dev')) {
        mainWindow.webContents.openDevTools();
    }
}

// App event handlers
app.whenReady().then(() => {
    // Initialize database
    initializeDatabase();
    
    createWindow();

    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) {
            createWindow();
        }
    });
});

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit();
    }
});

// IPC handlers for database operations
ipcMain.handle('get-investments', async () => {
    try {
        return await getAllInvestments();
    } catch (error) {
        console.error('Error getting investments:', error);
        throw error;
    }
});

ipcMain.handle('add-investment', async (event, investmentData) => {
    try {
        return await addInvestment(investmentData);
    } catch (error) {
        console.error('Error adding investment:', error);
        throw error;
    }
});

ipcMain.handle('delete-investment', async (event, id) => {
    try {
        return await deleteInvestment(id);
    } catch (error) {
        console.error('Error deleting investment:', error);
        throw error;
    }
});

ipcMain.handle('update-investment', async (event, id, investmentData) => {
    try {
        return await updateInvestment(id, investmentData);
    } catch (error) {
        console.error('Error updating investment:', error);
        throw error;
    }
});

// Handle app closing
app.on('before-quit', () => {
    // Any cleanup operations can be done here
});
