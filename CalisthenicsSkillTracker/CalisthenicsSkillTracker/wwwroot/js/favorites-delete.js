$(document).on("click", ".remove-favorite-btn", function (e) {
    e.preventDefault();

    const $btn = $(this);
    const url = $btn.data("url");
    const token = $('#antiForgeryForm input[name="__RequestVerificationToken"]').val();
    const $card = $btn.closest(".favorite-card");
    const $section = $btn.closest(".favorites-section");
    const $grid = $section.find(".favorites-grid");
    const $count = $section.find(".section-count");

    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        headers: {
            RequestVerificationToken: token
        },
        success: function (response) {
            if (!response || !response.success) {
                console.error("Remove favorite failed", response);
                return;
            }

            if (response.isFavorited === false) {
                $card.remove();

                let currentCount = parseInt($count.text());
                if (!isNaN(currentCount) && currentCount > 0) {
                    $count.text(currentCount - 1);
                }

                if ($grid.find(".favorite-card").length === 0) {
                    const emptyMessage = $section.data("empty-message");

                    $grid.replaceWith(`
                        <div class="empty-state">
                            <p class="empty-state-text">${emptyMessage}</p>
                        </div>
                    `);
                }
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX error:", status, error);
            console.error("status code:", xhr.status);
            console.error("response text:", xhr.responseText);
        }
    });
});