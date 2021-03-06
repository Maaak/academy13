﻿$(document).ready(function () {
    var windowObject = $(window);
    var documentObject = $(document);
    var albumApiUrl = $("#albumApiUrl").data("url");
    var userApiUrl = $("#userApiUrl").data("url");
    var takeAlbumsCount = 10;
    var skipCount = 0;
    var userId = 0;
    
    windowObject.scroll(scrolling);
    windowObject.resize(resizeTable);
    resizeTable();
    
    downloadUserInfo();

    function downloadUserInfo() {
        $.get(userApiUrl, getInfo);
    }
    
    function moveNoAlbumsContainer() {
        var alb = $('.albums');
        var noAlbContainer = $('.noAlbumsTextContainer');
        noAlbContainer.offset({ top: (alb.height() - noAlbContainer.height()) / 2, left: (alb.width() - noAlbContainer.width()) / 2 });
    }

    function resizeTable() {
        $("#content").height(windowObject.height() - $("#toolbar").height());
    }

    function getInfo(inf) {
        $("#userInformation").html($("#userTmpl").render(inf));
        userId = inf.Id;
        downloadNextPartionOfAlbums(userId);
    }

    function downloadNextPartionOfAlbums(id) {
        $.get(albumApiUrl, { userId: id, skip: skipCount, take: takeAlbumsCount }, getAlbums);
        skipCount += takeAlbumsCount;
    }
    function getAlbums(albums) {
        var length = albums.length;
        if (length > 0) {
            $(".albums").html(
                $("#collageTmpl").render(albums));
        } else {
            if (skipCount - takeAlbumsCount == 0) {
                $(".albums").html(
                    $("#uploadTmpl").render());
                windowObject.resize(moveNoAlbumsContainer);
                moveNoAlbumsContainer();
                windowObject.resize(resizeArrow);
                resizeArrow();
            }
            windowObject.unbind("scroll");
        }
    }
    
    function resizeArrow() {
        var tool = $("#toolbar");
        var noAlbumText = $('.noAlbumsText');
        var arrowJava = document.getElementById("arrow");
        var arrowJQuery = $('#arrow');
        var button = $('.btn.btn-success');

        var noOffset = noAlbumText.offset();
        var toolOffset = tool.offset();

        var lft = noOffset.left + noAlbumText.width() / 2;
        var tp = toolOffset.top + tool.height();
        var wdth = button.offset().left + button.width() / 2 - lft;
        var hght = noOffset.top - toolOffset.top - tool.height();
        
        arrowJQuery.offset({ left: lft, top: tp });
        arrowJava.width = wdth;
        arrowJava.height = hght;

        var canvas = document.getElementById("arrow");
        var ctx = canvas.getContext('2d');
        ctx.lineWidth = 2;
        ctx.moveTo(0, canvas.height);
        ctx.lineTo(canvas.width, 0);
        ctx.strokeStyle = "yellow";
        ctx.stroke();
    }

    function scrolling() {
        if (windowObject.scrollTop() == (documentObject.height() - windowObject.height())) {
            //Пользователь долистал до низа страницы
            downloadNextPartionOfAlbums(userId);
        }
    }
});