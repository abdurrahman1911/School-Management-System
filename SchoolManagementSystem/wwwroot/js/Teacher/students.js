


     const menuToggle = document.getElementById("menuToggle");
     const sidebar = document.getElementById("sidebar");

     menuToggle.addEventListener("click", () => {
     sidebar.classList.toggle("active");
     });

     document.addEventListener("click", function (e) {
     if (
     window.innerWidth <= 992 &&
     !sidebar.contains(e.target) &&
     !menuToggle.contains(e.target)
     ) {
     sidebar.classList.remove("active");
     }
     });
