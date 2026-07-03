document.addEventListener('DOMContentLoaded', () => {
    // Helper function for password toggle
    const setupPasswordToggle = (toggleId, inputId) => {
        const toggleBtn = document.getElementById(toggleId);
        const inputField = document.getElementById(inputId);

        if (toggleBtn && inputField) {
            toggleBtn.addEventListener('click', (e) => {
                e.preventDefault();
                const type = inputField.type === 'password' ? 'text' : 'password';
                inputField.type = type;
                toggleBtn.classList.toggle('fa-eye-slash');
                toggleBtn.classList.toggle('fa-eye');
            });
        }
    };

    // Setup toggles for Login page
    setupPasswordToggle('togglePassword', 'password');

    // Setup toggles for Reset Password page
    setupPasswordToggle('toggleNewPassword', 'newPassword');
    setupPasswordToggle('toggleConfirmPassword', 'confirmPassword');

    // Hide validation errors on page load
    const errorMessages = document.querySelectorAll('.text-danger');
    errorMessages.forEach(msg => {
        msg.style.display = 'none';
    });

    // Show validation errors only after form submission attempt
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const inputs = this.querySelectorAll('.form-input');
            inputs.forEach(input => {
                const formGroup = input.closest('.form-group');
                const errorSpan = formGroup?.querySelector('.text-danger');
                
                // Show error if input is invalid
                if (errorSpan && !input.value.trim()) {
                    errorSpan.style.display = 'block';
                    input.classList.add('input-validation-error');
                    formGroup.classList.add('has-error');
                } else if (errorSpan) {
                    errorSpan.style.display = 'none';
                    input.classList.remove('input-validation-error');
                    formGroup.classList.remove('has-error');
                }
            });
        });

        // Clear error when user starts typing
        const inputs = form.querySelectorAll('.form-input');
        inputs.forEach(input => {
            input.addEventListener('input', function() {
                const formGroup = this.closest('.form-group');
                const errorSpan = formGroup?.querySelector('.text-danger');
                
                if (this.value.trim()) {
                    if (errorSpan) {
                        errorSpan.style.display = 'none';
                    }
                    this.classList.remove('input-validation-error');
                    formGroup.classList.remove('has-error');
                }
            });

            input.addEventListener('change', function() {
                const formGroup = this.closest('.form-group');
                const errorSpan = formGroup?.querySelector('.text-danger');
                
                if (this.value.trim()) {
                    if (errorSpan) {
                        errorSpan.style.display = 'none';
                    }
                    this.classList.remove('input-validation-error');
                    formGroup.classList.remove('has-error');
                }
            });
        });
    });

    // ========== PASSWORD STRENGTH CHECKER ==========
    const newPasswordInput = document.getElementById('newPassword');
    const strengthContainer = document.getElementById('passwordStrength');
    const strengthText = document.getElementById('strengthText');
    const bars = [
        document.getElementById('bar1'),
        document.getElementById('bar2'),
        document.getElementById('bar3'),
        document.getElementById('bar4')
    ];

    if (newPasswordInput && strengthContainer) {
        newPasswordInput.addEventListener('input', function () {
            const password = this.value;

            if (password.length === 0) {
                strengthContainer.style.display = 'none';
                return;
            }

            strengthContainer.style.display = 'flex';

            // Calculate strength
            let score = 0;
            if (password.length >= 6) score++;
            if (password.length >= 10) score++;
            if (/[A-Z]/.test(password) && /[a-z]/.test(password)) score++;
            if (/[0-9]/.test(password)) score++;
            if (/[^A-Za-z0-9]/.test(password)) score++;

            // Normalize to 0-4
            let level = Math.min(Math.floor(score * 4 / 5), 4);
            if (password.length < 6) level = Math.min(level, 1);

            const levels = [
                { name: 'ضعيفة جداً', cls: 'weak' },
                { name: 'ضعيفة', cls: 'weak' },
                { name: 'متوسطة', cls: 'medium' },
                { name: 'قوية', cls: 'strong' },
                { name: 'قوية جداً', cls: 'very-strong' }
            ];

            const current = levels[level];

            // Reset bars
            bars.forEach((bar, i) => {
                bar.className = 'strength-bar';
                if (i < level) {
                    bar.classList.add(current.cls);
                }
            });

            // Update text
            strengthText.textContent = current.name;
            strengthText.className = 'strength-text ' + current.cls;
        });
    }

    // ========== PASSWORD MATCH CHECKER ==========
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const matchIndicator = document.getElementById('passwordMatch');
    const mismatchIndicator = document.getElementById('passwordMismatch');

    if (confirmPasswordInput && newPasswordInput && matchIndicator && mismatchIndicator) {
        const checkMatch = () => {
            const newPass = newPasswordInput.value;
            const confirmPass = confirmPasswordInput.value;

            if (confirmPass.length === 0) {
                matchIndicator.style.display = 'none';
                mismatchIndicator.style.display = 'none';
                return;
            }

            if (newPass === confirmPass) {
                matchIndicator.style.display = 'flex';
                mismatchIndicator.style.display = 'none';
            } else {
                matchIndicator.style.display = 'none';
                mismatchIndicator.style.display = 'flex';
            }
        };

        confirmPasswordInput.addEventListener('input', checkMatch);
        newPasswordInput.addEventListener('input', checkMatch);
    }

    // ========== FORM LOADING STATE ==========
    const forgotForm = document.getElementById('forgotPasswordForm');
    const resetForm = document.getElementById('resetPasswordForm');

    const setupLoadingState = (form, btnId) => {
        if (!form) return;

        form.addEventListener('submit', function () {
            const btn = document.getElementById(btnId);
            if (btn && form.checkValidity()) {
                btn.classList.add('loading');
                btn.disabled = true;
            }
        });
    };

    setupLoadingState(forgotForm, 'forgotSubmitBtn');
    setupLoadingState(resetForm, 'resetSubmitBtn');

    // ========== SHOW VALIDATION SUMMARY IF HAS ERRORS ==========
    const validationSummaries = document.querySelectorAll('[data-valmsg-summary="true"]');
    validationSummaries.forEach(summary => {
        const list = summary.querySelector('ul');
        if (list && list.children.length > 0) {
            // Check if any li has actual text
            const hasErrors = Array.from(list.children).some(li => li.textContent.trim().length > 0);
            if (hasErrors) {
                summary.style.display = 'flex';
            }
        }
    });

    // Show alert-error divs that have validation errors
    const alertErrors = document.querySelectorAll('.alert.alert-error');
    alertErrors.forEach(alert => {
        const list = alert.querySelector('ul');
        if (list && list.children.length > 0) {
            const hasErrors = Array.from(list.children).some(li => li.textContent.trim().length > 0);
            if (hasErrors) {
                alert.style.display = 'flex';
            }
        }
    });
});