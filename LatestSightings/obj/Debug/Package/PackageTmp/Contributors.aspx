<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contributors.aspx.cs" Inherits="LatestSightings.Contributors" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/style.datatables.css" rel="stylesheet">
    <link href="/css/dataTables.responsive.css" rel="stylesheet">
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-primary-head">
                <div id="basicTable_wrapper" class="dataTables_wrapper no-footer" style="width: 100%;">
                    <div class="dataTables_length" id="basicTable_length" style="width: 100%;">
                        <table id="contributors" class="table table-striped responsive dataTable no-footer" role="grid" aria-describedby="basicTable_info" style="width: 100%;" width="100%" cellspacing="0">
                            <thead class="datatableHeaders">
                                <tr>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">View</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Firstname</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Lastname</th>
                                    <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Email</th>
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
        jQuery(document).ready(function(){
            dTable = $('#contributors').dataTable({
                processing: true,
                serverSide: true,
                stateSave: false,
                pagingType: "full_numbers",
                ajax: "/ContributorsData.aspx",
                type: 'GET',
                pageLength: 10,
                language: { "sSearch": "" },
                order: [1, "asc"],
                "columns": [
                    { orderable: false, "render": function (data, type, full, meta) { return "<a href=\"javascript:void(0);\" onclick=\"location.href='/profile/" + full.Id + "';\")\">View</a>" } },
                    { "data": "Firstname", orderable: true },
                    { "data": "Lastname", orderable: true },
                    { "data": "Email", orderable: false }
                ]
            });

            /* Add events */
            $("body").on("click", "#contributors tbody tr", function (e) {
                /*
                var nTds = $('td', this);
                var sBrowser = $(nTds[1]).text();
                var sGrade = $(nTds[1]).text();
                var dialogText = "The info cell I need was in (col2) as:" + sBrowser + " and in (col5) as:" + sGrade + "";
                var targetUrl = $(this).attr("href");

                $('#table-dialog').modal();
                $('#modal-body').text(dialogText);
                */
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