<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Financials.aspx.cs" Inherits="LatestSightings.Financials" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/style.datatables.css" rel="stylesheet">
    <link href="/css/dataTables.responsive.css" rel="stylesheet">

    <style>
        .form-control-nomargin {
            margin-bottom: 0px;
        }
        .timepicker-index {
            z-index: 10000;
            position: relative;
        }
    </style>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-primary-head">
                <div class="panel-heading">
                    <div class="panel-btns">
                        <a href="" class="panel-minimize tooltips" data-toggle="tooltip" title="Minimize Panel"><i class="fa fa-minus"></i></a>
                        <a href="" class="panel-close tooltips" data-toggle="tooltip" title="Close Panel"><i class="fa fa-times"></i></a>
                    </div><!-- panel-btns -->
                    <div class="row" style="padding-bottom: 5px;">
                        <div class="form-group col-md-12 form-control-nomargin">
                            Youtube Earnings report/payments received in month
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-2 form-control-nomargin">
                            <asp:TextBox runat="server" ID="txtMonthPicker" Name="txtMonthPicker" placeholder="Month" CssClass="form-control timepicker-index" />
                        </div>
                        <div class="form-group col-md-2 form-control-nomargin">
                            <asp:Button  ID="btnMonth" runat="server" OnClick="ChangeMonth" CssClass="btn btn-success" Text="Change Month" />
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="form-group col-md-2">
                            <div class="input-group mb5">
                                <label class="control-label">Estimated Earnings</label>
                            </div>
                            <div class="input-group mb5">
                                <span class="input-group-addon">$</span>
                                <asp:TextBox runat="server" ID="txtEstimated" name="txtEstimated" CssClass="form-control" Enabled="false" />
                            </div>
                        </div>
                        <div class="form-group col-md-2">
                            <div class="input-group mb5">
                                <label class="control-label">Override Earnings</label>
                            </div>
                            <div class="input-group mb5">
                                <span class="input-group-addon">$</span>
                                <asp:TextBox runat="server" ID="txtOverRide" name="txtOverRide" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-md-2">
                            <div class="input-group mb5">
                                <label class="control-label">Earnings Recieved</label>
                            </div>
                            <div class="input-group mb5">
                                <span class="input-group-addon">R</span>
                                <asp:TextBox runat="server" ID="txtRecieved" name="txtRecieved" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-md-1">
                            <div class="input-group mb5">
                                <label class="control-label">Rate</label>
                            </div>
                            <div class="input-group mb5">
                                <asp:TextBox runat="server" ID="txtExchange" name="txtExchange" CssClass="form-control" Enabled="false" />
                            </div>
                        </div>
                        <div class="form-group col-md-2">
                            <div class="input-group mb5 smaller-margin">
                                <label class="control-label">&nbsp;</label>
                            </div>
                            <div class="input-group mb5">
                                <asp:Button  ID="btnCalculate" runat="server" OnClick="Calculate" CssClass="btn btn-success" Text="Calculate Earnings" />
                            </div>
                        </div>
                    </div><!-- row -->
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-primary-head">
                <div id="basicTable_wrapper" class="dataTables_wrapper no-footer" style="width: 100%;">
                    <div class="dataTables_length" id="basicTable_length" style="width: 100%;">
                        <table id="videos" class="table table-striped responsive dataTable no-footer" role="grid" aria-describedby="basicTable_info" style="width: 100%;" width="100%" cellspacing="0">
                            <thead class="datatableHeaders">
                                <tr>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Title</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Add Payment</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">History</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Contributor</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">YouTube (R)</th>
                                    <asp:Literal ID="ltlHeader" runat="server" EnableViewState="false" />
                                </tr>
                            </thead>
                            <tbody>

                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" id="modal-thirdparty">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button aria-hidden="true" data-dismiss="modal" class="close" type="button" id="modalCloseThirdParty">&times;</button>
                    <h4 class="modal-title">Add Third Party Payment</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group col-md-12">
                            Input the third party payment to calculate the revenue share
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <select name="ddlThirdParty" id="ddlThirdParty" data-placeholder="Choose Third Party" style="width: 100%;" runat="server">
                            
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <select name="ddlCurrency" id="ddlCurrency" data-placeholder="Choose Currency" style="width: 100%;" runat="server">
                            
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <input type="text"  class="form-control" placeholder="Third Party Payment" name="thirdPartyPayment" id="thirdPartyPayment" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <input type="button" class="btn btn-success" onclick="AddThirdPartyPayment()");" value="Add" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade bs-example-modal-lg in" tabindex="-1" role="dialog" id="modal-video" style="z-index: 20000;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button aria-hidden="true" data-dismiss="modal" class="close" type="button" id="modalCloseVideos">&times;</button>
                    <h4 class="modal-title" id="videoTitle"></h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-primary-head">
                                <div id="Div1" class="dataTables_wrapper no-footer" style="width: 100%;">
                                    <div class="dataTables_length" id="Div2" style="width: 100%;">
                                        <table id="video" class="table table-hover responsive dataTable no-footer" role="grid" aria-describedby="basicTable_info" style="width: 100%;">
                                            <thead class="datatableHeaders">
                                                <tr>
                                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Month</th>
                                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Youtube</th>
                                                    <asp:Literal ID="ltlCurrencies" runat="server" EnableViewState="false" />
                                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Views</th>
                                                </tr>
                                            </thead>
                                            <tbody>

                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="/js/bootstrap-timepicker.min.js"></script>
    <script>
        $(document).ready(function () {
            jQuery('#<%= txtMonthPicker.ClientID %>').datepicker({ dateFormat: 'MM yy' });
        })
    </script>

    <script>
        var year = <%=year %>;
        var month = <%=month %>;
        var dTable;

        jQuery(document).ready(function () {
                dTable = $('#videos').DataTable({
                processing: true,
                serverSide: true,
                stateSave: false,
                pagingType: "full_numbers",
                ajax: {
                    "url": "/FinancialsData.aspx",
                    "data": function (d) {
                        d.year = "<%=year %>", d.month = "<%=month %>";
                    }
                },
                type: 'GET',
                pageLength: 10,
                language: { "sSearch": "" },
                order: [0, "asc"],
                "columns": [
                    { "data": "Title", orderable: true },
                    { orderable: false, "render": function (data, type, full, meta) { return "<a href=\"javascript:void(0);\" onclick=\"ShowPayment('" + full.Id + "');\")\">Add Payment</a>" } },
                    { orderable: false, "render": function (data, type, full, meta) { return "<a href=\"javascript:void(0);\" onclick=\"VideoDetails('" + full.Id + "', '" + full.YouTubeId + "', '" + full.Title + "');\")\">View</a>" } },
                    { "data": "Contributor", orderable: true },
                    { "data": "YouTubeEarnings", orderable: true },
                    <% =currencyScripts%>
                ]
            });

            jQuery('div.dataTables_filter input').addClass('form-control');
            jQuery('div.dataTables_length select').addClass('form-control');


            // DataTables Length to Select2
            jQuery('div.dataTables_length select').removeClass('form-control input-sm');
            jQuery('div.dataTables_length select').css({ width: '60px' });
            jQuery('div.dataTables_length select').select2({
                minimumResultsForSearch: -1
            });
        });

        var videoId;
        /* Add events */
        $("body").on("click", "#videos tbody tr", function (e) {

        });

        function ShowPayment(id)
        {
            videoId = id;
            $("#<% =ddlThirdParty.ClientID %>")[0].selectedIndex = 0;
            $("#<% =ddlCurrency.ClientID %>")[0].selectedIndex = 0;
            $('#thirdPartyPayment').val("");
            jQuery('#<%= ddlThirdParty.ClientID %>').select2({
                minimumResultsForSearch: -1
            });
            jQuery('#<%= ddlCurrency.ClientID %>').select2({
                minimumResultsForSearch: -1
            });
            $('#modal-thirdparty').modal('toggle');
        }

        $(document).ready(function () {
            jQuery('#<%= ddlThirdParty.ClientID %>').select2({
                minimumResultsForSearch: -1
            });
            jQuery('#<%= ddlCurrency.ClientID %>').select2({
                minimumResultsForSearch: -1
            });
        });

        function AddThirdPartyPayment() {
            $.ajax({
                url: "/Financials.aspx/AddThirdPartyPayment",
                type: "POST",
                cache: false,
                data: JSON.stringify({ id: videoId, currency: $('#<% =ddlCurrency.ClientID %>').val(), thirdParty: $('#<% =ddlThirdParty.ClientID %>').val(), value: $('#thirdPartyPayment').val(), paymentYear: year, paymentMonth: month }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#modalCloseThirdParty').click();
                    $.gritter.add(
                    {
                        title: "Payment added",
                        class_name: 'growl-success',
                        image: '/images/screen.png',
                        sticky: false,
                        time: 6000
                    });

                    dTable.ajax.reload(null, false);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $.gritter.add(
                    {
                        title: "Error adding payment",
                        class_name: 'growl-danger',
                        image: '/images/screen.png',
                        sticky: false,
                        time: 6000
                    });
                }
            });
        }
        
        var dTable2;
        var video;
        var youtube;
        function VideoDetails(videoId, youtubeId, videoTitle)
        {
            $("#videoTitle").html(videoTitle);
            video = videoId;
            youtube = youtubeId;
            $('#modal-video').modal('toggle');
            if (dTable2 == null)
            {
                dTable2 = $('#video').DataTable({
                    processing: true,
                    serverSide: true,
                    stateSave: false,
                    pagingType: "full_numbers",
                    paginate: false,
                    info: false,
                    filter: false,
                    sort: false,
                    "ajax": {
                        "url": "/VideoItemData.aspx",
                        "data": function ( d ) {
                            d.videoId = video, d.youtubeId = youtube;
                        }
                    },
                    type: 'GET',
                    responsive: false,
                    pageLength: 10,
                    language: { "sSearch": "" },
                    "columns": [
                        { "data": "ItemMonth", orderable: false },
                        { "data": "YouTubeEarnings", orderable: false },
                        <% =currencyScripts%>
                        { "data": "Views", orderable: false },
                    ]
                });
            }
            else
            {
                $("#video tbody").html('');
                dTable2.ajax.reload(null, false);
            }
        }
    </script>

    <script src="/js/jquery.dataTables.min.js"></script>
    <script src="/js/dataTables.bootstrap.js"></script>
    <script src="/js/dataTables.responsive.js"></script>
</asp:Content>