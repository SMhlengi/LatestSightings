function AddNewThirdParty(controlName) {
    $.ajax({
        url: "/Video.aspx/AddThirdParty",
        type: "POST",
        cache: false,
        data: JSON.stringify({ name: $('#newThirdParty').val() }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.d.length > 0) {
                $('#' + controlName).empty();
                $('#' + controlName).append("<option value=\"\">Select Third Party...</option>");
                var selectedName;
                for (var i = 0; i < data.d.length; i++) {
                    if (data.d[i].Name == $('#newThirdParty').val()) { selectedName = data.d[i].Name; }
                    $('#' + controlName).append("<option value=\"" + data.d[i].Id + "\">" + data.d[i].Name + "</option>");
                }
            }
            $('#modalCloseThirdParty').click();
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}

function GetHtml() //Get the template and update the input field names
{
    thirdPaties++;
    var len = $('.thirdParty').length;
    var $html = $('.thirdPartyTemplate').clone();
    $html.find('#ddlSelectThirdParty').attr('name', 'selectThirdParty' + thirdPaties);
    $html.find('#ddlSelectThirdParty').attr('id', 'selectThirdParty' + thirdPaties);
    $html.find('#alias').attr('name', 'alias' + thirdPaties);
    $html.find('#alias').attr('id', 'alias' + thirdPaties);
    $html.find('#ttTPI').attr('id', 'ttTPI' + thirdPaties);
    return $html.html();
}

function ResetForm() {
    $('#Form1').trigger("reset");
    $('#container').empty();
}

function AddRevenueShare(controlName) {
    $.ajax({
        url: "/Video.aspx/AddRevenueShare",
        type: "POST",
        cache: false,
        data: JSON.stringify({ share: $('#newShare').val() }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.d.length > 0) {
                $('#' + controlName).empty();
                var selectedId;
                for (var i = 0; i < data.d.length; i++) {
                    if (data.d[i].ContributorShare == $('#newShare').val()) { selectedId = data.d[i].Text; }
                    $('#' + controlName).append("<option value=\"" + data.d[i].Text + "\">" + data.d[i].Text + "</option>");
                }
                jQuery('#' + controlName).select2({
                    minimumResultsForSearch: -1
                });
                $("#" + controlName).select2("val", selectedId);
            }
            $('#modalCloseRevenue').click();
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}

function addThirdParty() {
    $('<div/>', {
        'class': 'thirdParty', html: GetHtml()
    }).appendTo('#container'); //Get the html from template
    jQuery('#selectThirdParty' + thirdPaties).select2({
        minimumResultsForSearch: -1
    });

    $("#ttTPI" + thirdPaties).popover({
        trigger: 'hover',
        html: true,
        content: function () {
            return $('#popoverttTPI').html();
        }
    });
}

function Recalculate() {
    $.ajax({
        url: "/Video.aspx/Recalculate",
        type: "POST",
        cache: false,
        data: JSON.stringify({ id: videoId }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.d.length > 0) {
                $.gritter.add(
                {
                    title: "Video will calculate on next service run",
                    class_name: 'growl-success',
                    image: '/images/screen.png',
                    sticky: false,
                    time: 6000
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}

$(".confirm").confirm({
    text: "Are you sure you want to recalculate?",
    title: "Recalculate",
    confirm: function (button) {
        Recalculate();
    },
    cancel: function (button) {
        //alert('no');
    },
    confirmButton: "Yes I am",
    cancelButton: "No",
    post: true,
    confirmButtonClass: "btn-danger",
    cancelButtonClass: "btn-default"
});