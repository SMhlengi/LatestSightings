<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="articlesBoard.ascx.cs" Inherits="LatestSightings.articlesBoard" %>
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
                        Choose a category to view the articles.
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-2 form-control-nomargin">
                        <select id="category" class="articleBoardCategory">
                    <% foreach(var category in categories){ %>
                        <option value="<%=category["id"] %>" <% if(Convert.ToInt32(category["id"]) == categoryId){ %> selected="selected" <%} %>><%=category["categoryname"] %></option>
                    <%} %>
                </select>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <table class="table table-striped table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th>Article Heading</th>
                                        <th>Status</th>
                                        <th>Date Created</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                 <% foreach(var item in articles){ %>
                                    <tr>
                                        <td width="35%" class="header<%=item["id"] %>"><%=item["header"] %></td>
                                        <td width="30%">
                                            <% if (item["draft"] == "True"){ %>
                                                <span class="label label-danger">Draft</span>
                                            <%}else if (item["complete"] == "True"){ %>
                                                <span class="label label-success">Completed</span>
                                            <%} %>
                                        </td>
                                        <td width="25%">
                                            <%=item["dateCreated"] %>
                                        </td>
                                        <td width="10%">
                                            <span class="glyphicon glyphicon-zoom-in text-default displayinline cursor action" id="Span1"  title="" data-toggle="modal" data-target="#myModal<%=item["id"] %>" data-original-title="" data-container="body" data-toggle="popover" data-placement="top" data-content="Click to view the article"></span>
                                            <span class="glyphicon glyphicon-remove text-danger displayinline cursor delete action" id='<%=item["id"] %>' style="padding-left: 10px;"  title="" data-original-title="" data-container="body" data-toggle="popover" data-placement="top" data-content="Click to delete the article"></span>
                                            <span class="glyphicon glyphicon-edit text-success displayinline cursor editArticle action"  id="<%=item["id"] %>" style="padding-left: 10px;"  title="" data-original-title="" data-container="body" data-toggle="popover" data-placement="top" data-content="Click to edit the article"></span>
                                        </td>
                                    </tr>
                                 <%} %>
        <%--                        <% foreach(KeyValuePair<string,string> en_trans in englishTrans){ %>
                                <tr>
                                    <td width="35%"><%= en_trans.Value %></td>
                                    <td>
                                        <% if (String.IsNullOrEmpty(portugueseTrans[en_trans.Key])){ %>
                                            <span class="label label-danger">Missing portuguese </span><span class="<%=en_trans.Key %>"></span>
                                        <%}else{ %>
                                            <span class="label label-success" data-container="body" data-toggle="popover" data-placement="right" data-original-title="Captured translation" data-content="<%=portugueseTrans[en_trans.Key] %>" data-trigger="hover">Portuguese captured</span><span class="<%=en_trans.Key %>"></span>
                                        <%} %>
                                    </td>
                                    <td><a href="Javascript:void(0);" class="anchoroveride edittranslation"><span class="glyphicon glyphicon-pencil"></span><small class="edittrans" id="<%= en_trans.Key %>"> [ edit ]</small></a></td>
                                </tr>
                                <%} %>--%>
                                </tbody>
                            </table>
                        </div>

                        <!-- delete article -->
                        <div class="row deleteArticle">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div class="alert alert-danger backgroundcolorwhite" role="alert">
                                    <p>Article <strong class="articleContent"></strong> will be deleted</p>
                                    <p>
                                    <button type="button" class="btn btn-default btn-sm proceedWithArticleDelete">Proceed</button>
                                    <button type="button" class="btn btn-danger btn-sm cancellArticleDelete">Cancel</button>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <!-- end delete article -->
                        <div class="row onArticleDeletingMessage">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div class="alert alert-info backgroundcolorwhite" role="alert">
                                    <strong>Deleting article </strong> Please wait .....
                                </div>
                            </div>
                        </div>
                        <div class="row onArticleDeletingSuccessfully">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div class="alert alert-success backgroundcolorwhite" role="alert">
                                    <p><strong>Deleted Succesfully</strong>......Page been refreshed</p>
                                </div>
                            </div>
                        </div>
                        <% if (articles.Count == 0){ %>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div class="alert alert-danger backgroundcolorwhite" role="alert">
                                    <strong>No Articles </strong> have been captured for this category .....
                                </div>
                            </div>
                        </div>
                        <%} %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Modal -->
        <%foreach (var item in articles){ %>
            <div class="modal fade" id="myModal<%=item["id"] %>" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
              <div class="modal-dialog modal-lg">
                <div class="modal-content">
                  <div class="modal-header">
                    <!--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button> -->
                    <h4 class="modal-title" id="myModalLabel"><%=item["header"] %></h4>
                  </div>
                  <div class="modal-body">
                      <%=item["body"] %>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <!--<button type="button" class="btn btn-primary">Save changes</button>-->
                  </div>
                </div>
              </div>
            </div>
    <%} %>

<script>
    $(document).ready(function () {
        $("#dashboard").attr("class", "active");
        $(".action").popover({ trigger: 'hover' });
    });
</script>