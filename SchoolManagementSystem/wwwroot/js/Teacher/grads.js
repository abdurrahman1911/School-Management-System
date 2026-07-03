document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.querySelector(".sidebar");
    const menuBtn = document.getElementById("menuToggle");

    if (!sidebar || !menuBtn) return;

    // فتح / غلق
    menuBtn.addEventListener("click", function (e) {
        e.stopPropagation();
        sidebar.classList.toggle("active");
        document.body.classList.toggle("sidebar-open");
    });

    // قفل عند الضغط خارجها
    document.addEventListener("click", function (e) {
        if (!sidebar.contains(e.target) && !menuBtn.contains(e.target)) {
            sidebar.classList.remove("active");
            document.body.classList.remove("sidebar-open");
        }
    });

    // قفل عند الضغط على لينك
    document.querySelectorAll(".sidebar a").forEach(a => {
        a.addEventListener("click", () => {
            sidebar.classList.remove("active");
            document.body.classList.remove("sidebar-open");
        });
    });
});