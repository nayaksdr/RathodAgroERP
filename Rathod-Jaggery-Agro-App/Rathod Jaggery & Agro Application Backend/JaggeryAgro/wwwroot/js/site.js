// site.js

document.addEventListener('DOMContentLoaded', function () {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('overlay');
    const mainContent = document.getElementById('main-content');
    const toggleBtn = document.getElementById('sidebarToggle');

    // Toggle function
    function toggleSidebar() {
        if (sidebar && overlay && mainContent) {
            sidebar.classList.toggle('active');
            overlay.classList.toggle('active');
            mainContent.classList.toggle('active');
        }
    }

    // Add event listeners only if button/overlay exist
    if (toggleBtn) toggleBtn.addEventListener('click', toggleSidebar);
    if (overlay) overlay.addEventListener('click', toggleSidebar);
});


$(document).ready(function () {
    $('.navbar-toggler').click(function () {
        $(this).toggleClass('active');
        $('.navbar-collapse').toggleClass('show');
    });

    // Optional: Close menu on link click (mobile)
    $('.navbar-collapse .nav-link').click(function () {
        if ($(window).width() < 992) {
            $('.navbar-collapse').removeClass('show');
            $('.navbar-toggler').removeClass('active');
        }
    });
});

