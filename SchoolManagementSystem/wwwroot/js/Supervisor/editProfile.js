/**
 * Toggles password visibility between 'password' and 'text' types.
 * This is used for current, new, and confirmation password fields.
 * 
 * @param {string} inputId - The ID of the input element to toggle.
 * @param {HTMLElement} toggleSpan - The span element containing the eye icon.
 */
function togglePassword(inputId, toggleSpan) {
    const input = document.getElementById(inputId);
    const icon = toggleSpan.querySelector("i");

    if (!input || !icon) return;

    if (input.type === "password") {
        input.type = "text";
        // Update icon to 'eye' when password is visible
        icon.classList.replace("fa-eye-slash", "fa-eye");
    } else {
        input.type = "password";
        // Update icon back to 'eye-slash' when password is hidden
        icon.classList.replace("fa-eye", "fa-eye-slash");
    }
}

/**
 * Global event listener for form submission.
 * Can be used for client-side validation before sending data to the server.
 */
document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector('form');
    if (form) {
        form.onsubmit = () => {
            const newPass = document.getElementById('newPassword').value;
            const confirmPass = document.getElementById('confirmPassword').value;

            if (newPass !== confirmPass) {
                alert("New password and confirmation do not match!");
                return false; // Prevent form submission
            }
            return true;
        };
    }
});