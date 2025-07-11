document.addEventListener("DOMContentLoaded", function () {
    const currentPath = window.location.pathname.toLowerCase();
    const menuItems = document.querySelectorAll(".menu-item");

    menuItems.forEach(item => {
        const link = item.querySelector("a");
        const href = link.getAttribute("href").toLowerCase();

        if (currentPath.includes(href.replace("..", ""))) {
            item.classList.add("active");
        }
    });
});