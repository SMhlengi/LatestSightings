<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contributor.aspx.cs" Inherits="LatestSightings.Contributor" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="row">
    <div class="col-md-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">Add / Edit Contributor</h4>
                <p>Add or edit a contributor click the add user button to submit.</p>
            </div><!-- panel-heading -->
            <div class="panel-body">
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Firstname</label>
                        <asp:TextBox runat="server" ID="txtFirstName" Name="txtFirstName" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Surname</label>
                        <asp:TextBox runat="server" ID="txtSurname" name="txtSurname" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Email</label>
                        <asp:TextBox runat="server" ID="txtEmail" name="txtEmail" CssClass="form-control with-label" />
                    </div>
                </div><!-- row -->
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Cell number</label>
                        <asp:TextBox runat="server" ID="txtCell" name="txtCell" CssClass="form-control phonegroup with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Telephone number</label>
                        <asp:TextBox runat="server" ID="txtTelNumber" name="txtTelNumber" CssClass="form-control phonegroup with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Other contact details</label>
                        <asp:TextBox runat="server" ID="txtOther" name="txtOther" CssClass="form-control with-label" />
                    </div>
                </div><!-- row -->
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Skype address</label>
                        <asp:TextBox runat="server" ID="txtSkype" name="txtSkype" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Twitter handle</label>
                        <asp:TextBox runat="server" ID="txtTwitter" name="txtTwitter" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Facebook profile</label>
                        <asp:TextBox runat="server" ID="txtFacebook" name="txtFacebook" CssClass="form-control with-label" />
                    </div>
                </div><!-- row -->
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Address</label>
                        <asp:TextBox runat="server" ID="txtAddress" name="txtAddress" TextMode="MultiLine" Rows="6" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Banking</label>
                        <asp:TextBox runat="server" ID="txtBanking" name="txtBanking" TextMode="MultiLine" Rows="6" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Paypal</label>
                        <asp:TextBox runat="server" ID="txtPaypal" name="txtPaypal" TextMode="MultiLine" Rows="6" CssClass="form-control with-label" />
                    </div>
                </div><!-- row -->
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Account Type</label>
                        <asp:TextBox runat="server" ID="txtAccountType" name="txtAccountType" CssClass="form-control phonegroup with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Account Number</label>
                        <asp:TextBox runat="server" ID="txtAccountNo" name="txtAccountNo" CssClass="form-control phonegroup with-label" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Branch Name</label>
                        <asp:TextBox runat="server" ID="txtBranchName" name="txtBranchName" CssClass="form-control phonegroup with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Branch Code</label>
                        <asp:TextBox runat="server" ID="txtBranchCode" name="txtBranchCode" CssClass="form-control phonegroup with-label" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Password</label>
                        <asp:TextBox runat="server" ID="txtPassword" name="txtPassword" CssClass="form-control with-label" />
                    </div>
                </div><!-- row -->
                <div class="row">
                    <div class="form-group">
                        <div class="col-sm-9">
                            <label class="control-label">Role</label>
                            <div class="rdio rdio-primary">
                                <asp:RadioButton ID="rdbAdmin" runat="server" GroupName="gender" />
                                <label for="<%= rdbAdmin.ClientID %>">Admin</label>
                            </div><!-- rdio -->
                            <div class="rdio rdio-primary">
                                <asp:RadioButton ID="rdbFinance" runat="server" GroupName="gender" />
                                <label for="<%= rdbFinance.ClientID %>">Finance</label>
                            </div><!-- rdio -->
                            <div class="rdio rdio-primary">
                                <asp:RadioButton ID="rdbContributor" runat="server" GroupName="gender" />
                                <label for="<%= rdbContributor.ClientID %>">Contributor</label>
                            </div><!-- rdio -->
                        </div>
                    </div><!-- form-group -->
                </div><!-- row -->
                <div class="row">
                    <div class="form-group col-md-6">
                        <div class="ckbox ckbox-primary">
                            <asp:Checkbox ID="chbxActive" runat="server" Checked="true" />
                            <label for="<%= chbxActive.ClientID %>">Active</label>
                        </div>
                    </div>
                </div><!-- row -->
            </div><!-- panel-body -->
            <div class="panel-footer">
                <asp:Button  ID="btnSave" runat="server" OnClick="Save" CssClass="btn btn-success" Text="Submit" /> <input type="reset" name="reset" value="Reset" class="btn btn-danger" />
            </div><!-- panel-footer -->
        </div><!-- panel -->  
    </div><!-- col-md-12 -->
</div><!-- row -->

<script>
    $(document).ready(function () {
        $("#Form1").validate({
            highlight: function (element) {
                jQuery(element).closest('.form-group').removeClass('has-success').addClass('has-error');
            },
            success: function (element) {
                jQuery(element).closest('.form-group').removeClass('has-error');
            },
            rules: {
                <%= txtFirstName.UniqueID %>: {
                    required: true
                },
                <%= txtSurname.UniqueID %>: {
                    required: true
                },
                <%= txtEmail.UniqueID %>: {
                    email: true,
                    required: true
                },
                <%= txtCell.UniqueID %>: {
                    require_from_group: [1, ".phonegroup"]
                },
                <%= txtTelNumber.UniqueID %>: {
                    require_from_group: [1, ".phonegroup"]
                }
            },
            messages: {
                <%= txtEmail.UniqueID %>:
                {
                    required: "Please enter your email address",
                    email: "Please enter a valid email address"
                }
            }
        });
        $.validator.addMethod("require_from_group", $.validator.methods.require_from_group, 'Please enter at least one contact number');
    });
</script>
</asp:Content>