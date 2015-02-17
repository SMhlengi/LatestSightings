<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Videos.aspx.cs" Inherits="LatestSightings.Videos" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/style.datatables.css" rel="stylesheet">
    <link href="/css/dataTables.responsive.css" rel="stylesheet">

    <div class="row">
        <div class="col-md-12">
            <input type="button" name="btnPublished" value="Published" class="btn btn-success" onclick="location.href = '/videos/published';" />
            <input type="button" name="btnPending" value="Pending" class="btn btn-warning" onclick="location.href = '/videos/pending';" />
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
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">View</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Title</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Alias</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Contributor</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Id</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Revenue Split</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Prev Payment</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">MTD Revenue</th>
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

    <script>
        jQuery(document).ready(function () {
            dTable = $('#videos').dataTable({
                processing: true,
                serverSide: true,
                stateSave: false,
                pagingType: "full_numbers",
                "ajax": {
                    "url": "/VideosData.aspx",
                    "data": function ( d ) {
                        d.status = "<%= status %>";
                    }
                },
                type: 'GET',
                pageLength: 10,
                language: { "sSearch": "" },
                order: [1, "asc"],
                "columns": [
                    { orderable: false, "render": function (data, type, full, meta) { return "<a href=\"javascript:void(0);\" onclick=\"location.href='/video/" + full.Id + "';\")\">View</a>" } },
                    { "data": "Title", orderable: true },
                    { "data": "Alias", orderable: true },
                    { "data": "Contributor", orderable: true },
                    { "data": "YoutubeId", orderable: false },
                    { "data": "RevenueSplit", orderable: false },
                    { "data": "PreviousMonth", orderable: false },
                    { "data": "CurrentMonth", orderable: false }
                ]
            });

            /* Add events */
            $("body").on("click", "#videos tbody tr", function (e) {
                //e.preventDefault();
                //var dData = dTable.fnGetData(this, 0);
                //location.href = "/video/" + dData;
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
    </script>

    <script src="/js/jquery.dataTables.min.js"></script>
    <script src="/js/dataTables.bootstrap.js"></script>
    <script src="/js/dataTables.responsive.js"></script>
</asp:Content>