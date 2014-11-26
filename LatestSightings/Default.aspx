<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LatestSightings.Default" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/morris.css" rel="stylesheet">
    <div class="panel-body">
        <div class="row">
            <div class="col-md-4 mb30">
                <div class="panel panel-success-alt noborder">
                    <div class="panel-heading noborder">
                        <div class="panel-btns" style="display: block; opacity: 0;">
                            <a href="" class="panel-close tooltips" data-toggle="tooltip" title="" data-original-title="Close Panel"><i class="fa fa-times"></i></a>
                        </div><!-- panel-btns -->
                        <div class="panel-icon"><i class="fa fa-dollar"></i></div>
                        <div class="media-body">
                            <h5 class="md-title nomargin" id="earMnth" runat="server"></h5>
                            <h1 class="mt5" id="earMnthTot" runat="server"></h1>
                        </div><!-- media-body -->
                        <hr>
                        <div class="clearfix mt20">
                            <div class="pull-left">
                                <h5 class="md-title nomargin" id="earMnth1" runat="server"></h5>
                                <h4 class="nomargin" id="earMnth1Tot" runat="server"></h4>
                            </div>
                            <div class="pull-right">
                                <h5 class="md-title nomargin" id="earMnth2" runat="server"></h5>
                                <h4 class="nomargin" id="earMnth2Tot" runat="server"></h4>
                            </div>
                        </div>
                                        
                    </div><!-- panel-body -->
                </div>
            </div>
            <div class="col-md-4 mb30">
                <div class="panel panel-primary noborder">
                    <div class="panel-heading noborder">
                        <div class="panel-btns" style="display: block; opacity: 0;">
                            <a href="" class="panel-close tooltips" data-toggle="tooltip" title="" data-original-title="Close Panel"><i class="fa fa-times"></i></a>
                        </div><!-- panel-btns -->
                        <div class="panel-icon"><i class="fa fa-users"></i></div>
                        <div class="media-body">
                            <h5 class="md-title nomargin" id="viewMnth" runat="server"></h5>
                            <h1 class="mt5" id="viewMnthTot" runat="server"></h1>
                        </div><!-- media-body -->
                        <hr>
                        <div class="clearfix mt20">
                            <div class="pull-left">
                                <h5 class="md-title nomargin" id="viewMnth1" runat="server"></h5>
                                <h4 class="nomargin" id="viewMnthTot1" runat="server"></h4>
                            </div>
                            <div class="pull-right">
                                <h5 class="md-title nomargin" id="viewMnth2" runat="server"></h5>
                                <h4 class="nomargin" id="viewMnthTot2" runat="server"></h4>
                            </div>
                        </div>
                                        
                    </div><!-- panel-body -->
                </div>
            </div>
            <div class="col-md-4 mb30">
                <div class="panel panel-dark noborder">
                    <div class="panel-heading noborder">
                        <div class="panel-btns" style="display: block; opacity: 0;">
                            <a href="" class="panel-close tooltips" data-toggle="tooltip" title="" data-original-title="Close Panel"><i class="fa fa-times"></i></a>
                        </div><!-- panel-btns -->
                        <div class="panel-icon"><i class="fa fa-video-camera"></i></div>
                        <div class="media-body">
                            <h5 class="md-title nomargin">Total Videos</h5>
                            <h1 class="mt5" id="vidTot" runat="server"></h1>
                        </div><!-- media-body -->
                        <hr>
                        <div class="clearfix mt20">
                            <div class="pull-left">
                                <h5 class="md-title nomargin">Published</h5>
                                <h4 class="nomargin" id="vidPub" runat="server"></h4>
                            </div>
                            <div class="pull-right">
                                <h5 class="md-title nomargin">Pending</h5>
                                <h4 class="nomargin" id="vidPen" runat="server"></h4>
                            </div>
                        </div>
                                        
                    </div><!-- panel-body -->
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="row">
                    <div class="col-md-6 mb30">
                        <div class="panel panel-default">
                            <div class="panel-body padding15">
                                <h5 class="md-title mt0 mb10">Earnings</h5>
                                <div class="col-md-12 mb30">
                                    <div id="line-chart" class="height300"></div>
                                </div>
                            </div><!-- panel-body -->
                            <div class="panel-footer">
                                <div class="tinystat pull-left">

                                </div><!-- tinystat -->
                                <div class="tinystat pull-right">
                                    <div id="sparkline4" class="chart mt5"></div>
                                    <div class="datainfo">
                                        <span class="text-muted">Total</span>
                                        <h4><asp:Literal ID="ltlEarnings" runat="server" /></h4>
                                    </div>
                                </div><!-- tinystat -->
                            </div><!-- panel-footer -->
                        </div><!-- panel -->
                    </div>
                    <div class="col-md-6 mb30">
                        <div class="panel panel-default">
                            <div class="panel-body padding15">
                                <h5 class="md-title mt0 mb10">Views</h5>
                                <div class="col-md-12 mb30">
                                    <div id="line-chart1" class="height300"></div>
                                </div>
                            </div><!-- panel-body -->
                            <div class="panel-footer">
                                <div class="tinystat pull-left">

                                </div><!-- tinystat -->
                                <div class="tinystat pull-right">
                                    <div id="Div2" class="chart mt5"></div>
                                    <div class="datainfo">
                                        <span class="text-muted">Total</span>
                                        <h4><asp:Literal ID="ltlViews" runat="server" /></h4>
                                    </div>
                                </div><!-- tinystat -->
                            </div><!-- panel-footer -->
                        </div><!-- panel -->
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 mb30">
                        <div class="panel panel-default">
                            <div class="panel-body padding15">
                                <h5 class="md-title mt0 mb10">Earnings</h5>
                                <div class="col-md-12 mb30">
                                    <div id="line-chart2" class="height300"></div>
                                </div>
                            </div><!-- panel-body -->
                        </div><!-- panel -->
                    </div>
                    <div class="col-md-6 mb30">
                        <div class="panel panel-default">
                            <div class="panel-body padding15">
                                <h5 class="md-title mt0 mb10">Views</h5>
                                <div class="col-md-12 mb30">
                                    <div id="line-chart3" class="height300"></div>
                                </div>
                            </div><!-- panel-body -->
                        </div><!-- panel -->
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <ul class="nav nav-tabs nav-justified">
	                <li class="active"><a data-toggle="tab" href="#popular">Top 10 Earnings</a></li>
	                <li class=""><a data-toggle="tab" href="#recent">Top 10 Views</a></li>
	                <li class=""><a data-toggle="tab" href="#comments">Top 10 Countries</a></li>
                </ul>
                <div class="tab-content">
	                <div id="popular" class="tab-pane active">
		                <div class="table-responsive">
                            <table class="table table-primary mb30">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th>Title</th>
                                        <th>Earnings</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="ltlTop10Earnings" runat="server" />
                                </tbody>
                            </table>
                        </div> 
	                </div><!-- tab-pane -->
  
	                <div id="recent" class="widget-bloglist tab-pane">
		                <div class="table-responsive">
                            <table class="table table-primary mb30">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th>Title</th>
                                        <th>Views</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="ltlTop10Views" runat="server" />
                                </tbody>
                            </table>
                        </div> 
	                </div><!-- tab-pane -->
  
	                <div id="comments" class="widget-bloglist tab-pane">
		                <div class="table-responsive">
                            <table class="table table-primary mb30">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th>Country</th>
                                        <th>Views</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="ltlTop10Countries" runat="server" />
                                </tbody>
                            </table>
                        </div> 
	                </div><!-- tab-pane -->
                </div>
            </div>
        </div>
    </div>

    <script src="/graphs.aspx?year=<%=year %>&month=<%=month %>"></script>
    <script src="/js/morris.min.js"></script>
    <script src="/js/raphael-2.1.0.min.js"></script>
</asp:Content>