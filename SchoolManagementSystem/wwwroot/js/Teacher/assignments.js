
    const menuToggle = document.getElementById("menuToggle");
    const sidebar = document.getElementById("sidebar");
    const mainContent = document.getElementById("mainContent");

    menuToggle.addEventListener("click", function () {
        sidebar.classList.toggle("active");
        mainContent.classList.toggle("active");
    });

    // يقفل السايد بار لما تدوس بره (اختياري بس مهم)
    document.addEventListener("click", function (e) {
        if (!sidebar.contains(e.target) && !menuToggle.contains(e.target)) {
            sidebar.classList.remove("active");
            mainContent.classList.remove("active");
        }
    });
