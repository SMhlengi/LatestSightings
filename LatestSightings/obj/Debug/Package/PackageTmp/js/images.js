checkitem = function () {
    var $this;
    $this = $("#myCarousel");
    if ($("#myCarousel .carousel-inner .item:first").hasClass("active")) {
        $this.children(".left").hide();
        $this.children(".right").show();

    } else if ($("#myCarousel .carousel-inner .item:last").hasClass("active")) {
        $this.children(".right").hide();
        $this.children(".left").show();
    } else {
        $this.children(".carousel-control").show();
    }
};

function LoadImages(date) {
    $.ajax({
        url: "/GalleryData.aspx/GetImageGallery",
        type: "POST",
        cache: false,
        data: JSON.stringify({ dateCombo: date, approved: $('#checkboxsuccess').prop('checked'), notApproved: $('#checkboxwarning').prop('checked') }),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $('#carousel-inner').empty();
            $('.loader').show()
        },
        complete: function () {
            $('.loader').hide();
        },
        success: function (data) {
            if (data != null && data.d != null && data.d.length > 0) {
                var $itemDiv = $("<div>", { class: "item" });
                for (var i = 0; i < data.d.length; i++) {
                    if (i % 6 == 0) {
                        if ($itemDiv.children().length > 0) {
                            $('#carousel-inner').append($itemDiv);
                        }
                        $itemDiv = $("<div>", { class: "item" });
                    }
                    var $div = $("<div>", { class: "col-sm-2 imgthumbnail" });
                    var href = $('<a>');
                    href.attr('href', data.d[i].Title);
                    href.attr('target', 'blank');
                    var img = $('<img class="latestImage">');
                    img.attr('src', data.d[i].Url);
                    img.attr('id', data.d[i].Id);
                    img.appendTo(href);
                    href.appendTo($div);
                    var $divSelect = $("<div>", { class: "toggle toggle-success" });
                    $divSelect.attr('id', data.d[i].Id);
                    $divSelect.html("<div class='toggle-slide active'><div class='toggle-inner'><div class='toggle-on'>ON</div><div class='toggle-blob'></div><div class='toggle-off'>OFF</div></div></div>");
                    $div.append($divSelect);
                    $itemDiv.append($div);

                    if (data.d[i].Description == "True") {
                        $($divSelect).toggles({ on: true });
                    }
                    else {
                        $($divSelect).toggles({ on: false });
                    }
                }

                $('#carousel-inner').append($itemDiv);
                $('.carousel').carousel({ pause: true, interval: false }).carousel('next');

                checkitem();

                $(".toggle").on('toggle', function (e, active) {
                    UpdateStatus($(this).attr('id'), active);
                });
            }
            else {
                alert('No items match your search criteria');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('No items match your search criteria');
        }
    });
}

function UpdateStatus(curId, curStatus) {
    $.ajax({
        url: "/GalleryData.aspx/UpdateImageStatus",
        type: "POST",
        cache: false,
        data: JSON.stringify({ id: curId, status: curStatus }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data != null && data.d != null && data.d == "True") {

            }
            else {
                alert('Error updating image status');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error updating image status');
        }
    });
}