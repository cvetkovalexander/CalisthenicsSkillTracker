console.log("favorites.js loaded");

$(document).on("click", ".favorite-btn", function (e) {
    e.preventDefault();

    const $btn = $(this);
    const url = $btn.data("url");

    $.ajax({
        url: url,
        type: "POST",
        success: function (response) {
            if (!response || !response.success) {
                console.error("Favorite toggle failed");
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
        error: function () {
            console.error("Error toggling favorite");
        }
    });
});