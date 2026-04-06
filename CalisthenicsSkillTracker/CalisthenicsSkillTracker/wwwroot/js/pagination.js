export function initKeysetPagination({
    searchBoxId,
    sortSelectId,
    containerId,
    endpoint,
    pageSize = 10,
    debounceDelay = 300
}) {
    const searchBox = document.getElementById(searchBoxId);
    const container = document.getElementById(containerId);
    const sortSelect = document.getElementById(sortSelectId);

    if (!container) {
        return;
    }

    let timeoutId;

    async function loadPage(filter = '', sortOrder = '', indexName = '', indexId = '', currentPageSize = pageSize, isPreviousPage = false) {
        const params = new URLSearchParams();

        if (filter) {
            params.append('filter', filter);
        }

        if (indexName) {
            params.append('indexName', indexName);
        }

        if (indexId) {
            params.append('indexId', indexId);
        }

        sortOrder = sortSelect ? sortSelect.value : '';
        if (sortOrder) {
            params.append('sortOrder', sortOrder);
        }

        params.append('pageSize', currentPageSize);
        params.append('isPreviousPage', isPreviousPage);

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
                loadPage(searchBox.value.trim(), '', '', '', pageSize, false);
            }, debounceDelay);
        });
    }

    if (sortSelect) {
        sortSelect.addEventListener('change', () => {
            const filter = searchBox ? searchBox.value.trim() : '';
            loadPage(filter, '', '', '', pageSize, false);
        });
    }

    document.addEventListener('click', (event) => {
        const button = event.target.closest('[data-role="next-page"], [data-role="previous-page"]');

        if (!button || !container.contains(button)) {
            return;
        }

        const filter = button.dataset.filter || '';
        const indexName = button.dataset.indexName || '';
        const indexId = button.dataset.indexId || '';
        const buttonPageSize = Number(button.dataset.pageSize) || pageSize;
        const isPreviousPage = button.dataset.isPreviousPage === 'true';

        loadPage(filter, '', indexName, indexId, buttonPageSize, isPreviousPage);
    });
}