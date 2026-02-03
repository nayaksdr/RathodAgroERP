// Animate Hamburger Toggler
document.addEventListener('DOMContentLoaded', function () {
    var toggler = document.querySelector('.navbar-toggler');
    toggler.addEventListener('click', function () {
        this.classList.toggle('collapsed');
    });
});

// Highlight Active Nav Item
document.addEventListener('DOMContentLoaded', function () {
    var currentUrl = window.location.href;
    var navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    navLinks.forEach(function (link) {
        if (currentUrl.includes(link.getAttribute('href'))) {
            link.classList.add('active');
        }
    });
});
