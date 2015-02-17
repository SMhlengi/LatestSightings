<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeaturedCategories.aspx.cs" Inherits="LatestSightings.FeaturedCategories" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <asp:Panel runat="server" id="main" cssclass="row">
        <div class="col-md-4">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">Featured Categories</h4>
                    <p>Select the categories you would like to feature number 1 being the most prevelant. Set the drop down to "Blank" if you do not wish to feature that many</p>
                </div><!-- panel-heading -->
                <div class="panel-body">
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            1.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory1" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            2.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory2" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            3.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory3" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            4.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory4" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            5.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory5" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            6.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory6" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            7.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory7" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            8.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory8" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            9.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory9" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-1" style="line-height: 40px; text-align: right">
                            10.
                        </div>
                        <div class="form-group col-md-11">
                            <asp:DropDownList ID="ddlCategory10" data-placeholder="Choose One" CssClass="categorySelect" runat="server" ClientIDMode="Static" style="width: 100%;" />
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:Button OnClick="Save" runat="server" CssClass="btn btn-success" Text="Save" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <script>
        jQuery(document).ready(function() {
            jQuery(".categorySelect").select2();
        })
    </script>
</asp:Content>