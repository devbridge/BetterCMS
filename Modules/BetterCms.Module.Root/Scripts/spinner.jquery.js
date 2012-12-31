/*global define, console */

(function ($) {
    'use strict';

    var spinner = { };

    spinner.showLoading = function (indicatorId) {
        var loadingDiv = $('<div class="bcms-loader"></div>');
        
        if (!indicatorId) {
            indicatorId = $(this).attr('id');
        }

        $(loadingDiv).attr('id', 'loading-indicator-' + indicatorId);

        var border_top_width = $(this).css('border-top-width');
        var border_left_width = $(this).css('border-left-width');

        border_top_width = isNaN(parseInt(border_top_width)) ? 0 : border_top_width;
        border_left_width = isNaN(parseInt(border_left_width)) ? 0 : border_left_width;

        var left = $(this).css('position') == 'relative' ? 0 : $(this).position().left;
        var top = $(this).css('position') == 'relative' ? 0 : $(this).position().top;

        var overlay_left_pos = left + parseInt(border_left_width);
        var overlay_top_pos = top + parseInt(border_top_width);

        var overlay_width = parseInt($(this).width()) + parseInt($(this).css('padding-right')) + parseInt($(this).css('padding-left'));
        var overlay_height = parseInt($(this).height()) + parseInt($(this).css('padding-top')) + parseInt($(this).css('padding-bottom'));

        $(loadingDiv).css('display', 'none');
        $(this).append(loadingDiv);

        $(loadingDiv).css('width', overlay_width.toString() + 'px');
        $(loadingDiv).css('height', overlay_height.toString() + 'px');

        $(loadingDiv).css('left', overlay_left_pos.toString() + 'px');
        $(loadingDiv).css('position', 'absolute');

        $(loadingDiv).css('top', overlay_top_pos.toString() + 'px');
        $(loadingDiv).css('z-index', '5000');

        $(loadingDiv).show();

        return this;
    };
    
    spinner.hideLoading = function (indicatorId) {
        if (!indicatorId) {
            indicatorId = $(this).attr('id');
        }

        var loadingDiv = $('#loading-indicator-' + indicatorId);

        loadingDiv.remove();

        return this;
    };

    /**
    * Initializes spinners when CMS loads for the first time
    */
    spinner.init = function () {
        console.log('Initializing spinner CMS');

        $.fn.showLoading = spinner.showLoading;
        $.fn.hideLoading = spinner.hideLoading;
    };

    spinner.init();
    
})(jQuery);
