<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Video.aspx.cs" Inherits="LatestSightings.Video" MasterPageFile="~/DefaultMaster.Master" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .thirdPartyTemplate {
            display: none;
            height: 0px;
            width: 0px;
        }
        .form-group-alert {
            margin-bottom: 0px;
        }

        .fileUpload {
	        position: relative;
	        overflow: hidden;
	        margin: 10px;
        }
        .fileUpload input.upload {
	        position: absolute;
	        top: 0;
	        right: 0;
	        margin: 0;
	        padding: 0;
	        font-size: 20px;
	        cursor: pointer;
	        opacity: 0;
	        filter: alpha(opacity=0);
        }
    </style>
    <div class="row">
    <div class="col-md-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">Add / Edit Video</h4>
                <p>Add or edit a video click the add video button to submit.</p>
            </div><!-- panel-heading -->
            <div class="panel-body">
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Title</label>
                        <asp:TextBox runat="server" ID="txtTitle" Name="txtTitle" CssClass="form-control with-label" required />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Alias</label>
                        <asp:TextBox runat="server" ID="txtAlias" Name="txtAlias" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Date Received</label>
                        <asp:TextBox runat="server" ID="txtRecievedPicker" Name="txtRecievedPicker" placeholder="mm/dd/yyyy" CssClass="form-control with-label" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Youtube ID</label>
                        <asp:TextBox runat="server" ID="txtYoutubeId" Name="txtYoutubeId" CssClass="form-control with-label" ClientIDMode="Static" />
                    </div>
                    <div class="form-group col-md-4">
                        <div style="margin-bottom: 4px;"><label class="control-label">Status</label><span id="ttSI" class="glyphicon glyphicon-question-sign popovers" title="" data-original-title="Status"  style="margin-left: 5px;" data-container="body" data-toggle="popover" data-placement="top"></span></div>
                        <asp:DropDownList ID="ddlStatus" runat="server" data-placeholder="Choose One" style="width: 100%;" ClientIDMode="Static">
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="Published" Value="Published" />
                            <asp:ListItem Text="UnPublished" Value="UnPublished" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-4" style="margin-top: 18px;">
                        <p style="display: inline-block;">
                            <asp:Button  ID="Button1" runat="server" OnClick="SendEmail" CssClass="btn btn-success cancel with-label" Text="Send Publish Letter" />
                        </p>
                    </div>
                </div>
                <asp:PlaceHolder ID="plcThirdParties" runat="server"></asp:PlaceHolder>
                <div class="row" id="Div2">
                    <div class="form-group col-md-4">
                        <a href="javascript:void(0);" onclick="addThirdParty();">Insert Third Party </a> or if the third party does not exist <a href="javascript:void(0);"  data-toggle="modal" data-target="#modal-thirdparty">Add a Third Party</a>
                    </div>
                </div>
                <div id="container"></div>
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">T&Cs accepted on</label>
                        <asp:TextBox runat="server" ID="txtIpPicker" Name="txtIpPicker" placeholder="mm/dd/yyyy" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4" style="margin-top: 15px; ">
                        <p style="display: inline-block;">
                            <asp:TextBox ID="txtFile" runat="server" CssClass="form-control" placeholder="Choose File" Enabled="false" style="width: 200px; display: inline-block;" />
                        </p>
                        <div class="fileUpload btn btn-primary" style="display:  inline-block;">
                            <span id="uploadSpan" runat="server">Upload</span>
                            <asp:FileUpload id="File1" CssClass="upload" runat="server" />
                        </div>
                        <asp:HyperLink ID="ipLink" runat="server" Text="View IP Document" Visible="false" />
                    </div>
                    <div class="form-group col-md-4" id="divContributor"  runat="server" visible="false">
                        <label class="control-label">Change Contributor</label>
                        <asp:TextBox runat="server" ID="txtContributor" Name="txtContributor" CssClass="form-control with-label" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-4">
                        <div style="margin-bottom: 4px;"><label class="control-label">Revenue Share on this Video (<a href="javascript:void(0);"  data-toggle="modal" data-target="#modal-revenue">Add a Revenue Share</a>)</label></div>
                        <select name="ddlRevenueShare" id="ddlRevenueShare" data-placeholder="Choose One" style="width: 100%;" runat="server">
                            
                        </select>
                    </div>
                </div>
                    <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Keywords</label>
                        <asp:TextBox runat="server" ID="txtKeywords" Name="txtTitle" CssClass="form-control with-label" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="control-label">Region</label>
                        <asp:TextBox runat="server" ID="txtRegion" Name="txtTitle" CssClass="form-control with-label" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-4">
                        <label class="control-label">Notes</label>
                        <asp:TextBox runat="server" ID="txtNotes" name="txtAddress" TextMode="MultiLine" Rows="6" CssClass="form-control with-label" />
                    </div>
                </div><!-- row -->
                <div class="row">
                    <div class="form-group col-md-6">
                        <div class="ckbox ckbox-primary">
                            <asp:Checkbox ID="chbxStream" runat="server" Checked="false" />
                            <label for="<%= chbxStream.ClientID %>">Live Stream</label>
                        </div>
                    </div>
                </div>
            </div>
             <div class="panel-footer">
                <asp:Button  ID="btnSave" runat="server" OnClick="Save" CssClass="btn btn-success" Text="Submit" /> <button class="btn btn-danger" onclick="ResetForm();">Reset</button> <button id="btnWatch" runat="server" visible="false" class="btn btn-primary youtube cancel">Watch Video</button>
                <div style="float: right; width: 165px; height: 40px;" id="divRecalculate" runat="server" visible="false">
                    <button class="confirm btn btn-primary" type="button">Recalculate Earnings</button>
                </div>
            </div><!-- panel-footer -->
        </div>
    </div>
    <div class="thirdPartyTemplate">
        <div class="row  alert alert-info" id="thirdParty">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
            <div class="form-group col-md-4 form-group-alert">
                <select name="ddlSelectThirdParty" id="ddlSelectThirdParty" runat="server" data-placeholder="Choose One" style="width: 100%;" ClientIDMode="Static" required />
            </div>
            <div class="form-group col-md-4 form-group-alert">
                <input type="text" class="form-control" placeholder="Alias" name="alias" id="alias" required>
            </div>
            <div class="form-group col-md-4 form-group-alert">
                <span id="ttTPI" class="glyphicon glyphicon-question-sign popovers" title="" data-original-title="Third Party Alias"  style="margin-left: 5px;" data-container="body" data-toggle="popover" data-placement="top"></span>
            </div>
        </div>
    </div>

    <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" id="modal-revenue">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button aria-hidden="true" data-dismiss="modal" class="close" type="button" id="modalCloseRevenue">&times;</button>
                    <h4 class="modal-title">Add Revenue Share</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group col-md-12">
                            Input the percentage the contributor will ceceive eg: 50
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <input type="text"  class="form-control" placeholder="Contributor Share" name="newShare" id="newShare" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <input type="button" class="btn btn-success cancel" onclick="AddRevenueShare('<%= ddlRevenueShare.ClientID %>');" value="Add" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" id="modal-thirdparty">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button aria-hidden="true" data-dismiss="modal" class="close" type="button" id="modalCloseThirdParty">&times;</button>
                    <h4 class="modal-title">Add Third Party</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group col-md-12">
                            Input the third party name eg: Youtube
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <input type="text"  class="form-control" placeholder="Third Party Name" name="newThirdParty" id="newThirdParty" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <input type="button" class="btn btn-success cancel" onclick="AddNewThirdParty('<%= ddlSelectThirdParty.ClientID %>');" value="Add" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

         <div id="popoverttSI" style="display: none">
            Pending: Anything up until Youtube ID entered by administrator<br /><br />
            Published: All videos on Youtube that are earning money<br /><br />
            Pending: The video is no longer on Youtube earning money.<br /><br />
        </div>

        <div id="popoverttTPI" style="display: none">
            Third party alias means another name the third party might have called the video.
        </div>
    <script>
        $("#ttSI").popover({
            trigger: 'hover',
            html : true, 
            content: function() {
                return $('#popoverttSI').html();
            }
        });
        var thirdPaties = <%= thirdPartiesCount %>;
    </script>
    <script src="/js/bootstrap-timepicker.min.js"></script>
    <script src="/js/jquery.confirm.min.js"></script>
    <script src="/js/bootstrap.youtubepopup.min.js"></script>
    <script src="/js/video.aspx.js"></script>
    <script>
        var videoId = "<%= id %>";
        $("#<%= File1.ClientID %>").change(function () {
            var filenanme = this.value;
            if (filenanme.indexOf("\\") >= 0) {
                filenanme = filenanme.substring(filenanme.lastIndexOf('\\') + 1);
            } else if (filenanme.indexOf("/") >= 0)
            {
                filenanme = filenanme.substring(filenanme.lastIndexOf('/') + 1);
            }
            $('#<%= txtFile.ClientID %>').val(filenanme);
        });

        $(document).ready(function () {

            jQuery('#<%= txtRecievedPicker.ClientID %>').datepicker();
            jQuery('#<%= txtIpPicker.ClientID %>').datepicker();
            jQuery('#<%= ddlRevenueShare.ClientID %>').select2({
                minimumResultsForSearch: -1
            });
            jQuery('#<%= ddlStatus.ClientID %>').select2({
                minimumResultsForSearch: -1
            });
            <%= PageLoadScript %>

            $("#Form1").validate({
                highlight: function (element) {
                    jQuery(element).closest('.form-group').removeClass('has-success').addClass('has-error');
                },
                success: function (element) {
                    jQuery(element).closest('.form-group').removeClass('has-error');
                }
            })
        })
    </script>
<script type="text/javascript">
    $(function () {
        $(".youtube").YouTubeModal({autoplay:0, width:640, height:480, youtubeId: jQuery('#<%= txtYoutubeId.ClientID %>').val()});
    });
</script>
</asp:Content>