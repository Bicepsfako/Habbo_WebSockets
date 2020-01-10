$(document).ready(function () {
    // MAKE WINDOWS CLOSABLE
    $(document).on('click', '.close', function () {
        var box = $(this).parent().parent();
        if (box.attr('id') === 'alert-box')
            box.remove();
        else
            box.hide();
    });

    $(".scroll").click(function (event) {
        event.preventDefault();
        $('html,body').animate({scrollTop: $(this.hash).offset().top}, 800);
    });

    var selfId;
    var items = {};
    var speak_bubbles = [];

    function preloadChars() {
        for (var i = 0; i < 7; i++) {
            new Image().src = './web-gallery/images/habbo/' + i + '_0.png';
            new Image().src = './web-gallery/images/habbo/' + i + '_1.png';
            new Image().src = './web-gallery/images/habbo/' + i + '_2.png';
            new Image().src = './web-gallery/images/habbo/' + i + '_3.png';
        }
    }

    preloadChars();

    /* START A WEB SOCKET CONNECTION */
    var websocket = new WebSocket('ws://192.168.15.24:81/');
    websocket.binaryType = "arraybuffer";
    websocket.onopen = function (evt) {
        //websocket.send('{"command":"connection","id":' + user_id + '}');
        websocket.send('connect|' + user_id);
        //mapGenerator();
    };

    websocket.onclose = function (evt) {
        $('body').prepend('<div class="disconnected"><span class="title">Desconectado</span><p>Algum' +
            ' erro ocorreu e você foi desconectado do servidor.</p><p>Reentre ou tente novamente mais tarde.</p><p><a' +
            ' href="client">Clique' +
            ' aqui para' +
            ' entrar' +
            ' novamente</a></p></div>');
        $('.mask').show();
        //window.location.href = "./disconnected";
    };

    websocket.onmessage = function (evt) {
        var evento = evt.data.split('|');
        console.log('evento: ' + evt.data);
        switch (evento[0]) {
            case 'userChat':
                charSpeak(evento[1], evento[2], evento[3]);
                break;
            case 'newPosition':
                positionUpdate(evento[1], evento[2], evento[3]);
                break;
            case 'newConnection':
                loadUser(evento[1], evento[2], evento[3], evento[4]);
                break;
            case 'newDisconnection':
                $('.charh[data-cid="' + evento[1] + '"]').remove();
                break;
            case 'selfId':
                selfId = evento[1];
                $('#mapbox').prepend('<span class="charh" data-cid="' + evento[1] + '" style="background:url(./web-gallery/images/habbo/7_0.png)no-repeat;z-index:25;left:660px;top:88px;"></span>');
                break;
            case 'loadUsers':
                loadUser(evento[1], evento[2], evento[3], evento[4]);
                break;
            case 'updateStats':
                var max_health = parseInt(evento[1]);
                var actual_health = parseInt(evento[2]);
                $('#healthpoints').attr('value', actual_health).attr('max', max_health);
                break;
            case 'fromto':
                break;
            case 'load_all_rooms':
                loadNavigator(evento[1]);
                break;
            default:
                //websocket.close();
                break;
        }
    };

    websocket.onerror = function (evt) {
        $('body').prepend('<div class="disconnected"><span class="title">Desconectado</span><p>Algum' +
            ' erro ocorreu e você foi desconectado do servidor.</p><p>Reentre ou tente novamente mais tarde.</p><p><a' +
            ' href="client">Clique' +
            ' aqui para' +
            ' entrar' +
            ' novamente</a></p></div>');
        $('.mask').show();
    };

    window.onbeforeunload = function (e) {
        websocket.send('disconnect|' + user_id);
        websocket.close();
    };
    /* END THE WEB SOCKET CONNECTION */

    /* TERMINO ENVIO DO ID */

    /* ADICIONA FALA AO PERSONAGEM */
    function charSpeak(speech, username, charid) {
        var charSelect = $('.charh[data-cid="' + charid + '"]');
        var charPosTop = parseInt(charSelect.position().top);
        var PosTop = charPosTop - 50 + 'px';
        var CharPosLeft = parseInt(charSelect.position().left + 20);
        bumpBubble();
        for (var i = 0; i <= 100; i++) {
            if ($.inArray('b' + i, speak_bubbles) == "-1") {
                speak_bubbles.push("b" + i);
                $('#mapbox').prepend('<div class="speak_bubble" data-bid="b' + i + '" style="top:' + PosTop + '"><span class="user">' + username + ':</span> ' + speech + '</div>');
                var bubbleW = parseInt($('.speak_bubble[data-bid="b' + i + '"]').width());
                var PosLeft = parseInt(CharPosLeft - (bubbleW / 2) + 'px');
                $('.speak_bubble[data-bid="b' + i + '"]').css({"left": PosLeft});
                return false;
            }
        }
        $('#chatlog-box').append('<li><span class="user">' + username + ':</span> ' + speech + '</li>');
    }

    setInterval(function () {
        bumpBubble();
    }, 2000);

    function bumpBubble() {
        $('.speak_bubble').each(function () {
            $(this).css('top', $(this).position().top - 30);
            if ($(this).position().top <= -50) {
                $(this).remove();
                var itemToRemove = $(this).data('bid');
                speak_bubbles.splice($.inArray(itemToRemove, speak_bubbles), 1);
            }
        });
    }

    $('#charSpeak').keyup(function (e) {
        var regex = new RegExp('<');
        $(this).val($(this).val().replace(regex, ''));
    });
    /* FIM DA FALA DO PERSONAGEM */

    /* ATUALIZA A POSIÇÃO DO USUÁRIO RECEBIDA DO SERVIDOR */
    function positionUpdate(position, direction, char) {
        setTimeout(function () {
            var positionSelector = $('li[data-pos="' + position + '"]');
            var charSelector = $('.charh[data-cid="' + char + '"]');
            var posLeft = parseInt(positionSelector.css("left"));
            var posLeft = posLeft + 10 + 'px';
            var posTop = parseInt(positionSelector.css("top"));
            var posTop = posTop - 62 + 'px';
            var zindex = positionSelector.css('z-index');
            charSelector.css('z-index', zindex + 10);
            charSelector.css('background-repeat', 'no-repeat');
            charSelector.css('opacity', '1');

            charSelector.animate({
                top: posTop,
                left: posLeft
            }, 400, function () {
                charAnimate(char, direction);
                if (char === selfId) {
                    var posSplit = position.split('_');
                    websocket.send('onTileUpdate|' + posSplit[0] + '|' + posSplit[1]);
                }
                if ($.inArray(position, items) !== -1) {
                    for (var i = 0; i <= items.length; i++) {
                        if (items[i] === position) {
                            $('.item[data-item="' + i + '"').remove();
                            $('.grass_itened[data-pos="' + position + '"]').removeClass('grass_itened').addClass('grass1');
                            if (char === selfId) {
                                for (var z = 0; z <= '29'; z++) {
                                    if ($('#inventory-box li[data-slotid="' + z + '"]').is(':empty')) {
                                        $('#inventory-box li[data-slotid="' + z + '"]').append('<img src="web-gallery/images/itens/' + i + '_1.png" alt="" />');
                                        user_items.push(i);
                                        items.splice($.inArray(position, items), 1);
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }, 0);
    }

    function charAnimate(char, direction) {
        setTimeout(function () {
            $('.charh[data-cid="' + char + '"]').css('background-image', ('url(./web-gallery/images/habbo/' + direction + '_1.png'));
        }, 100);
        setTimeout(function () {
            $('.charh[data-cid="' + char + '"]').css('background-image', ('url(./web-gallery/images/habbo/' + direction + '_2.png)'));
        }, 200);
        setTimeout(function () {
            $('.charh[data-cid="' + char + '"]').css('background-image', ('url(./web-gallery/images/habbo/' + direction + '_3.png)'));
        }, 300);
        setTimeout(function () {
            $('.charh[data-cid="' + char + '"]').css('background-image', ('url(./web-gallery/images/habbo/' + direction + '_2.png)'));
        }, 400);
        setTimeout(function () {
            $('.charh[data-cid="' + char + '"]').css('background-image', ('url(./web-gallery/images/habbo/' + direction + '_1.png)'));
        }, 500);
        setTimeout(function () {
            $('.charh[data-cid="' + char + '"]').css('background-image', ('url(./web-gallery/images/habbo/' + direction + '_0.png)'));
        }, 600);
    }

    function loadUser(position, direction, char) {
        setTimeout(function () {
            var positionSelector = $('li[data-pos="' + position + '"]');
            var posLeft = parseInt(positionSelector.css("left"));
            var posLeft = posLeft + 10 + 'px';
            var posTop = parseInt(positionSelector.css("top"));
            var posTop = posTop - 62 + 'px';
            var zindex = positionSelector.css('z-index');
            $('.charh[data-cid="' + char + '"]').css('opacity', '1');
            $('#mapbox').prepend('<span class="charh" data-cid="' + char + '" style="background:url(./web-gallery/images/habbo/' + direction + '_0.png) no-repeat; left: ' + posLeft + '; top: ' + posTop + '; z-index:' + zindex + 10 + '"></span>');
        }, 300);
    }

    /* FIM ATUALIZAR POSIÇÃO RECEBIDA DO SERVIDOR */

    /* COLOCA ITENS NO PISO */
    function itemSpawner(item, position) {
        var posLeftInt = parseInt($('li[data-pos="' + position + '"]').css("left"));
        var posLeft = posLeftInt + 12 + 'px';
        var posTopInt = parseInt($('li[data-pos="' + position + '"]').css("top"));
        var posTop = posTopInt - 15 + 'px';
        var zindex = position.split('_');
        $('.grass1[data-pos="' + position + '"').addClass('grass_itened');
        $('.grass1[data-pos="' + position + '"').removeClass('grass1');
        $('.grass2[data-pos="' + position + '"').addClass('grass_itened');
        $('.grass2[data-pos="' + position + '"').removeClass('grass2');
        $('#mapbox').prepend('<span class="item" data-item="' + item + '" style="background:url(./web-gallery/images/itens/' + item + '_1.png) no-repeat; left: ' + posLeft + '; top: ' + posTop + '; z-index:' + (zindex[0] + 1) * (zindex[1] + 3) + ';"></span>');
        setInterval(function () {
            $('.item[data-item="' + item + '"').animate({
                top: posTop
            }, 500);
            $('.item[data-item="' + item + '"').animate({
                top: posTopInt - 23 + 'px'
            }, 500);
        }, 1);
        items[item] = position;
    }

    /* FIM COLOCAR ITENS */

    /* USER STATUS */
    function changeUserStatus(status, user) {
        $.ajax({
            url: 'controller/userStatus.php',
            type: 'POST',
            dataType: 'JSON',
            data: {
                user: user,
                status: status
            }
        });
    }

    /* USER STATUS */

    /* ENVIA A FALA PARA O SERVIDOR */
    $('#charSpeak').keypress(function (e) {
        if (e.which == 13) {
            var charSpeak = $(this).val();
            websocket.send('speak|' + charSpeak + '|' + selfId);
            $(this).val('');
        }
    });
    /* FIM DO ENVIO DA FALA AO SERVIDOR */

    /* ENVIA A NOVA POSIÇÃO (QUAL O USER CLICOU) PARA O SERVIDOR */
    $.get('map.txt', function (data) {
        $(document).on('click', '.clickable', function () {
            var newPosition = $(this).attr('data-pos').split('_');
            var map = data.split('\r\n');
            var mapWidth = map.length;
            var newPosX = newPosition['0'];
            var newPosY = newPosition['1'];
            for (var x = 1; x <= mapWidth; x++) {
                var colunas = map[x - 1].split(',');
            }
            var mapHeight = colunas.length;
            websocket.send('position|' + mapWidth + '|' + mapHeight + '|' + newPosX + '|' + newPosY + '|' + selfId);
        });
    });
    /* FIM ENVIAR NOVA POSIÇÃO PARA O SERVIDOR */

    /* GERA O MAPA NA CLIENTE RECEBENDO OS DADOS DO MAP.TXT */
    var mapGenerator = function () {
        $.get('map.txt', function (data) {
            var map = data.split('\r\n'); // Cria array pra Coluna x Linha
            var top_s = 150;
            var left_s = 650;
            var linhas = map.length;
            for (var x = 1; x <= linhas; x++) {
                var colunas = map[x - 1].split(',');
                $('#mapbox').append('<ul data-x="' + (x - 1) + '">');
                for (var y = 1; y <= colunas.length; y++) {
                    if (colunas[y - 1] === '1') {
                        $('#mapbox ul[data-x="' + (x - 1) + '"').append('<li class="tile" data-pos="' + (x - 1) + '_' + (y - 1) + '" style="top: ' + top_s + 'px; left: ' + left_s + 'px; z-index: ' + (x + y) + '"></li>');
                        $('#mapbox ul[data-x="' + (x - 1) + '"').append('<li class="clickable" data-pos="' + (x - 1) + '_' + (y - 1) + '" style="top: ' + top_s + 'px; left: ' + left_s + 'px; z-index: ' + (x + 1) + (y) + '"></li>');
                    } else if (colunas[y - 1] === '0') {
                        $('#mapbox ul[data-x="' + (x - 1) + '"').append('<li class="tile_blank" data-pos="' + (x - 1) + '_' + (y - 1) + '" style="top: ' + (top_s + 6) + 'px; left: ' + left_s + 'px; z-index: ' + (x + y) + '"></li>');
                    }

                    if (colunas[y - 1] == '0') {
                        websocket.send('mapObstacles|' + (x - 1) + '|' + (y - 1) + '|1');
                    }
                    top_s = top_s + 16;
                    left_s = left_s + 32;
                }
                $('#mapbox').append('</ul>');
                top_s = 150 + (x * 16);
                left_s = left_s - (y * 32);
            }
        })
            .always(function () {
                //itemSpawner('1', '10_3');
                //itemSpawner('5', '1_16');
                //itemSpawner('2', '4_6');
            });
    };
    /* FIM GERAR MAPA NA CLIENT */

    /* GERA O INVENTÁRIO */
    /*for (var i = 0; i < 30; i++) {
     var img = '<img src="web-gallery/images/itens/' + user_items[i] + '_1.png" alt="" />';
     if (user_items[i] === undefined) {
     $('#inventory-box').append('<li data-slotid="' + i + '"></li>');
     } else {
     $('#inventory-box').append('<li data-slotid="' + i + '"><img src="web-gallery/images/itens/' + user_items[i] + '_1.png" alt="" /></li>');
     }
     }*/
    /* FIM DO INVENTÁRIO */

    $("#inventory-box").draggable({containment: '.gamebox', scroll: false, handle: '.top'}); // FAZ O INVENTÁRIO ARRASTÁVEL
    $("#catalog-box").draggable({containment: '.gamebox', scroll: false, handle: '.top'}); // FAZ O CATÁLOGO ARRASTÁVEL
    $("#alert-box").draggable({containment: '.gamebox', scroll: false, handle: '.header'}); // FAZ A JANELA DE
    // ALERTAARRASTÁVEL
    $("#navigator-box").draggable({containment: '.gamebox', scroll: false, handle: '.header'}); // FAZ A JANELA DE

    // TILES
    $(document).on('mouseover', '.clickable', function () {
        var pleft = parseInt($(this).position().left) + 12;
        var ptop = parseInt($(this).position().top);
        var zindex = parseInt($(this).css('z-index')) - 10;
        $(this).before('<li class="tile_hover" style="top: ' + ptop + 'px; left: ' + pleft + 'px; z-index: ' + zindex + '"></li>');
    });
    $(document).on('mouseleave', '.clickable', function () {
        $(this).prev('.tile_hover').remove();
    });

    // CATALOGUE
    $(document).on('click', '#catalog', function () {
        if ($("#catalog-box").is(':hidden')) {
            $('#cat_list').html('');
            $("#catalog-box").show();
            generateCatalog();
        } else {
            $("#catalog-box").hide();
        }
    });
    $(document).on('click', '#cat_list li', function () {
        $('#cat_list li').removeClass('page_active');
        $(this).addClass('page_active');
    });
    $(document).on('click', '#cat_list li ul li', function () {
        $('#cat_list li ul li').removeClass('page_active2');
        $(this).addClass('page_active2');
    });

    $(document).on('click', '#cat_list li', function () {
        $('#cat_list .sub_page').addClass('hidden');
        var pageId = $(this).attr('data-pageid');
        $('#cat_list .sub_page[data-parentid="' + pageId + '"]').removeClass('hidden');
    });

    // NAVIGATOR
    var navigatorTabsLi = $("#navigator-tabs li");
    $(document).on('click', '#navigator', function () {
        if ($("#navigator-box").is(':hidden')) {
            $("#navigator-box").show();
            websocket.send('get_all_rooms');
        } else {
            $("#navigator-box").hide();
        }
    });

    $(document).on('mousedown', '#navigator-public', function () {
        navigatorTabsLi.removeClass('active');
        $(this).addClass('active');
        // TODO: Load public rooms
    });

    $(document).on('mousedown', '#navigator-all', function () {
        navigatorTabsLi.removeClass('active');
        $(this).addClass('active');
        // TODO: Load all rooms
    });

    $(document).on('mousedown', '#navigator-events', function () {
        navigatorTabsLi.removeClass('active');
        $(this).addClass('active');
        // TODO: Load events rooms
    });

    $(document).on('mousedown', '#navigator-mine', function () {
        navigatorTabsLi.removeClass('active');
        $(this).addClass('active');
        // TODO: Load my own rooms
    });

    // ZINDEX AJUSTMENT FOR WINDOWS
    $(document).on('click', '#catalog-box', function () {
        $(this).css('z-index', '999999');
        $('#inventory-box').css('z-index', '999998');
    });
    $(document).on('click', '#inventory-box', function () {
        $(this).css('z-index', '999999');
        $('#catalog-box').css('z-index', '999998');
    });

    // INVENTORY
    $(document).on('click', '#inventory', function () {
        $("#inventory-box").toggle();
    });

    $(document).on('click', '#inventory-box ul.tabs li', function () {
        var tabId = $(this).attr('id').split('_');
        var tabName = tabId[1];
        $('#inventory-box .content').hide();
        $('#inventory-box ul.tabs li').removeClass('active');
        $('#tab_' + tabName).addClass('active');
        $('#cont_' + tabName).show();
    });

    $(document).on('click', '#inventory_items li', function () {
        $('#inventory_items li').removeClass('active');
        $(this).addClass('active');
    });

    items['1034'] = 'hween_c15_angel';
    items['954'] = 'bling15_pooltable';
    items['1165'] = 'exe_c15_telephone';
    items['1035'] = 'hween_c15_angel';
    items['1036'] = 'hween_c15_angel';
    items['1037'] = 'hween_c15_angel';
    items['1038'] = 'hween_c15_angel';
    $.each(items, function (key, value) {
        $('#inventory_items').append('<li><img src="furni/' + value + '/icon.png" alt="' + key + '" data-itemid="' + key + '" /></li>');
    });

    // ZOOM
    $("#zoom").on('click', function () {
        var classes = '.clickable, .char, .charh, .item, .tile';
        if ($(classes).hasClass('zoomOut')) {
            $(classes).removeClass('zoomOut');
        } else {
            $(classes).addClass('zoomOut');
        }
    });

    // Create Catalogue
    function generateCatalog() {
        $.ajax({
            url: 'controller/genCatalog.php',
            type: 'POST',
            dataType: 'JSON',
            data: {
                validation: '1'
            },
            success: function (data) {
                for (var i = 0; i < data.dados.length; i++) {
                    $('#cat_list').append('<li data-pageid="' + data.dados[i].id + '" data-active="' + data.dados[i].enable + '"><img src="./web-gallery/images/catalogue/images/icon_' + data.dados[i].icon_image + '.png" alt="icon_' + data.dados[i].icon_image + '" /> <span>' + data.dados[i].caption + '</span></li>');
                    generateCatalogSubPages(data.dados[i].id);
                }
            }
        });
    }

    // Create Catalogue sub-pages
    function generateCatalogSubPages(pageid) {
        var selector = $('li[data-pageid="' + pageid + '"]');
        $.ajax({
            url: 'controller/genCatalogSubPages.php',
            type: 'POST',
            dataType: 'JSON',
            data: {
                parent: pageid
            },
            success: function (data) {
                if (data.dados.length > 0) {
                    selector.append('<ul class="sub_page hidden" data-parentid="' + pageid + '"></ul>');
                    for (var i = 0; i < data.dados.length; i++) {
                        $('ul.sub_page[data-parentid="' + pageid + '"]').append('<li><img src="./web-gallery/images/catalogue/images/icon_' + data.dados[i].icon_image + '.png" alt="icon_' + data.dados[i].icon_image + '" /> ' + data.dados[i].caption + '</li>');
                    }
                }
            }
        });
    }

    // Load Navigator
    function loadNavigator(data) {
        var navigator = $("#navigator-rooms");
        var dataArray = $.parseJSON(data);
        navigator.html('');
        for (var key in dataArray) {
            var color;
            if (dataArray[key].users_now > 0)
                color = '#62B061';
            else
                color = '#CAC9C0';

            navigator.append('' +
                '<li><div class="users_now" style="background:' + color + '"><span class="icon-users-now"' +
                ' aria-hidden="true"></span> ' + dataArray[key].users_now + '</div> ' + dataArray[key].caption + '</li>' +
                '');
        }
    }
});