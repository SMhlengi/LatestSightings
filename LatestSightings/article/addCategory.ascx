<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addCategory.ascx.cs" Inherits="LatestSightings.addCategory" %>
<div class="row">
    <div class="col-sm-4">
        <div class="panel panel-primary">
            <div class="panel-heading">Captured Categories</div>
            <div class="panel-body">
                <table class="table table-striped table-condensed table-hover">
                    <tbody>
                        <% foreach(var item in categories){ %>
                        <tr>
                            <td width="90%" id='categoryNameId<%=item["id"] %>'><%=item["categoryname"] %></td>
                            <td width="10%">
                                <% if (item["categoryname"] != "unassigned"){ %>
                                <span class="glyphicon glyphicon-remove text-danger displayinline cursor deleteCategory action" id="<%=item["id"] %>" title="" data-original-title="" data-container="body" data-toggle="popover" data-placement="top" data-content="Click to delete the category"></span>
                                <span class="glyphicon glyphicon-edit text-success displayinline editCategory action editPencil<%=item["id"] %>" id="<%=item["id"] %>" style="margin-left: 10px;" title="" data-original-title="" data-container="body" data-toggle="popover" data-placement="top" data-content="Click to edit the category"></span>
                                <%} %>
                            </td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="panel panel-primary">
            <div class="panel-heading">Capture New Category</div>
            <div class="panel-body">
                <div class="row newCategoryrow">
                    <div class="col-xs-9 col-sm-9 col-md-9 col-lg-9">
                        <div class="form-group">
                            <input type="text" class="form-control newcategorycaptured" placeholder="New Category">
                        </div>
                    </div>
                    <div class="col-xs-3 col-sm-3 col-md-3 col-lg-=3">
                        <button type="button" class="btn btn-default btn-block saveCategory">Save</button>
                    </div>
                </div>
                <!-- edit category -->
                <div class="row editCategoryrow">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <p class="bg-primary capturenewcategory">Edit Category</p>
                    </div>
                </div>
                <div class="row editCategoryrow">
                    <div class="col-xs-9 col-sm-9 col-md-9 col-lg-9">
                        <div class="form-group">
                            <input type="text" class="form-control currentcategorycaptured" placeholder="New Category">
                        </div>
                    </div>
                    <div class="col-xs-3 col-sm-3 col-md-3 col-lg-=3">
                        <button type="button" class="btn btn-default btn-block UpdatedCategory">Update</button>
                    </div>
                    <!-- testing something -->
                    <div class="col-xs-9 col-sm-9 col-md-9 col-lg-9">
                    </div>
                    <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3 margintopminus10">
                        <button type="button" class="btn btn-danger btn-block CancellUpdatedCategory">Cancel</button>
                    </div>
                </div>
                <!-- end of edit category -->
                <!-- delete category -->
                <div class="row deleteCategoryrow">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <p class="bg-primary capturenewcategory">Delete Category</p>
                    </div>
                </div>
                <div class="row deleteCategoryrow deleteCategoryRowAlert">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="alert alert-danger backgroundcolorwhite" role="alert">
                            <p>Category <strong class="deleteCreatedCategory"></strong> will be deleted</p>
                            <p class="margintop10">
                            <button type="button" class="btn btn-default btn-sm proceedWithCategoryDelete">Proceed</button>
                            <button type="button" class="btn btn-danger btn-sm cancelCategoryDelete">Cancel</button>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- end of deletion -->
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="alert alert-info backgroundcolorwhite updatingcategory" role="alert">
                            <p><strong>Updating Category</strong>......</p>
                        </div>
                        <div class="alert alert-info backgroundcolorwhite deletinggcategory" role="alert">
                            <p><strong>Deleting Category</strong>......</p>
                        </div>
                        <div class="alert alert-info backgroundcolorwhite UnassignedCategoryExists" role="alert">
                            <p>This category is associated with articles. Please create <strong>Unassigned </strong> category to move these articles to</p>
                            <button type="button" class="btn btn-success btn-sm createUnassignedCategory">Create Unassigned Category </button>
                        </div>
                        <div class="alert alert-success backgroundcolorwhite categoryupdated" role="alert">
                            <p><strong>Updated Succesfully</strong>......Page been refreshed</p>
                        </div>
                        <div class="alert alert-success backgroundcolorwhite categoryDeleted" role="alert">
                            <p><strong>Category Succesfully deleted</strong>......Page been refreshed</p>
                        </div>
                        <div class="alert alert-info backgroundcolorwhite savingcategory" role="alert">
                            <p><strong>Saving Category</strong>......</p>
                        </div>
                        <div class="alert alert-success backgroundcolorwhite categorysaved" role="alert">
                            <p><strong>Saved Succesfully</strong>......Page been refreshed</p>
                        </div>
                        <div class="alert alert-danger backgroundcolorwhite categoryExists" role="alert">
                            <p><strong>Category already Exists......</strong></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script>
    $(document).ready(function () {
        $("#categories").attr("class", "active");
        $(".action").popover({ trigger: 'hover' });
    });
</script>