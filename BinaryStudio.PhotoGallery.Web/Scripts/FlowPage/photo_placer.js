﻿$(document).ready(function () {
    $("#photopreloader").hide();
    $(window).load(function() {
        prepareToShow();
        scrolled();
    });
    $(window).on('resize', function () {
        $LastRow = calcPhotoSizes($('#photoWrapper'), $("div.photoContainer > img"), marginsOfPhotoCont);
    });
    $(window).scroll(scrolled);

    var $LastRow = new Array();
    var busy = false;
    var scrHeight = $(window).height();

    function scrolled() {
        if (!busy) {
            busy = true;
            var underScroll = $(this).scrollTop();
            var phWrapHeight = $("#photoWrapper").height();
            var scrollPos = scrHeight + underScroll;

            if (phWrapHeight - scrollPos < 300) {
                ajaxPhotoLoad();
            } else {
                busy = false;
            }
                    
        }
    }

    var marginsOfPhotoCont = parseInt($('.photoContainer').css('margin-left'))
                             + parseInt($('.photoContainer').css('margin-right'))
                             + parseInt($('.photoContainer').css("border-left-width"))
                             + parseInt($('.photoContainer').css("border-right-width"));
     
    function calcPhotoSizes($container, $photos, marginPhotoCont) {
        var width = 0;
        var firstElemInRow = 0;
        var margins = 0;
        var wrapperWidth = $container.width();
        var $lastRow = $();

        jQuery.each($photos, function (indPh) {
            width += this.width;
            margins += marginPhotoCont;
            if (width > wrapperWidth - margins) {
                var coef = (wrapperWidth - margins) / width;
                for (var indSub = firstElemInRow; indSub <= indPh; indSub++) {
                    $($photos[indSub]).closest("div").css('width', ($photos[indSub].width * coef) - 0.2);
                    $($photos[indSub]).closest("div").addClass("resized");
                }
                firstElemInRow = indPh + 1;
                width = 0;
                margins = 0;
            }
            else if (indPh == $photos.length - 1) {
                for (indSub = firstElemInRow; indSub <= indPh; indSub++) {
                    $($photos[indSub]).closest("div")
                        .css('width', $photos[indSub].width);
                    $lastRow.push($photos[indSub]);
                }
            }

        });
        return ($lastRow);
    }
    
    function prepareToShow() {
        var $newPhotoContainers = $('#photoWrapper > div.invisible');
        var $photos = $newPhotoContainers.find("img:first");
        $photos = $.merge($LastRow, $photos);
        $LastRow = calcPhotoSizes($('#photoWrapper'), $photos, marginsOfPhotoCont);
        $newPhotoContainers.removeClass("invisible");
    }

    var photoPortion = 30;
    var startIndex = photoPortion;

    function ajaxPhotoLoad() {
        $("#photopreloader").show();
        $.post("/Home/GetPhotosViaAjax", { startIndex: startIndex, endIndex: startIndex + photoPortion }, getPhotos);
    }

    function getPhotos(photos) {
        if (photos.length > 0) {
            ko.utils.arrayPushAll(window.viewModel.Photos, photos);
            prepareToShow();
            busy = false;
            startIndex += photoPortion;
        } else {
            $(window).unbind("scroll");
        }
        $("#photopreloader").hide();
    }

});
