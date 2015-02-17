<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addnewarticle.ascx.cs" Inherits="LatestSightings.addnewarticle" %>
<script>
    function saveArticle() {
        $(".articleNotCompletelyFilledIn").hide();
        $(".articleBeenSaved").show();
        if (articleSaved == false) {
            if (allDataFilledIn() == false) {
                $(".articleBeenSaved").hide();
                $(".articleNotCompletelyFilledIn").show();
                return false;
            }

            var postUrl = "/AjaxFunction.aspx/saveArticle";
            var draft = 0;
            var completed = 0;

            if (document.getElementById("draft").checked) {
                draft = 1;
            }

            if (document.getElementById("completed").checked) {
                completed = 1;
            }

            var articlebody = tinyMCE.activeEditor.getContent();
            articlebody = articlebody.replace(/'/g, "\\'");

            var articleheading = $("#articleheading").val();
            articleheading = articleheading.replace(/'/g, "\\'");
            

            if ($('#category').length) {
                categoryid = $("#category").val();
            }

            $.ajax({
                type: "POST",
                url: postUrl,
                data: "{'heading' : '" + articleheading + "', 'body' : '" + articlebody + "', 'categoryid' : '" + categoryid + "', 'draft' : '" + draft + "', 'complete' : '" + completed + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(
                function (data, textStatus, jqXHR) {
                    if (data.d.articleSaved == 1) {
                        articleSaved = true;
                        setTimeout(function () { $(".buttonSubmit").click(); }, 2500);
                    }
                }
            ).fail(
                function (data, textStatus, jqXHR) {
                }
            );
            return false;
        } else {
            return articleSaved;
        }
    }

    function allDataFilledIn() {
        var dataCaptured = false;
        if (tinyMCE.activeEditor.getContent() != "" && $("#articleheading").val() != "") {
            if (document.getElementById("draft").checked || document.getElementById("completed").checked) {
                if ($('#category').length) {
                    categoryid = $("#category").val();
                }

                if (categoryid != "") {
                    dataCaptured = true;
                }
            }
        }

        return dataCaptured;
    }
</script>
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
                        Please choose the Category for the new Article.
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-2 form-control-nomargin">
                        <select id="category">
                            <% foreach(var category in categories){ %>
                                <option value="<%=category["id"] %>"><%=category["categoryname"] %></option>
                            <%} %>
                        </select>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 margintop20">
                                <input type="text" class="form-control" placeholder="Heading" id="articleheading" />
                            </div>
                        </div>
                        <div class="row articletable">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 addnewarticleTinyMcContainer">
                		                <textarea id="articlebody"></textarea>
                            </div>
                            <!-- article image-->
                            <div class="col-xs-7 col-sm-7 col-md-7 col-lg-7 captureitemsheadings">
                                <div class="panel panel-default">
                                    <div class="panel-heading">Article Image</div>
                                    <div class="panel-body">
                                        <asp:FileUpload ID="articleImage" runat="server"/>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-5 col-sm-5 col-md-5 col-lg-5 captureitemsheadings">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Article Status
                                    </div>
                                    <div class="panel-body">
                                            <div class="radio">
                                                <label class="radio-inline">
                                                    <input type="radio" name="articlestatus" id="draft" />Draft
                                                </label>
                                                <label class="radio-inline">
                                                    <input type="radio" name="articlestatus" id="completed" />Completed
                                                </label>
                                            </div>
                                    </div>
                                </div>
                            </div>
                            <!-- end of article image -->
                        </div>
                        <div class="row">
                            <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4">
                                <asp:Button ID="saveArticle" runat="server" Text="Save Article" class="btn btn-default btn-block buttonSubmit" OnClick="saveArticle_Click" OnClientClick="return saveArticle()"/>
                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6 col-lg-6">
                                <div class="alert alert-danger backgroundcolorwhite articleNotCompletelyFilledIn" role="alert">
                                    <strong>Some Article attributes have not been filled in!!</strong><br />
                                    Please complete and save the article
                                </div>
                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6 col-lg-6">
                                <div class="alert alert-info backgroundcolorwhite articleBeenSaved" role="alert">
                                    <strong>Article been saved</strong><br />
                                    Please wait.......
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script>

    $(document).ready(function () {
        $("#articles").attr("class", "active");
        jQuery('#category').select2({
            minimumResultsForSearch: -1
        });

    });

</script>
