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

// Theme system
const THEMES = {
    'light': 'Light Mode',
    'dark': 'Dark Mode',
    'protanopia-light': 'Protanopia (Light)',
    'protanopia-dark': 'Protanopia (Dark)',
    'deuteranopia-light': 'Deuteranopia (Light)',
    'deuteranopia-dark': 'Deuteranopia (Dark)',
    'tritanopia-light': 'Tritanopia (Light)',
    'tritanopia-dark': 'Tritanopia (Dark)',
    'achromatopsia-light': 'Achromatopsia (Light)',
    'achromatopsia-dark': 'Achromatopsia (Dark)'
};

const COLORBLIND_THEMES = {
    'protanopia': ['protanopia-light', 'protanopia-dark'],
    'deuteranopia': ['deuteranopia-light', 'deuteranopia-dark'],
    'tritanopia': ['tritanopia-light', 'tritanopia-dark'],
    'achromatopsia': ['achromatopsia-light', 'achromatopsia-dark']
};

function setTheme(theme) {
    console.log('Setting theme to:', theme);
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
    
    // Update theme selector
    const selector = document.getElementById('theme-selector');
    if (selector) {
        selector.value = theme;
    }
    
    console.log('Theme set. Current data-theme attribute:', document.documentElement.getAttribute('data-theme'));
}

function showThemePopup() {
    const overlay = document.getElementById('theme-popup-overlay');
    const content = document.getElementById('theme-popup-content');
    
    if (!overlay || !content) return;
    
    // Step 1: Ask about color blindness
    content.innerHTML = `
        <h3>Welcome to Baynatna!</h3>
        <p>To provide you with the best experience, please help us choose the right theme for you.</p>
        <p><strong>Do you have any form of color blindness?</strong></p>
        <div class="theme-options">
            <div class="theme-option" data-choice="colorblind">Yes, I have color blindness</div>
            <div class="theme-option" data-choice="normal">No, I have normal color vision</div>
        </div>
        <div class="theme-popup-buttons">
            <button class="theme-popup-btn secondary" onclick="skipThemeSelection()">Skip for now</button>
        </div>
    `;
    
    overlay.style.display = 'flex';
    
    // Add click handlers for options
    content.querySelectorAll('.theme-option').forEach(option => {
        option.addEventListener('click', function() {
            // Remove selected class from all options
            content.querySelectorAll('.theme-option').forEach(opt => opt.classList.remove('selected'));
            // Add selected class to clicked option
            this.classList.add('selected');
            
            setTimeout(() => {
                if (this.dataset.choice === 'colorblind') {
                    showColorBlindnessOptions();
                } else {
                    showNormalVisionOptions();
                }
            }, 300);
        });
    });
}

function showColorBlindnessOptions() {
    const content = document.getElementById('theme-popup-content');
    
    content.innerHTML = `
        <h3>Color Blindness Type</h3>
        <p>What type of color blindness do you have?</p>
        <div class="theme-options">
            <div class="theme-option" data-type="protanopia">
                <strong>Protanopia</strong><br>
                <small>Difficulty seeing red colors</small>
            </div>
            <div class="theme-option" data-type="deuteranopia">
                <strong>Deuteranopia</strong><br>
                <small>Difficulty seeing green colors</small>
            </div>
            <div class="theme-option" data-type="tritanopia">
                <strong>Tritanopia</strong><br>
                <small>Difficulty seeing blue colors</small>
            </div>
            <div class="theme-option" data-type="achromatopsia">
                <strong>Achromatopsia</strong><br>
                <small>Complete color blindness (grayscale)</small>
            </div>
        </div>
        <div class="theme-popup-buttons">
            <button class="theme-popup-btn secondary" onclick="showThemePopup()">Back</button>
            <button class="theme-popup-btn secondary" onclick="skipThemeSelection()">Skip</button>
        </div>
    `;
    
    content.querySelectorAll('.theme-option').forEach(option => {
        option.addEventListener('click', function() {
            content.querySelectorAll('.theme-option').forEach(opt => opt.classList.remove('selected'));
            this.classList.add('selected');
            
            setTimeout(() => {
                showColorBlindModeOptions(this.dataset.type);
            }, 300);
        });
    });
}

function showColorBlindModeOptions(colorBlindType) {
    const content = document.getElementById('theme-popup-content');
    const themes = COLORBLIND_THEMES[colorBlindType];
    
    content.innerHTML = `
        <h3>Choose Your Preferred Mode</h3>
        <p>Would you prefer light or dark mode?</p>
        <div class="theme-options">
            <div class="theme-option" data-theme="${themes[0]}">
                <strong>Light Mode</strong><br>
                <small>Bright background, dark text</small>
            </div>
            <div class="theme-option" data-theme="${themes[1]}">
                <strong>Dark Mode</strong><br>
                <small>Dark background, light text</small>
            </div>
        </div>
        <div class="theme-popup-buttons">
            <button class="theme-popup-btn secondary" onclick="showColorBlindnessOptions()">Back</button>
            <button class="theme-popup-btn secondary" onclick="skipThemeSelection()">Skip</button>
        </div>
    `;
    
    content.querySelectorAll('.theme-option').forEach(option => {
        option.addEventListener('click', function() {
            content.querySelectorAll('.theme-option').forEach(opt => opt.classList.remove('selected'));
            this.classList.add('selected');
            
            setTimeout(() => {
                applySelectedTheme(this.dataset.theme);
            }, 300);
        });
    });
}

function showNormalVisionOptions() {
    const content = document.getElementById('theme-popup-content');
    
    content.innerHTML = `
        <h3>Choose Your Preferred Mode</h3>
        <p>Would you prefer light or dark mode?</p>
        <div class="theme-options">
            <div class="theme-option" data-theme="light">
                <strong>Light Mode</strong><br>
                <small>Bright background, dark text</small>
            </div>
            <div class="theme-option" data-theme="dark">
                <strong>Dark Mode</strong><br>
                <small>Dark background, light text</small>
            </div>
        </div>
        <div class="theme-popup-buttons">
            <button class="theme-popup-btn secondary" onclick="showThemePopup()">Back</button>
            <button class="theme-popup-btn secondary" onclick="skipThemeSelection()">Skip</button>
        </div>
    `;
    
    content.querySelectorAll('.theme-option').forEach(option => {
        option.addEventListener('click', function() {
            content.querySelectorAll('.theme-option').forEach(opt => opt.classList.remove('selected'));
            this.classList.add('selected');
            
            setTimeout(() => {
                applySelectedTheme(this.dataset.theme);
            }, 300);
        });
    });
}

function applySelectedTheme(theme) {
    const content = document.getElementById('theme-popup-content');
    
    content.innerHTML = `
        <h3>Theme Applied!</h3>
        <p>Your theme has been set to <strong>${THEMES[theme]}</strong>.</p>
        <p>You can change this anytime using the theme selector in the header.</p>
        <div class="theme-popup-buttons">
            <button class="theme-popup-btn primary" onclick="closeThemePopup()">Continue</button>
        </div>
    `;
    
    setTheme(theme);
    localStorage.setItem('theme-selected', 'true');
}

function skipThemeSelection() {
    setTheme('light'); // Default to light mode
    localStorage.setItem('theme-selected', 'true');
    closeThemePopup();
}

function closeThemePopup() {
    const overlay = document.getElementById('theme-popup-overlay');
    if (overlay) {
        overlay.style.display = 'none';
    }
}

// Make functions globally available
window.skipThemeSelection = skipThemeSelection;
window.closeThemePopup = closeThemePopup;
window.showThemePopup = showThemePopup;

// Initialize theme system
function initTheme() {
    console.log('Initializing theme system');
    
    const savedTheme = localStorage.getItem('theme');
    const themeSelected = localStorage.getItem('theme-selected');
    const prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    
    // If no theme has been selected before, show the popup
    if (!themeSelected) {
        console.log('No theme selected before, showing popup');
        setTimeout(() => showThemePopup(), 1000); // Show popup after 1 second
        setTheme('light'); // Default while popup is shown
    } else {
        // Use saved theme or default
        const theme = savedTheme || (prefersDark ? 'dark' : 'light');
        console.log('Using saved/default theme:', theme);
        setTheme(theme);
    }
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

    // Theme selector dropdown
    const themeSelector = document.getElementById('theme-selector');
    if (themeSelector) {
        themeSelector.addEventListener('change', function() {
            console.log('Theme selector changed to:', this.value);
            setTheme(this.value);
            localStorage.setItem('theme-selected', 'true');
        });
        console.log('Theme selector found and connected');
    } else {
        console.log('Theme selector NOT found');
    }

    // Initialize theme
    initTheme();
}); 