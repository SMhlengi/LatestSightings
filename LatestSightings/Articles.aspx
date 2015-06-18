<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Articles.aspx.cs" Inherits="LatestSightings.Articles" MasterPageFile="~/DefaultMaster.Master" %>
<%@ Register Src="~/article/uc_norecords.ascx" TagName="records" TagPrefix="no" %>
<%@ Register Src="~/article/addnewarticle.ascx" TagName="article" TagPrefix="addnew" %>
<%@ Register Src="~/article/addCategory.ascx" TagName="category" TagPrefix="view" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/article.css" rel="stylesheet">
    <script src="/js/article.js"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/assets/tinymce/tinymce.min.js")%>"></script>
	<script type="text/javascript">
	    tinymce.init({
	        selector: "textarea",
	        height: 380,
	        auto_focus: "articlebody",
	        plugins: ["wordcount", "link"]
	    });
	</script>
    <script>
        var categoryid = "";
        var articleSaved = false;
    </script>
    <div class="row">
        <div class="col-md-12">
            <% if (View == "norecords")
                       { %>         
                <no:records id="norecordscontrol" runat="server" />    
                <%}
                   else if (View == "addnewarticle")
                       { %>       
                <addnew:article ID="newarticle" runat="server" />
                    <%}
                   else if (View == "articlesboard")
                    { %>
                        <asp:Placeholder ID="articlesboard_two" runat="server" />
                <%}
                   else if (View == "addnewcategory")
                    { %>
                    <view:category ID="addnewcategory" runat="server" />
                <%}
                   else if (View == "editarticle")
                       { %>
                    <asp:PlaceHolder id="editArticle" runat="server" />
                    <%} %>
        </div>
    </div>
</asp:Content>