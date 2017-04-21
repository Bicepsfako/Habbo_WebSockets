var Mouse, MouseMode, Map = [], MousedMap = [], Offset = [], AddTileArea = function (a, c, b) {
    var e = document.createElement("area");
    $(e).attr("shape", "poly").attr("Coords", a).appendTo($("map")).bind("mouseenter", function () {
        TileOver(c, b)
    }).bind("mouseleave", function () {
        TileOut(c, b)
    }).bind("click", function () {
        TileClick(c, b)
    })
}, TileOver = function (a, c) {
    if (Mouse)
        MousedMap[a][c] || Map[a][c] != MouseMode || (MousedMap[a][c] = !0, Map[a][c] = !MouseMode, ReDrawMap());
    else {
        var b = "#tile" + a + "-" + c;
        Map[a][c] ? $(b).attr("src", "http://peace-tool.tk/assets/image/del.png") :
                $(b).attr("src", "http://peace-tool.tk/assets/image/add.png")
    }
}, TileOut = function (a, c) {
    if (!Mouse) {
        var b = "#tile" + a + "-" + c;
        Map[a][c] ? $(b).attr("src", "http://peace-tool.tk/assets/image/on.png") : $(b).attr("src", "http://peace-tool.tk/assets/image/off.png")
    }
}, TileClick = function (a, c) {
    $("#toggle").is(":checked") ? Mouse ? (Mouse = !1, ResetMousedMap()) : (Mouse = !0, MouseMode = Map[a][c], Map[a][c] = !Map[a][c], ReDrawMap()) : (Map[a][c] = !Map[a][c], ReDrawMap())
}, ResetMousedMap = function () {
    for (var a = Map.length, c = Map[0].length, b = 0; b < a; b++)
        for (var e = 0; e < c; e++)
            MousedMap[b][e] = !1
}, ResetMap =
        function () {
            Map = [];
            $("#preview").empty()
        }, PrepareBlankMap = function (a, c) {
    if (isNaN(a) || isNaN(c))
        return alert("Invalid size !"), !1;
    Mouse = !1;
    Map = [];
    MousedMap = [];
    Offset.x = 32 * c - 7;
    Offset.y = 25;
    for (var b = 0; b < a; b++) {
        Map[b] = [];
        for (var e = 0; e < c; e++) {
            Map[b][e] = !0;
            MousedMap[b] = [];
            var f = 32 * b + -32 * e + Offset.x, g = 16 * b + 16 * e + Offset.y, d;
            d = f + 31;
            d += ",";
            d += g;
            d += ",";
            d += f + 63;
            d += ",";
            d += g + 16;
            d += ",";
            d += f + 32;
            d += ",";
            d += g + 31;
            d += ",";
            d += f + 31;
            d += ",";
            d += g + 31;
            d += ",";
            AddTileArea(d, b, e)
        }
    }
    drawMap();
    ResetMousedMap()
}, drawMap = function () {
    for (var a =
            Map.length, c = Map[0].length, b = 0; b < a; b++)
        for (var e = 0; e < c; e++) {
            var f = 32 * b + -32 * e + Offset.x, g = 16 * b + 16 * e + Offset.y;
            Map[b][e] ? $("#preview").append('<img id="tile' + b + "-" + e + '" class="square" style="top: ' + g + "px; left: " + f + 'px;" src="http://peace-tool.tk/assets/image/on.png" alt="Click To Remove" />') : $("#preview").append('<img id="tile' + b + "-" + e + '" class="square" style="top: ' + g + "px; left: " + f + 'px;" src="http://peace-tool.tk/assets/image/off.png" alt="Click To Remove" />');
            f = 32 * a + 32 * c + 50;
            g = 16 * a + 16 * c + 58;
            $("#preview").css("width", f + "px");
            $("#preview").css("height",
                    g + "px");
            $("#mapimg").css("width", f + "px");
            $("#mapimg").css("height", g + "px")
        }
}, ReDrawMap = function () {
    for (var a = Map.length, c = Map[0].length, b = 0; b < a; b++)
        for (var e = 0; e < c; e++) {
            var f = "#tile" + b + "-" + e;
            Map[b][e] ? $(f).attr("src", "http://peace-tool.tk/assets/image/on.png") : $(f).attr("src", "http://peace-tool.tk/assets/image/off.png");
        }
    RefreshExport()
}, RefreshExport = function () {
    for (var a = "", c = Map.length, b = Map[0].length, e = 0; e < b; e++) {
        for (var f = 0; f < c; f++)
            a = Map[f][e] ? a + "0" : a + "X";
        a += "\n"
    }
    $("#export").val(a)
};
$(document).ready(function () {
    PrepareBlankMap(5, 5)
});
$("#createMap").submit(function () {
    var a = $(this).serializeArray(), c = a[0].value, a = a[1].value;
    ResetMap();
    PrepareBlankMap(c, a);
    return!1
});