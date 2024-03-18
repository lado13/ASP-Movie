

document.addEventListener("DOMContentLoaded", function () {

    var registrationPasswordInput = document.getElementById("passwordInput");
    var registrationConfirmPasswordInput = document.getElementById("confirmPasswordInput");
    var registrationShowPasswordBtn = document.getElementById("showPasswordBtn");
    var registrationShowConfirmPasswordBtn = document.getElementById("showConfirmPasswordBtn");

    if (registrationPasswordInput && registrationShowPasswordBtn) {
        registrationShowPasswordBtn.addEventListener("click", function () {
            togglePasswordVisibility(registrationPasswordInput, registrationShowPasswordBtn);
        });
    }

    if (registrationConfirmPasswordInput && registrationShowConfirmPasswordBtn) {
        registrationShowConfirmPasswordBtn.addEventListener("click", function () {
            togglePasswordVisibility(registrationConfirmPasswordInput, registrationShowConfirmPasswordBtn);
        });
    }


    var loginPasswordInput = document.getElementById("loginPasswordInput");
    var loginShowPasswordBtn = document.getElementById("showLoginPasswordBtn");

    if (loginPasswordInput && loginShowPasswordBtn) {
        loginShowPasswordBtn.addEventListener("click", function () {
            togglePasswordVisibility(loginPasswordInput, loginShowPasswordBtn);
        });
    }
});

function togglePasswordVisibility(inputField, showPasswordBtn) {
    if (inputField.type === "password") {
        inputField.type = "text";
        showPasswordBtn.innerHTML = '<i class="fa-regular fa-eye-slash"></i>';
    } else {
        inputField.type = "password";
        showPasswordBtn.innerHTML = '<i class="fa-regular fa-eye"></i>';
    }
}
