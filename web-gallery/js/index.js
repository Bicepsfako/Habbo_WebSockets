/**
 * Created by xSmoking on 4/20/2017.
 */
$(document).ready(function ()
{
    $("#register").draggable({handle: '.header'});
    $("#login").draggable({handle: '.header'});

    $("#login").mousedown(function ()
    {
        if ($("#register").is(":visible"))
            $("#register").css("z-index", "0");

        $("#login").css("z-index", "1");
    });

    $("#register").mousedown(function ()
    {
        $("#register").css("z-index", "1");
        $("#login").css("z-index", "0");
    });

    $("#closeRegister").click(function ()
    {
        $("#register").css("display", "none");
    });

    $("#registerBtn").click(function ()
    {
        $("#register").css("display", "block");
    });

    // Login
    $('#login_verify').click(function ()
    {
        LoginAjax();
    });

    function LoginAjax()
    {
        $("#login_alert").hide();
        $.ajax({
            url: 'controller/AjaxController.php',
            type: 'POST',
            dataType: 'JSON',
            data: {
                action: 'LoginUser',
                username: $('#login_username').val(),
                password: $('#login_password').val(),
                remindme: $('#login_remember').is(':checked')
            },
            success: function (e)
            {
                $("#login_alert").html(e.message);
                if (e.type === true)
                {
                    $("#login_alert").addClass("alert-success");
                    window.location.href = "./client";
                }
                else
                {
                    $("#login_alert").addClass("alert-error");
                    $("#login_alert").show();
                }
            }
        });
    }

    // Register
    $('#regComplete').click(function ()
    {
        RegisterAjax();
    });

    function RegisterAjax()
    {
        $("#reg_alert").hide();
        $.ajax({
            url: 'controller/AjaxController.php',
            type: 'POST',
            dataType: 'JSON',
            data: {
                action: 'RegisterUser',
                username: $('#reg_username').val(),
                email: $('#reg_email').val(),
                password: $('#reg_password').val(),
                password_2: $('#reg_password2').val()
            },
            success: function (e)
            {
                $("#reg_alert").html(e.message);
                if (e.type === true)
                {
                    $("#reg_alert").addClass("alert-success");
                    window.location.href = "./client";
                }
                else
                {
                    $("#reg_alert").addClass("alert-error");
                    $("#reg_alert").show();
                }
            }
        });
    }
});