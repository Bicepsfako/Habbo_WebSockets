<?php
/**
 * Created by IntelliJ IDEA.
 * User: xSmoking
 * Date: 4/20/2017
 * Time: 10:36 PM
 */

header('Cache-Control: no-cache');
header('Pragma: no-cache');
session_start();
require_once 'controller/Connection.php';

if (!isset($_SESSION['username']))
{
    header('Location: index');
}

$userQuery = $mysqli->query("SELECT * FROM users WHERE username = '" . $_SESSION['username'] . "'") or die($mysqli->error);
$user = $userQuery->fetch_assoc();
?>
<!DOCTYPE html>
<html lang="pt">
<head>
    <meta charset="utf-8"/>
    <title>Habbo WebSocket</title>
    <link href="./web-gallery/css/reset.css" rel="stylesheet"/>
    <link href="./web-gallery/css/global.css" rel="stylesheet"/>
    <link href="./web-gallery/css/client.css" rel="stylesheet"/>
    <script>
        var _0x463b = ["\x75\x73\x65\x72\x5F\x69\x64"];
        window[_0x463b[0]] = <?php echo $user['id']; ?>;
    </script>
</head>
<body style="background:url(./web-gallery/images/backgrounds/habbo15/habbo15_background_gradient.png)">
<div class="gamebox">
    <div class="background-right"></div>
    <div class="background-left"></div>
    <div class="mask hidden"></div>
    <!--<div class="alert-window" id="alert-box">
        <div class="header">Alerta <span class="close" style="right:12px"></span></div>
        <div class="body">
            teste
        </div>
        <div class="footer"></div>
    </div>-->
    <div class="up_right_options">
        <span class="help" id="options_help">Ajuda</span>
        <span class="quit" id="options_quit"></span>
        <span class="settings" id="options_settings"></span>t
    </div>
    <div class="bottom_bar">
        <span class="navigator" id="navigator" title="Navegador"></span>
        <span class="catalog" id="catalog" title="Catálogo"></span>
        <span class="inventory" id="inventory" title="Inventário"></span>
        <div class="charspeech">
            <span class="speech_bubbles"></span>
            <input type="text" id="charSpeak" class="input_bubble" placeholder="Fale aqui..." />
        </div>
    </div>

    <ul class="inventory-box" id="inventory-box" style="display:none;"></ul>
    <ul class="object-box" id="object-box" style="display:none;"></ul>

    <!-- Catálogo -->
    <div class="catalog-window" id="catalog-box" style="display:none;">
        <div class="top">Loja <span class="close" style="right:12px"></span></div>
        <div class="body">
            <div class="left-side">
                <input class="b_search" type="text" id="search" placeholder="Procure aqui" autocomplete="off"/>
                <div class="b_top"></div>
                <div class="b_body">

                </div>
                <div class="b_bottom"></div>
            </div>
            <div class="right-side">

            </div>
        </div>
        <div class="bottom"></div>
    </div>

    <!-- Navegador -->
    <div class="navigator-window" id="navigator-box" style="display:none;">
        <div class="header">Navegador  <span class="info" style="right:36px"></span><span class="close" style="right:12px"></span></div>
        <ul class="tabs" id="navigator-tabs">
            <li id="navigator-public" class="active">Público</li>
            <li id="navigator-all">Todos os quartos</li>
            <li id="navigator-events">Eventos</li>
            <li id="navigator-mine">Meu mundo</li>
        </ul>
        <div class="body">
            <div class="filter">
                <select id="look_for">
                    <option value="anything" selected>Qualquer coisa</option>
                    <option value="room_name">Nome do quarto</option>
                    <option value="owner">Dono</option>
                    <option value="tag">Etiqueta</option>
                    <option value="group">Grupo</option>
                </select>
                <label>
                    <input type="text" id="search" placeholder="filtrar quartos por" autocomplete="false" />
                    <span class="pencil"></span>
                </label>
                <span class="update"></span>
            </div>
            <ul class="rooms" id="navigator-rooms"></ul>
        </div>
        <div class="footer"></div>
    </div>
</div>

<script src="https://code.jquery.com/jquery-3.2.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script src="web-gallery/js/jquery.cookie.js"></script>
<script src="web-gallery/js/client.js"></script>
</body>
</html>