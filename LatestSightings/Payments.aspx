<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="LatestSightings.Payments" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/style.datatables.css" rel="stylesheet">
    <link href="/css/dataTables.responsive.css" rel="stylesheet">

    <style>
        .form-control-nomargin {
            margin-bottom: 0px;
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
                    <div class="row">
                        <div class="form-group col-md-2 form-control-nomargin">
                            <asp:TextBox runat="server" ID="txtMonthPicker" Name="txtMonthPicker" placeholder="Month" CssClass="form-control" />
                        </div>
                        <div class="form-group col-md-2 form-control-nomargin">
                            <asp:Button  ID="btnMonth" runat="server" OnClick="ChangeMonth" CssClass="btn btn-success" Text="Change Month" />
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" name="btnPublished" value="Paid" class="btn btn-success" onclick="location.href = '/payments/<% =year %>/<% =month %>?filter=paid';" />
                                <input type="button" name="btnPending" value="Not Paid" class="btn btn-warning" onclick="location.href = '/payments/<% =year %>/<% =month %>?filter=notpaid';" />
                                <asp:HyperLink ID="lnkCSV" runat="server">
                                    <img src="/images/csv-150x150.jpg" height="37" align="right" />
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="panel panel-primary-head">
                                    <div id="basicTable_wrapper" class="dataTables_wrapper no-footer" style="width: 100%;">
                                        <div class="dataTables_length" id="Div2" style="width: 100%;">
                                            <table id="videos" class="table table-striped responsive dataTable no-footer" role="grid" aria-describedby="basicTable_info" style="width: 100%;" width="100%" cellspacing="0">
                                                <thead class="datatableHeaders">
                                                    <tr>
                                                        <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Contributor</th>
                                                        <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Paid</th>
                                                        <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Videos</th>
                                                        <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">History</th>
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
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade bs-example-modal-lg in" tabindex="-1" role="dialog" id="modal-videos">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button aria-hidden="true" data-dismiss="modal" class="close" type="button" id="modalCloseVideos">&times;</button>
                        <h4 class="modal-title">Contributor videos</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="panel panel-primary-head">
                                    <div id="Div1" class="dataTables_wrapper no-footer" style="width: 100%;">
                                        <div class="dataTables_length" id="basicTable_length" style="width: 100%;">
                                            <table id="contributors" class="table table-striped responsive dataTable no-footer" role="grid" aria-describedby="basicTable_info" style="width: 100%;" width="100%" cellspacing="0">
                                                <thead class="datatableHeaders">
                                                    <tr>
                                                        <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Title</th>
                                                        <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">YouTube</th>
                                                        <asp:Literal ID="ltlCurrencies" runat="server" EnableViewState="false" />
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

            <div class="modal fade bs-example-modal-lg in" tabindex="-1" role="dialog" id="modal-videos1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button aria-hidden="true" data-dismiss="modal" class="close" type="button" id="Button1">&times;</button>
                        <h4 class="modal-title">Contributor videos</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="panel panel-primary-head">
                                    <div id="Div3" class="dataTables_wrapper no-footer" style="width: 100%;">
                                        <div class="dataTables_length" id="Div4" style="width: 100%;">
                                            <table id="contributors1" class="table table-striped responsive dataTable no-footer" role="grid" aria-describedby="basicTable_info" style="width: 100%;" width="100%" cellspacing="0">
                                                <thead class="datatableHeaders">
                                                    <tr>
                                                        <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Month</th>
                                                        <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">YouTube</th>
                                                        <asp:Literal ID="ltlCurrencies1" runat="server" EnableViewState="false" />
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
                    "url": "/PaymentsData.aspx",
                    "data": function (d) {
                        d.year = "<%=year %>", d.month = "<%=month %>", d.filter = "<%=filter %>";
                    }
                },
                type: 'GET',
                pageLength: 10,
                language: { "sSearch": "" },
                order: [0, "asc"],
                "columns": [
                    { "data": "ContributorName", orderable: true },
                    { orderable: false, "render": function (data, type, full, meta) { return "<input type=\"checkbox\" id=\"someCheckbox\" name=\"someCheckbox\" onclick=\"updatePaid(this, '" + full.ContributorId + "')\" " + full.Checked + " />" } },
                    { orderable: false, "render": function (data, type, full, meta) { return "<a href=\"javascript:void(0);\" onclick=\"ContributorVideos('" + full.ContributorId + "')\">View</a>" } },
                    { orderable: false, "render": function (data, type, full, meta) { return "<a href=\"javascript:void(0);\" onclick=\"ContributorVideos1('" + full.ContributorId + "')\">View</a>" } },
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

        var dTable2;
        var contributor;
        function ContributorVideos(contributorId)
        {
            contributor = contributorId;
            $('#modal-videos').modal('toggle');

            if (dTable2 == null)
            {
                dTable2 = $('#contributors').DataTable({
                    processing: true,
                    serverSide: true,
                    stateSave: false,
                    pagingType: "full_numbers",
                    paginate: false,
                    info: false,
                    filter: false,
                    sort: false,
                    ajax: {
                        "url": "/FinancialsData.aspx",
                        "data": function (d) {
                            d.year = "<%=year %>", d.month = "<%=month %>", d.owner = contributor;
                        }
                    },
                    type: 'GET',
                    responsive: false,
                    pageLength: 10,
                    language: { "sSearch": "" },
                    order: [0, "asc"],
                    "columns": [
                        { "data": "Title", orderable: false },
                        { "data": "YouTubeEarnings", orderable: false },
                    <% =currencyScripts1%>
                    ]
                });
            }
            else
            {
                $("#contributors tbody").html('');
                dTable2.ajax.reload(null, false);
            }
        }

        var dTable3;
        var contributor;
        function ContributorVideos1(contributorId)
        {
            contributor = contributorId;
            $('#modal-videos1').modal('toggle');

            if (dTable3 == null)
            {
                dTable3 = $('#contributors1').DataTable({
                    processing: true,
                    serverSide: true,
                    stateSave: false,
                    pagingType: "full_numbers",
                    paginate: false,
                    info: false,
                    filter: false,
                    sort: false,
                    ajax: {
                        "url": "/ContributorHistory.aspx",
                        "data": function (d) {
                            d.owner = contributor;
                        }
                    },
                    type: 'GET',
                    responsive: false,
                    pageLength: 10,
                    language: { "sSearch": "" },
                    order: [0, "asc"],
                    "columns": [
                        { "data": "ItemMonth", orderable: false },
                        { "data": "YouTubeEarnings", orderable: false },
                    <% =currencyScripts1%>
                    ]
                });
            }
            else
            {
                $("#contributors tbody").html('');
                dTable3.ajax.reload(null, false);
            }
        }

        function updatePaid(box, id) {
            var isChecked = box.checked == true ? true: false;
            $.ajax({
                url: "/Payments.aspx/UpdatePayment",
                type: "POST",
                cache: false,
                data: JSON.stringify({ contributor: id, isPaid: isChecked, paymentYear: year, paymentMonth: month }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                   $.gritter.add(
                    {
                        title: "Payment updated",
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
                        title: "Error updating payment",
                        class_name: 'growl-danger',
                        image: '/images/screen.png',
                        sticky: false,
                        time: 6000
                    });
                }
            });
        }
    </script>

    <script src="/js/jquery.dataTables.min.js"></script>
    <script src="/js/dataTables.bootstrap.js"></script>
    <script src="/js/dataTables.responsive.js"></script>
</asp:Content>