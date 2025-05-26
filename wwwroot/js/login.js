document.addEventListener('DOMContentLoaded', function () {
    const passwordInput = document.getElementById('passwordInput');
    const toggleButton = document.getElementById('togglePassword');
    const toggleIcon = document.getElementById('togglePasswordIcon');

    if (passwordInput && toggleButton && toggleIcon) {
        toggleButton.addEventListener('click', function () {
            const isPasswordVisible = passwordInput.type === 'text';
            passwordInput.type = isPasswordVisible ? 'password' : 'text';

            // Correct icon toggle logic
            toggleIcon.classList.remove(isPasswordVisible ? 'bi-eye' : 'bi-eye-slash');
            toggleIcon.classList.add(isPasswordVisible ? 'bi-eye-slash' : 'bi-eye');
        });
    }
});
