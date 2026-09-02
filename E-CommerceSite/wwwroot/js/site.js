document.addEventListener("DOMContentLoaded", function () {
    const body = document.body;
    const toggleButton = document.getElementById("sidebarToggle");
    const overlay = document.getElementById("sidebarOverlay");

    function isMobile() {
        return window.innerWidth <= 767;
    }

    function toggleSidebar() {
        if (isMobile()) {
            body.classList.toggle("sidebar-mobile-open");
        } else {
            body.classList.toggle("sidebar-collapsed");
        }
    }

    function closeMobileSidebar() {
        body.classList.remove("sidebar-mobile-open");
    }

    toggleButton.addEventListener("click", toggleSidebar);
    overlay.addEventListener("click", closeMobileSidebar);

    window.addEventListener("resize", function () {
        closeMobileSidebar();

        if (isMobile()) {
            body.classList.remove("sidebar-collapsed");
        }
    });

    document.addEventListener("keydown", function (event) {
        if (event.key === "Escape") {
            closeMobileSidebar();
        }
    });

});

const successAlert =
    document.getElementById("deleteAlert");

if (successAlert) {
    setTimeout(function () {
        successAlert.classList.remove("show");

        setTimeout(function () {
            successAlert.remove();
        }, 200);
    }, 5000);
}