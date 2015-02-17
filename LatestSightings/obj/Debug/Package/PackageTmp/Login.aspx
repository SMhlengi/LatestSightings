<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LatestSightings.Login" %>
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
        <link href="/css/style.default.css" rel="stylesheet">
        <link href="/css/tooltipster.css" rel="stylesheet">
        <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
        <!--[if lt IE 9]>
        <script src="js/html5shiv.js"></script>
        <script src="js/respond.min.js"></script>
        <![endif]-->
    </head>
    <body class="signin">
        <section>
            <div class="panel panel-signin">
                <div class="panel-body">
                    <div class="logo text-center">
                        <img src="images/logo-primary.png" alt="Chain Logo" >
                    </div>
                    <br />
                    <h4 class="text-center mb5">Log In</h4>
                    <p class="text-center">Log in with your email address</p>
                    <div class="mb20"></div>
                    <div class="alert alert-danger" id="alert" runat="server" visible="false">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        <strong>Log In Error!</strong> The email address and password combination you entered is invalid
                     </div>
                    <form name="signin" id="signin" runat="server">
                        <div class="input-group mb15">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" name="txtEmail" />
                        </div><!-- input-group -->
                        <div class="input-group mb15">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Password" name="txtPassword" TextMode="Password" />
                        </div><!-- input-group -->
                        <div class="clearfix">
                            <div class="pull-left">
                                <div class="ckbox ckbox-primary mt10">
                                    <asp:Checkbox ID="chbxRemember" runat="server" />
                                    <label for="chbxRemember">Remember Me</label>
                                </div>
                            </div>
                            <div class="pull-right">
                                <asp:Button  ID="btnLogin" runat="server" OnClick="LogIn" CssClass="btn btn-success" Text="Sign In" />
                            </div>
                        </div>                      
                    </form>
                </div><!-- panel-body -->
                <div class="panel-footer">
                    <a href="mailto:info@latestsightings.com" class="btn btn-primary btn-block">Don't know your details? Email the admin</a>
                </div><!-- panel-footer -->
            </div><!-- panel -->
        </section>

        <script src="js/jquery-1.11.1.min.js"></script>
        <script src="js/jquery-migrate-1.2.1.min.js"></script>
        <script src="js/jquery.validate.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/modernizr.min.js"></script>
        <script src="js/retina.min.js"></script>
        <script src="js/jquery.cookies.js"></script>
        <script src="js/custom.js"></script>
        <script src="js/additional-methods.js"></script>
        <script src="js/jquery.tooltipster.js"></script>

        <script>
            $(document).ready(function () {
                $('#signin input[type="text"], #signin input[type="password"]').tooltipster({
                    trigger: 'custom',
                    onlyOne: false,
                    position: 'right'
                });
            });

            $(document).ready(function () {
                $("#signin").validate({
                    focusInvalid: true,
                    rules: {
                        txtPassword: {
                            required: true
                        },
                        txtEmail: {
                            email: true,
                            required: true
                        },
                    },
                    messages: {
                        txtEmail:
                        {
                            required: "Please enter your email address",
                            email: "Please enter a valid email address"
                        },
                        txtPassword:
                        {
                            required: "Please enter your password",
                        },
                    },
                    errorPlacement: function (error, element) {
                        $(element).tooltipster('update', $(error).text());
                        $(element).tooltipster('show');
                    },
                    success: function (label, element) {
                        $(element).tooltipster('hide');
                    }
                });
            });

        </script>
    </body>
</html>