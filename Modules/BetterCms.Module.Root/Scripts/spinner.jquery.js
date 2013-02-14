/*global define, console */

(function ($) {
    'use strict';

    var spinner = { },
        selectors = {
            scrollableParents: '.bcms-scroll-window'
        };

    spinner.showLoading = function (indicatorId) {
        showLoading(this, indicatorId, true);

        return this;
    };
    
    spinner.hideLoading = function (indicatorId) {
        if (!indicatorId) {
            indicatorId = $(this).attr('id');
        }

        var loadingDiv = $('#loading-indicator-' + indicatorId);
        loadingDiv.remove();

        loadingDiv = $('#loading-indicator-' + indicatorId + '-overlay');
        loadingDiv.remove();

        return this;
    };

    function showLoading(target, indicatorId) {
        target = $(target);
        
        if (!indicatorId) {
            indicatorId = target.attr('id') || '';
        }

        var border_top_width = target.css('border-top-width'),
            border_left_width = target.css('border-left-width');

        border_top_width = isNaN(parseInt(border_top_width)) ? 0 : border_top_width;
        border_left_width = isNaN(parseInt(border_left_width)) ? 0 : border_left_width;

        var left = target.css('position') == 'relative' ? 0 : target.position().left,
            top = target.css('position') == 'relative' ? 0 : target.position().top,

            overlay_left_pos = left + parseInt(border_left_width),
            overlay_top_pos = top + parseInt(border_top_width),

            overlay_width = parseInt(target.width()) + parseInt(target.css('padding-right')) + parseInt(target.css('padding-left')),
            overlay_height = parseInt(target.height()) + parseInt(target.css('padding-top')) + parseInt(target.css('padding-bottom')),

            spinner_left = Math.round(overlay_left_pos + overlay_width / 2),
            spinner_top = Math.round(overlay_top_pos + overlay_height / 2),
            
            scrollableParent = target.closest(selectors.scrollableParents);

        if (scrollableParent.length > 0) {
            var scrollableHeight = scrollableParent.height(),
                scrollableWidth = scrollableParent.width();
                
            if (scrollableHeight && overlay_height > scrollableHeight) {
                spinner_top = Math.round(scrollableHeight / 2);
            }

            if (scrollableWidth && overlay_width > scrollableWidth) {
                spinner_left = Math.round(scrollableWidth / 2);
            }
        }

        // Create overlay div
        var overlayDiv = $('<div style="display: none;"></div>');
        $(overlayDiv).attr('id', 'loading-indicator-' + indicatorId);
        
        $(overlayDiv).css('width', overlay_width.toString() + 'px');
        $(overlayDiv).css('height', overlay_height.toString() + 'px');

        $(overlayDiv).css('left', overlay_left_pos.toString() + 'px');
        $(overlayDiv).css('position', 'absolute');

        $(overlayDiv).css('top', overlay_top_pos.toString() + 'px');
        $(overlayDiv).css('z-index', '5000');

        // Create loader div
        var loaderDiv = $('<div class="bcms-loader-2"></div>');
        loaderDiv.css('left', spinner_left.toString() + 'px');
        loaderDiv.css('top', spinner_top.toString() + 'px');
        $(overlayDiv).html(loaderDiv);

        target.append(overlayDiv);
        $(overlayDiv).show();
    }

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
