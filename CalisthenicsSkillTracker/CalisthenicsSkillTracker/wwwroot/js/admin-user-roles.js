document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".assign-role-form, .remove-role-form").forEach(form => {
        form.addEventListener("submit", function () {
            const row = form.closest("tr");
            const selectedRole = row.querySelector(".role-select").value;
            form.querySelector(".selected-role-input").value = selectedRole;
        });
    });
});