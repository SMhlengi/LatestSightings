<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Images.aspx.cs" Inherits="LatestSightings.Images" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .gallery {
            height: 140px;
        }
        .imgthumbnail {
            text-align: center;
            border: 1px solid #DCDCDC;
            padding: 15px 0 15px 0;
            min-height: 140px;
        }
        .carousel-control {
            top: 60%;
        }
    </style>
    
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-primary-head">
                    <div class="panel-heading">
                        <h3 class="panel-title">Images</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row" style="margin-bottom: 10px;">
                            <div class="col-md-12">
                                <div style="float: right;" class="input-group">
                                    <asp:TextBox runat="server" ID="txtMonthPicker" Name="txtMonthPicker" placeholder="Month" CssClass="form-control" />
                                </div>
                                <div class="ckbox ckbox-warning" style="float: right; margin: 7px 10px 0 10px">
                                    <input type="checkbox" value="1" id="checkboxwarning" checked="checked">
                                    <label for="checkboxwarning">Not Approved</label>
                                </div>
                                <div class="ckbox ckbox-success" style="float: right; margin-top: 7px;">
                                    <input type="checkbox" value="2" id="checkboxsuccess" checked="checked">
                                    <label for="checkboxsuccess">Approved</label>
                                </div>
                            </div>
		                </div>
                        <div class="row" style="min-height: 130px; position: relative;">
                            <div class="col-md-12 col-md-offset-0">
                                <div class="loader">
                                   <center>
                                       <img class="loading-image" src="/images/loaders/loader10.gif" alt="loading..">
                                   </center>
                                </div>
                                <div id="myCarousel" class="carousel gallery">
			                        <div class="carousel-inner" id="carousel-inner">

			                        </div>
			                        <a data-slide="prev" href="#myCarousel" class="left carousel-control"><</a>
                                    <a data-slide="next" href="#myCarousel" class="right carousel-control">></a>
		                        </div>
                            </div>
                        </div>
                    </div>
                </div>
		
        </div><!-- col-md-12 -->
    </div><!-- row -->

    <script src="/js/toggles.min.js"></script>
    <script src="/js/bootstrap-timepicker.min.js"></script>
    <script src="/js/images.js"></script>
    <script>
        $(document).ready(function () {
            $('#<%= txtMonthPicker.ClientID %>').datepicker({
                dateFormat: 'MM yy',
                onSelect: function (date) {
                    LoadImages(date);
                }
            });
            checkitem();
            $("#myCarousel").on("slid.bs.carousel", "", checkitem);
            LoadImages('<%=startDate%>');

            $('.ckbox').change(function () {
                LoadImages($('#<%= txtMonthPicker.ClientID %>').val());
            });
        })
    </script>
</asp:Content>