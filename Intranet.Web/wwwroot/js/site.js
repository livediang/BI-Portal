// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* ==== Search Text ==== */

let searchInput = document.getElementById('searchInput');

if (searchInput) {
    let timeout = null;

    searchInput.addEventListener('keyup', function () {
        clearTimeout(timeout);

        let searchTerm = this.value;
        let searchUrl = this.getAttribute('data-url');

        timeout = setTimeout(function () {
            fetch(`${searchUrl}?searchTerm=${encodeURIComponent(searchTerm)}`, {
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            })
            .then(response => response.text())
            .then(data => {
                document.getElementById('userTableContainer').innerHTML = data;
            });
        }, 300);
    });
}