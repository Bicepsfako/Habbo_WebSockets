<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>NDC-Pixels</title>
    <link href="https://fonts.googleapis.com/css?family=Ubuntu" rel="stylesheet"/>
    <link href="web-galley/css/reset.css" rel="stylesheet"/>
    <link href="web-galley/css/global.css" rel="stylesheet"/>
    <link href="web-galley/css/index.css" rel="stylesheet"/>
</head>
<body>
<div class="background-right"></div>
<div class="background-left"></div>

<div id="login" class="window" style="position:absolute;left:calc(50% - 150px);top:30%;z-index:0">
    <div class="header">Login</div>
    <div class="body">
        <div class="alert" style="display:none" id="login_alert">teste</div>
        <div class="form-group">
            <label for="login_username">Usuário ou E-mail:</label>
            <input type="text" id="login_username" class="form-control" autocomplete="false" autofocus />
        </div>
        <div class="form-group">
            <label for="login_password">Senha:</label>
            <input type="password" id="login_password" class="form-control" autocomplete="false" />
        </div>
        <div class="form-group">
            <input type="checkbox" id="login_remember" />
            <label for="remember">Lembre-se de mim</label>
        </div>
        <button class="button button-blue" id="registerBtn">Cadastre-se</button>
        <button class="button button-green" style="float:right;position:absolute;right:33px" id="login_verify">Entrar</button>
    </div>
    <div class="footer"></div>
</div>

<div id="register" class="window" style="position:absolute;left:calc(50% - 150px);top:30%;display:none;z-index:1">
    <div class="header">Cadastre-se <span class="close" id="closeRegister"></span></div>
    <div class="body">
        <div class="alert" style="display:none" id="reg_alert">teste</div>
        <div class="form-group">
            <label for="reg_username">Usuário:</label>
            <input type="text" id="reg_username" class="form-control" autocomplete="false" autofocus />
        </div>
        <div class="form-group">
            <label for="reg_email">E-mail:</label>
            <input type="email" id="reg_email" class="form-control" autocomplete="false" />
        </div>
        <div class="form-group">
            <label for="reg_password">Senha:</label>
            <input type="password" id="reg_password" class="form-control" autocomplete="false" />
        </div>
        <div class="form-group">
            <label for="reg_password2">Verifique a Senha:</label>
        <input type="password" id="reg_password2" class="form-control" autocomplete="false" />
    </div>
        <button class="button button-green" style="width:calc(100% - 23px)" id="regComplete">Completar Cadastro</button>
    </div>
    <div class="footer"></div>
</div>

<script src="https://code.jquery.com/jquery-3.2.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script src="web-galley/js/index.js"></script>
</body>
</html>