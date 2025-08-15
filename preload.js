const { contextBridge, ipcRenderer } = require('electron');

// Expose protected methods that allow the renderer process to use
// the ipcRenderer without exposing the entire object
contextBridge.exposeInMainWorld('electronAPI', {
    // Database operations
    getInvestments: () => ipcRenderer.invoke('get-investments'),
    addInvestment: (investmentData) => ipcRenderer.invoke('add-investment', investmentData),
    deleteInvestment: (id) => ipcRenderer.invoke('delete-investment', id),
    updateInvestment: (id, investmentData) => ipcRenderer.invoke('update-investment', id, investmentData),
    
    // System info (optional)
    platform: process.platform,
    
    // App version (optional)
    getVersion: () => ipcRenderer.invoke('get-version')
});

// Log that preload script has loaded
console.log('Preload script loaded successfully');
