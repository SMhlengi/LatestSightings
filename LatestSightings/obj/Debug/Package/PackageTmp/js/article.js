$(document).ready(function () {
    checkIfCredentialsAreSetToBeRemembered();
    $("#category").select2();

    function checkIfCredentialsAreSetToBeRemembered() {
        if ($.cookie("username") && $.cookie("password")) {
            $(".username").val($.cookie("username"));
            $(".password").val($.cookie("password"));
            $("#rememberme").attr("checked", "checked");
        }
    }

    $(".login").click(function () {
        $("#username").attr("class", "form-group");
        $("#password").attr("class", "form-group");
        $(".inlineoveride").css("display", "inline");
        $(".message").css("color", "#000000");
        $(".message").html("Validating Credentials... Please wait");
        setTimeout(function () { ValidateForm() }, 1000);
    });

    function ValidateForm() {
        if (CheckIfCredentialsEntered()) {
            ValidateCredentials();
        } else {
            DetermineWhatHasNotBeenEntered();
        }
    }

    function CheckIfCredentialsEntered() {
        if ($(".username").val().length > 0 && $(".password").val().length > 0) {
            return true;
        } else {
            return false;
        }
    }

    function DetermineWhatHasNotBeenEntered() {
        DisplayMessageWhereNothingHasBeenEntered();
        DisplayMessageWhereOnlyUsernameEntered();
        DisplayMessageWhereOnlyPasswordEntered();
    }

    function DisplayMessageWhereNothingHasBeenEntered() {
        if ($(".username").val().length == 0 && ($(".password").val().length == 0)) {
            $("#username").attr("class", "form-group has-error");
            $("#password").attr("class", "form-group has-error");
            $(".message").css("color", "#CD0000");
            $(".message").html("Please enter your Username and Password");
            HideSpinner();
        }
    }

    function DisplayMessageWhereOnlyUsernameEntered() {
        if ($(".username").val().length > 0 && $(".password").val().length == 0) {
            $("#password").attr("class", "form-group has-error");
            $(".message").css("color", "#CD0000");
            $(".message").html("Please enter your Password");
            HideSpinner();
        }
    }

    function DisplayMessageWhereOnlyPasswordEntered() {
        if ($(".username").val().length == 0 && $(".password").val().length > 0) {
            $("#username").attr("class", "form-group has-error");
            $(".message").css("color", "#CD0000");
            $(".message").html("Please enter your Username");
            HideSpinner();
        }
    }

    function HideSpinner() {
        $(".inlineoveride").css("display", "none");
    }

    function ValidateCredentials(postUrl) {
        var postUrl = "/AjaxFunction.aspx/ValidateCredentials";
        $.ajax({
            type: "POST",
            url: postUrl,
            data: "{'username' : '" + $(".username").val() + "', 'password' : '" + $(".password").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(
            function (data, textStatus, jqXHR) {
                if (IsValid(data)) {
                }
            }
        ).fail(
            function (data, textStatus, jqXHR) {
            }
        );
    }

    function IsValid(data) {
        if (data.d.status == "invalid") {
            ProcessInvalidCredentials();
        } else if (data.d.status = "valid") {
            ProcessValidCredentials(data.d.username);
        }

    }

    function ProcessInvalidCredentials() {
        $("#username").attr("class", "form-group has-error");
        $("#password").attr("class", "form-group has-error");
        $(".message").css("color", "#CD0000");
        $(".message").html("Invalid Username / Password");
        HideSpinner();
    }

    function ProcessValidCredentials(username) {
        $(".message").css("color", "#008B45");
        $(".message").html("Valid Credentials...Loging in");
        $.cookie("logged_in_user", username);
        checkifRememberMeWasSet();

        setTimeout(function () { RedirectToUrl("/articles") }, 1500);
    }

    function RedirectToUrl(url) {
        location.href = url;
    }

    function checkifRememberMeWasSet() {
        if ($("#rememberme").prop('checked')) {
            $.cookie("username", $(".username").val());
            $.cookie("password", $(".password").val());
        }
    }

    $(".logout").click(function () {
        SetloggedInStatusSessionToFalse();
    });

    function SetloggedInStatusSessionToFalse() {
        var postUrl = "/AjaxFunction.aspx/logout";
        $.ajax({
            type: "POST",
            url: postUrl,
            data: "{'sessionName' : 'loggedInStatus'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(
            function (data, textStatus, jqXHR) {
                if (data.d == true) {
                    location.href = "/";
                } else {
                    location.href = "/";
                }

            }
        ).fail(
            function (data, textStatus, jqXHR) {
            }
        );
    }

    $(".addNewArticle").click(function () {
        location.href = "/articles/addnewarticle";
    });

    $(".addnewarticle").click(function () {
        location.href = "/articles/addnewarticle";
    });

    $("#createCategory").click(function () {
        $("#noCategoryAlertHeading").css("font-size", "17px");
        $("#noCategoryAlertHeading").html("Creating New Category.....");
        $("#noCategoryAlertBody").hide()
        $("#categoryNameChoosen").show("slow");
        $(this).attr("class", "btn btn-success");
        $(this).html("Save Category");
        if ($("#categoryNameChoosen").val()) {
            var category = $("#categoryNameChoosen").val();
            var postUrl = "/AjaxFunction.aspx/SaveCategory";
            $.ajax({
                type: "POST",
                url: postUrl,
                data: "{'category' : '" + category + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(
                function (data, textStatus, jqXHR) {
                    if (data.d.categoryname != "") {
                        $("#newlyCreatedCategory").html(data.d.categoryname);
                        categoryid = data.d.categoryid;
                        articleId = data.d.categoryid;
                        $(".nocategory").hide("slow");
                        $(".newcategory").show("slow");
                    }
                }
            ).fail(
                function (data, textStatus, jqXHR) {
                }
            );
        }
    });


    $(".articleBoardCategory").change(function () {
        var categoryid = $("#category").val();
        location.href = "/articles/category/" + categoryid;
    });

    $(".saveCategory").click(function () {
        if ($(".newcategorycaptured").val()) {
            $(".categoryExists").hide();
            $(".form-group").attr("class", "form-group");
            var category = $(".newcategorycaptured").val();
            $(".savingcategory").show();
            var postUrl = "/AjaxFunction.aspx/SaveCategory";
            $.ajax({
                type: "POST",
                url: postUrl,
                data: "{'category' : '" + category + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(
                function (data, textStatus, jqXHR) {
                    if (data.d.categoryExists != "1") {
                        $(".savingcategory").hide();
                        $(".categorysaved").show();
                        setTimeout(function () { location.href = "/articles/addcategory"; }, 2500);
                    } else {
                        $(".form-group").attr("class", "form-group has-error");
                        $(".savingcategory").hide();
                        $(".categoryExists").show();
                    }
                }
            ).fail(
                function (data, textStatus, jqXHR) {
                }
            );
        }
    });

    $(".delete").click(function () {
        var articleId = $(this).attr("id");
        var articleHeading = $(".header" + articleId).html();
        $(".articleContent").html(articleHeading);
        $(".proceedWithArticleDelete").attr("id", articleId);
        $(".deleteArticle").show();
    });

    $(".cancellArticleDelete").click(function () {
        $(".deleteArticle").hide();
    });

    $(".proceedWithArticleDelete").click(function () {
        var articleId = $(".proceedWithArticleDelete").attr("id");
        $(".deleteArticle").hide();
        $(".onArticleDeletingMessage").show();
        setTimeout(function () { DeleteArticle(articleId) }, 3000);
    });

    function DeleteArticle(id) {

        var postUrl = "/AjaxFunction.aspx/DeleteArticle";
        $.ajax({
            type: "POST",
            url: postUrl,
            data: "{'articleId' : '" + id + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(
            function (data, textStatus, jqXHR) {
                if (data.d.successfull == "1") {
                    $(".onArticleDeletingMessage").hide();
                    $(".onArticleDeletingSuccessfully").show();
                    setTimeout(function () { RedirectToUrl(location.href) }, 3000);
                } else {

                }
            }
        ).fail(
            function (data, textStatus, jqXHR) {
            }
        );
    }

    $(".editCategory").click(function () {
        $(".editPencil").hide();
        $(".deleteCategoryrow").hide();
        $(".UnassignedCategoryExists").hide();
        var categoryId = $(this).attr("id");
        var categoryName = $("#categoryNameId" + categoryId).html();
        $(".newCategoryrow").hide();
        $(".currentcategorycaptured").val(categoryName);
        $(".UpdatedCategory").attr("id", categoryId);
        $(".editCategoryrow").show();
        $(".editPencil" + categoryId).show();
    });

    $(".CancellUpdatedCategory").click(function () {
        $(".editPencil").hide();
        $(".editCategoryrow").hide();
        $(".newCategoryrow").show();
    });

    $(".UpdatedCategory").click(function () {
        if ($(".currentcategorycaptured").val()) {
            $(".form-group").attr("class", "form-group");
            $(".categoryExists").hide();
            var id = $(this).attr("id");
            var updateCategoyName = $(".currentcategorycaptured").val();
            $(".updatingcategory").show();
            var postUrl = "/AjaxFunction.aspx/UpdateCategory";
            $.ajax({
                type: "POST",
                url: postUrl,
                data: "{'category' : '" + updateCategoyName + "', 'id' : '" + id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(
                function (data, textStatus, jqXHR) {
                    if (data.d.categoryExists != "1") {
                        $(".updatingcategory").hide();
                        $(".categoryupdated").show();
                        setTimeout(function () { location.href = "/articles/addcategory"; }, 2500);
                    } else {
                        $(".form-group").attr("class", "form-group has-error");
                        $(".updatingcategory").hide();
                        $(".categoryExists").show();
                    }
                }
            ).fail(
                function (data, textStatus, jqXHR) {
                }
            );
        }
    });

    $(".deleteCategory").click(function () {
        $(".editPencil").hide();
        var categoryId = $(this).attr("id");
        var categoryName = $("#categoryNameId" + categoryId).html();
        $(".deleteCreatedCategory").html(categoryName);
        $(".proceedWithCategoryDelete").attr("id", categoryId);
        $(".newCategoryrow").hide();
        $(".editCategoryrow").hide();
        $(".deleteCategoryrow").show();
        $(".editPencil" + categoryId).show();
    });

    $(".cancelCategoryDelete").click(function () {
        $(".editPencil").hide();
        $(".deleteCategoryrow").hide();
        $(".newCategoryrow").show();
    });

    $(".proceedWithCategoryDelete").click(function () {
        var categoryId = $(this).attr("id");
        $(".deleteCategoryRowAlert").hide();
        $(".deletinggcategory").show();
        var postUrl = "/AjaxFunction.aspx/DeleteCategory";
        $.ajax({
            type: "POST",
            url: postUrl,
            data: "{'id' : '" + categoryId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(
            function (data, textStatus, jqXHR) {
                if (data.d.articlesExists == "0" && data.d.categoryDeleted == "1") {
                    $(".deletinggcategory").hide();
                    $(".categoryDeleted").show();
                    setTimeout(function () { location.href = "/articles/addcategory"; }, 2500);
                } else if (data.d.UnassignedCategoryExists == "1" && data.d.categoryDeleted == "1") {
                    $(".deletinggcategory").hide();
                    $(".categoryDeleted").show();
                    setTimeout(function () { location.href = "/articles/addcategory"; }, 2500);
                } else if (data.d.articlesExists == "1" && data.d.UnassignedCategoryExists == "0") {
                    $(".updatingcategory").hide();
                    $(".deletinggcategory").hide();
                    $(".UnassignedCategoryExists").show();
                }
            }
        ).fail(
            function (data, textStatus, jqXHR) {
            }
        );
    });

    $(".createUnassignedCategory").click(function () {
        $(".UnassignedCategoryExists").hide();
        $(".savingcategory").show();
        var postUrl = "/AjaxFunction.aspx/CreateUnassignedCategory";
        $.ajax({
            type: "POST",
            url: postUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(
            function (data, textStatus, jqXHR) {
                $(".savingcategory").hide();
                $(".categorysaved").show();
                setTimeout(function () { location.href = "/articles/addcategory"; }, 2500);
            }
        ).fail(
            function (data, textStatus, jqXHR) {
            }
        );
    });

    $(".editArticle").click(function () {
        var articleId = $(this).attr("id");
        location.href = "/articles/edit/" + articleId;
    });

    $(".changeImageOne").click(function () {
        $(".viewImage").hide();
        $(".changeImageOne").hide();
        $(".editFileUpload1").show();
        $(".cancelFileUpload1").show();
    });

    $(".cancelFileUpload1").click(function () {
        $(".viewImage").show();
        $(".changeImageOne").show();
        $(".editFileUpload1").hide();
        $(".cancelFileUpload1").hide();
    });

    $(".cancelUpdate").click(function () {
        location.href = "/articles";
    });

});