export function initKeysetPagination({
    searchBoxId,
    containerId,
    endpoint,
    pageSize = 10,
    debounceDelay = 300
}) {
    const searchBox = document.getElementById(searchBoxId);
    const container = document.getElementById(containerId);

    if (!container) {
        return;
    }

    let timeoutId;

    async function loadPage(filter = '', lastName = '', lastId = '', currentPageSize = pageSize) {
        const params = new URLSearchParams();

        if (filter) {
            params.append('filter', filter);
        }

        if (lastName) {
            params.append('lastName', lastName);
        }

        if (lastId) {
            params.append('lastId', lastId);
        }

        params.append('pageSize', currentPageSize);

        const response = await fetch(`${endpoint}?${params.toString()}`);

        if (!response.ok) {
            return;
        }

        const html = await response.text();
        container.innerHTML = html;
    }

    if (searchBox) {
        searchBox.addEventListener('input', () => {
            clearTimeout(timeoutId);

            timeoutId = setTimeout(() => {
                loadPage(searchBox.value.trim());
            }, debounceDelay);
        });
    }

    document.addEventListener('click', (event) => {
        const nextButton = event.target.closest('[data-role="next-page"]');

        if (!nextButton || !container.contains(nextButton)) {
            return;
        }

        const filter = nextButton.dataset.filter || '';
        const lastName = nextButton.dataset.lastName || '';
        const lastId = nextButton.dataset.lastId || '';
        const buttonPageSize = Number(nextButton.dataset.pageSize) || pageSize;

        loadPage(filter, lastName, lastId, buttonPageSize);
    });
}