<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoSorting.aspx.cs" Inherits="LatestSightings.VideoSorting" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link rel="stylesheet" href="http://code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css">
<script src="/js/videosorting.js"></script>
<div class="row">
    <div class="col-md-12">
        <div class="panel panel-primary-head">
                <div class="panel-heading">
                    <h3 class="panel-title">Videos</h3>
                </div>
                <div class="panel-body">
                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-md-10" style="line-height: 40px;">Click and image to add it to the featured videos</div>
                        <div class="col-md-2">
                            <div class="input-group">
                                <input id="searchBox" name="searchBox" type="search" class="form-control" placeholder="Search..." />
                                <span class="input-group-addon" id="searchButton"><i class="fa fa-search"></i></span>
                            </div>
                        </div>
		            </div>
                    <div class="row" style="min-height: 175px; position: relative;">
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
		
    </div><!-- col-md-12 -->
</div><!-- row -->

<div class="row">
    <div class="col-md-8 col-md-offset-2">
            <div class="panel panel-primary-head">
                <div class="panel-heading">
                    <h3 class="panel-title">Featured Videos</h3>
                </div>
                <div class="panel-body">
                    <div class="loader1">
                       <center>
                           <img class="loading-image" src="/images/loaders/loader10.gif" alt="loading..">
                       </center>
                    </div>
                    <div id="sortable">
                        
                    </div>
                </div>
            </div>
        </div><!-- col-md-12 -->
</div><!-- row -->
</asp:Content>