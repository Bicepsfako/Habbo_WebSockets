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
<html>
<head>
    <meta charset="utf-8"/>
    <link href="./web-gallery/css/reset.css" rel="stylesheet"/>
    <link href="./web-gallery/css/client.css" rel="stylesheet"/>
    <script>
        var _0x463b = ["\x75\x73\x65\x72\x5F\x69\x64"];
        window[_0x463b[0]] =<?php echo $user['id']; ?>;
    </script>
</head>
<body>
<div class="gamebox">
    <div class="mask hidden"></div>
    <div class="charstats">
        <span class="healthpoints"></span>
        <progress id="healthpoints" max="100" value="0"></progress>
    </div>
    <div class="onlineusers-box" id="onlineusers-box">teste</div>
    <ul class="options">
        <li><span class="zoom" id="zoom"></span></li>
        <li><span class="online_users" id="online_users"></span></li>
        <li><a href="logout.php">Sair</a></li>
    </ul>
    <div class="mapbox" id="mapbox"></div>
    <div class="bottom_bar">
        <ul class="bottom_bar_content">
            <li><span class="catalog" id="catalog" title="Catálogo"></span></li>
            <li><span class="inventory" id="inventory" title="Inventário"></span></li>
            <!--<li><span class="chatlog" id="chatlog" title="Registro de Conversa"></span></li>-->
            <li><input type="text" id="charSpeak" class="charspeak"/></li>
        </ul>
    </div>

    <ul class="chatlog-box hidden" id="chatlog-box"></ul>
    <ul class="inventory-box hidden" id="inventory-box"></ul>
    <ul class="object-box hidden" id="object-box"></ul>
    <div class="catalog-box" id="catalog-box">
        <div class="top">Catálogo <span class="close"></span></div>
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
</div>

<script src="https://code.jquery.com/jquery-3.2.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<script src="web-galley/js/jquery.preload.min.js"></script>
<script src="./web-gallery/js/client.js"></script>
<script src="./web-gallery/js/ajax.js"></script>
</body>
</html>