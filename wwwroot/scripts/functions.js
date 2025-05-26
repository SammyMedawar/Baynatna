console.log('functions.js loaded successfully');

// Font size control for accessibility
let fontSize = 16;
window.increaseFontSize = function() {
    fontSize = Math.min(fontSize + 2, 32);
    document.documentElement.style.fontSize = fontSize + 'px';
};
window.decreaseFontSize = function() {
    fontSize = Math.max(fontSize - 2, 10);
    document.documentElement.style.fontSize = fontSize + 'px';
};

// Theme toggle functionality
function updateThemeIcon(theme) {
    const themeIcon = document.getElementById('theme-icon');
    console.log('Updating theme icon for theme:', theme);
    if (themeIcon) {
        themeIcon.textContent = theme === 'dark' ? '🌜' : '🌞';
        console.log('Theme icon updated to:', themeIcon.textContent);
    } else {
        console.log('Theme icon element not found');
    }
}

function setTheme(theme) {
    console.log('Setting theme to:', theme);
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
    updateThemeIcon(theme);
    console.log('Theme set. Current data-theme attribute:', document.documentElement.getAttribute('data-theme'));
}

function toggleTheme() {
    console.log('Toggle theme clicked');
    const currentTheme = document.documentElement.getAttribute('data-theme') || 'light';
    console.log('Current theme:', currentTheme);
    const newTheme = currentTheme === 'light' ? 'dark' : 'light';
    console.log('New theme will be:', newTheme);
    setTheme(newTheme);
}

// Initialize theme
function initTheme() {
    console.log('Initializing theme');
    const savedTheme = localStorage.getItem('theme');
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const theme = savedTheme || (prefersDark ? 'dark' : 'light');
    console.log('Saved theme:', savedTheme, 'Prefers dark:', prefersDark, 'Final theme:', theme);
    setTheme(theme);

    // Watch for system theme changes
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
        if (!localStorage.getItem('theme')) {
            setTheme(e.matches ? 'dark' : 'light');
        }
    });
}

// Wait for DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM loaded, setting up theme functionality');
    
    // Font size controls
    const incBtn = document.getElementById('increase-font');
    const decBtn = document.getElementById('decrease-font');
    if (incBtn) {
        incBtn.onclick = window.increaseFontSize;
        console.log('Font increase button found and connected');
    }
    if (decBtn) {
        decBtn.onclick = window.decreaseFontSize;
        console.log('Font decrease button found and connected');
    }

    // Theme toggle
    const themeBtn = document.getElementById('theme-toggle');
    if (themeBtn) {
        themeBtn.onclick = toggleTheme;
        console.log('Theme toggle button found and connected');
    } else {
        console.log('Theme toggle button NOT found');
    }

    // Initialize theme
    initTheme();
}); 