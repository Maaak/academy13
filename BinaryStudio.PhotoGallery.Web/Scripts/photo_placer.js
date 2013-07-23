﻿$(document).ready(function () {
    $(window).resize(CalcPhotoSizes);
});
    var width = 0;
    jQuery.each(photos, function(i) {
        width += this.width;
        margins += marginPhotoCont;
        if (width > wrapperWidth - margins) {
            var coef = (wrapperWidth - margins) / width;
            for (var j = firstElemInRow; j <= i; j++) {
                $(photos[j]).closest(".photoContainer").css('width', (photos[j].width * coef) - 0.2);
            }
            firstElemInRow = i + 1;
            width = 0;
            margins = 0;
        }
    });
}