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
document.addEventListener('DOMContentLoaded', function() {
    const incBtn = document.getElementById('increase-font');
    const decBtn = document.getElementById('decrease-font');
    if (incBtn) incBtn.onclick = window.increaseFontSize;
    if (decBtn) decBtn.onclick = window.decreaseFontSize;
}); 