$(document).on("click", ".favorite-btn", function (e) {
    e.preventDefault();

    const $btn = $(this);
    const url = $btn.data("url");
    const token = $("#antiForgeryForm input[name='__RequestVerificationToken']").val();

    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        headers: {
            "RequestVerificationToken": token
        },
        success: function (response) {
            if (!response || !response.success) {
                console.error("Favorite toggle failed", response);
                return;
            }

            const isFavorited = response.isFavorited;
            const $icon = $btn.find("i");

            if (isFavorited) {
                $btn.addClass("favorited");
                $icon.removeClass("bi-star").addClass("bi-star-fill");
                $btn.attr("data-is-favorited", "true");
            } else {
                $btn.removeClass("favorited");
                $icon.removeClass("bi-star-fill").addClass("bi-star");
                $btn.attr("data-is-favorited", "false");
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX error:", status, error);
            console.error("status code:", xhr.status);
            console.error("response text:", xhr.responseText);
        }
    });
});