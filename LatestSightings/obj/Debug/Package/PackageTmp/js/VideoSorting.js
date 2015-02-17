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

jQuery(document).ready(function () {
    checkitem();
    $("#myCarousel").on("slid.bs.carousel", "", checkitem);
    LoadImages('');
    LoadFeatured();

    $('#searchButton').on('click', function () {
        LoadImages($('#searchBox').val());
    });
})

function LoadImages(query) {
    $.ajax({
        url: "/GalleryData.aspx/GetGallery",
        type: "POST",
        cache: false,
        data: JSON.stringify({ type: 'video', queryText: query }),
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
                var $itemDiv = $("<div>", { class: "item" });;
                for (var i = 0; i < data.d.length; i++) {
                    if (i % 6 == 0) {
                        if ($itemDiv.children().length > 0) {
                            $('#carousel-inner').append($itemDiv);
                        }
                        $itemDiv = $("<div>", { class: "item" });
                    }
                    var $div = $("<div>", { class: "col-sm-2 thumbnail" });
                    var img = $('<img class="latestImage">');
                    img.attr('src', data.d[i].Url);
                    img.attr('id', data.d[i].Id);
                    img.appendTo($div);
                    var $divTitle = $("<div>", { class: "title" });
                    $divTitle.html(data.d[i].Title);
                    $div.append($divTitle);
                    $itemDiv.append($div);
                }
                $('#carousel-inner').append($itemDiv);
                $('.carousel').carousel({ pause: true, interval: false }).carousel('next');
                checkitem();

                $(".latestImage").click(function () {
                    InsertFeatured($(this).attr('id'));
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

$(function () {
    var startIndex = 0;
    var stopIndex = 0;
    $("#sortable").sortable({
        revert: true,
        start: function (event, div) {
            startIndex = div.item.index();
        },
        stop: function (event, div) {
            stopIndex = div.item.index();
            var i = 1;
            var itemList = "";
            $('#sortable .col-sm-2').each(function (index, item) {
                $(item).children('div').children('div').html(index + 1 + ".")
                if (index > 0) { itemList += ","; }
                itemList += $(item).children('div').children('img').attr('id');
            });
            UpdateFeatured(itemList);
        }
    });
    $("ul, li").disableSelection();
});

function LoadFeatured() {
    $.ajax({
        url: "/GalleryData.aspx/GetFeatured",
        type: "POST",
        cache: false,
        data: JSON.stringify({ type: 'video' }),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $('#sortable').empty();
            $('.loader1').show()
        },
        complete: function () {
            $('.loader1').hide();
        },
        success: function (data) {
            if (data != null && data.d != null && data.d.length > 0) {
                var $divSpace = $("<div>", { class: "col-sm-1" });
                $('#sortable').append($divSpace);
                for (var i = 0; i < data.d.length; i++) {
                    var $divItem = $("<div>", { class: "col-sm-2" });
                    var $divCont = $("<div>");
                    var $divOrder = $("<div>", { class: "number" });
                    var img = $('<img>');
                    img.attr('class', "img-responsive");
                    img.attr('src', data.d[i].Url);
                    img.attr('id', data.d[i].Id);
                    img.appendTo($divCont);
                    $divCont.append($divOrder);
                    $divItem.append($divCont);
                    $('#sortable').append($divItem);
                    $('#sortable .col-sm-2').each(function (index, item) {
                        $(item).children('div').children('div').html(index + 1 + ".")
                    });
                }
                $divSpace = $("<div>", { class: "col-sm-1" });
                $('#sortable').append($divSpace);
            }
            else {
                alert('No featured items');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('No featured items');
        }
    });
}

function InsertFeatured(itemId) {
    $.ajax({
        url: "/GalleryData.aspx/InsertNewFeatured",
        type: "POST",
        cache: false,
        data: JSON.stringify({ type: 'video', id: itemId }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data != null && data.d != null && data.d == "True") {
                LoadFeatured();
            }
            else {
                alert('Error inserting featured item');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error inserting featured item');
        }
    });
}

function UpdateFeatured(items) {
    $.ajax({
        url: "/GalleryData.aspx/UpdateFeaturedOrder",
        type: "POST",
        cache: false,
        data: JSON.stringify({ type: 'video', itemList: items }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data != null && data.d != null && data.d == "True") {
                LoadFeatured();
            }
            else {
                alert('Error updating featured items order');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error updating featured items order');
        }
    });
}