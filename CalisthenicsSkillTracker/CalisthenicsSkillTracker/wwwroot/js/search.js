export function initTableSearch(inputId, containerId, searchUrl) {
    const searchBox = document.getElementById(inputId);
    const container = document.getElementById(containerId);
    if (!searchBox || !container) return;

    let timeout = null;

    searchBox.addEventListener("input", function () {
        clearTimeout(timeout);

        timeout = setTimeout(() => {
            loadTable(this.value);
        }, 300);
    });

    function loadTable(filter = "") {
        fetch(`${searchUrl}?filter=${encodeURIComponent(filter)}`)
            .then(res => res.text())
            .then(html => {
                container.innerHTML = html;
            })
            .catch(err => console.error("Error loading table:", err));
    }
}