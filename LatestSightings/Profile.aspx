<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="LatestSightings.Profile" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/style.datatables.css" rel="stylesheet">
    <link href="/css/dataTables.responsive.css" rel="stylesheet">

    <div class="col-sm-4 col-md-3">
        <div class="text-center">
            <h4 class="profile-name mb5" id="name" runat="server"><%= person.FirstName %> <%= person.LastName %></h4>
            <div><%= LatestSightingsLibrary.Person.RoleName(person.Role) %></div>
                                
            <div class="mb20"></div>
                                
            <div class="btn-group">
                <input type="button" name="btn1" value="Edit Details" class="btn btn-success" onclick="parent.location='/contributor/<%= person.Id %>'" />
                <input type="button" name="btn1" value="Add Video" class="btn btn-success" onclick="parent.location='/video?contributor=<%= person.Id %>'" />
            </div>
        </div><!-- text-center -->
                                
        <br />
                              
        <h5 class="md-title">Contact</h5>
        <ul class="list-unstyled social-list" id="contacts" runat="server"></ul>
                              
        <h5 class="md-title">Connect</h5>
        <ul class="list-unstyled social-list" id="connect" runat="server"></ul>
                              
        <div class="mb30"></div>
                              
        <h5 class="md-title"><i class="fa fa-home"></i> Address</h5>
        <address>
            <%= !String.IsNullOrEmpty(person.Address) ? person.Address.Replace(Environment.NewLine, "<br />") : "Not available" %>
        </address>

        <div class="mb30"></div>
                              
        <h5 class="md-title"><i class="fa fa-bank"></i> Bank Details</h5>
        <address>
            <%= !String.IsNullOrEmpty(person.Banking) ? person.Banking.Replace(Environment.NewLine, "<br />") : "Not available" %>
        </address>

        <h5 class="md-title"><i class="fa fa-bank"></i> Paypal Details</h5>
        <address>
            <%= !String.IsNullOrEmpty(person.Paypal) ? person.Paypal.Replace(Environment.NewLine, "<br />") : "Not available" %>
        </address>
                              
    </div><!-- col-sm-4 col-md-3 -->

    <div class="col-sm-8 col-md-9">
        <div class="panel panel-primary-head">
            <div class="panel-heading">
                <div class="panel-btns">
                    <a href="" class="panel-minimize tooltips" data-toggle="tooltip" title="Minimize Panel"><i class="fa fa-minus"></i></a>
                    <a href="" class="panel-close tooltips" data-toggle="tooltip" title="Close Panel"><i class="fa fa-times"></i></a>
                </div><!-- panel-btns -->
                <h3 class="panel-title">Videos</h3>
            </div>
            <div class="panel-body">
                <div class="alert alert-info fade in nomargin" id="alert" runat="server" visible="false">
                    <button aria-hidden="true" data-dismiss="alert" class="close" type="button">×</button>
                    <h4>No Video!</h4>
                    <p>There are no videos loaded for this contributor to add a video click the "Add Video" button.</p>
                    <p>
                        <input type="button" name="btn1" value="Add Video" class="btn btn-info" onclick="parent.location='/video?contributor=<%= person.Id %>    '" />
                    </p>
                </div>

                <div class="row" id="tableRow" runat="server">
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
                                                <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Id</th>
                                                <th style="background-color:#554337; color: #FFFFFF; border-bottom: 0px;">Revenue Split</th>
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
            </div><!-- panel-body -->
        </div><!-- panel -->
    </div><!-- col-sm-8 col-md-93 -->

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
                        d.contributor = "<%= person.Id %>";
                    }
                },
                type: 'GET',
                responsive: false,
                pageLength: 10,
                language: { "sSearch": "" },
                order: [1, "asc"],
                "columns": [
                    { orderable: false, "render": function (data, type, full, meta) { return "<a href=\"javascript:void(0);\" onclick=\"location.href='/video/" + full.Id + "';\")\">View</a>" } },
                    { "data": "Title", orderable: true },
                    { "data": "Alias", orderable: true },
                    { "data": "YoutubeId", orderable: false },
                    { "data": "RevenueSplit", orderable: false }
                ]
            });

            /* Add events */
            $("body").on("click", "#videos tbody tr", function (e) {

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