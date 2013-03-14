define('bcms.spinner.jquery', ['bcms.jquery'], function(jquery) {

/*global define, console */

(function ($) {
    'use strict';

    var spinner = { },
        selectors = {
            scrollableParents: '.bcms-scroll-window',
            spinnerContainers: '.bcms-spinner-container'
        },
        constants = {
            indicatorPrefix: 'bcms-loading-indicator',
            generatedPrefix: 'bcms-spinner-target-'
        },
        resizeTimer,
        staticId = 1;

    spinner.showLoading = function (indicatorId) {
        spinner.hideLoading(indicatorId);
        showLoading(this, indicatorId, true);

        return this;
    };
    
    spinner.hideLoading = function (indicatorId) {
        var id = createIndicatorId($(this), indicatorId);

        var loadingDiv = $('#' + id);
        loadingDiv.remove();

        return this;
    };

    function createIndicatorId(target, indicatorId) {
        if (!indicatorId) {
            indicatorId = target.attr('id') || '';
        }

        if (indicatorId) {
            return constants.indicatorPrefix + '-' + indicatorId;
        } else {
            return constants.indicatorPrefix;
        }
    }

    function showLoading(target, indicatorId) {
        target = $(target);
        if (target.length == 0) {
            return;
        }

        var targetId = target.attr('id');
        if (!targetId) {
            targetId = generateId();
            target.attr('id', targetId);
        }
        
        var id = createIndicatorId(target, indicatorId),
            border_top_width = target.css('border-top-width'),
            border_left_width = target.css('border-left-width');

        border_top_width = isNaN(parseInt(border_top_width)) ? 0 : border_top_width;
        border_left_width = isNaN(parseInt(border_left_width)) ? 0 : border_left_width;

        var left = target.offset().left,
            top = target.offset().top,

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
        var overlayDiv = $('<div class="bcms-spinner-container" id="' + id + '" style="display: none;"></div>');
        
        $(overlayDiv).css('width', overlay_width.toString() + 'px');
        $(overlayDiv).css('height', overlay_height.toString() + 'px');

        $(overlayDiv).css('left', overlay_left_pos.toString() + 'px');
        $(overlayDiv).css('position', 'fixed');

        $(overlayDiv).css('top', overlay_top_pos.toString() + 'px');
        $(overlayDiv).css('z-index', '5000');

        $(overlayDiv).data('target', targetId);

        // Create loader div
        var loaderDiv = $('<div class="bcms-loader-2"></div>');
        loaderDiv.css('left', spinner_left.toString() + 'px');
        loaderDiv.css('top', spinner_top.toString() + 'px');
        $(overlayDiv).html(loaderDiv);

        target.append(overlayDiv);
        $(overlayDiv).show();
    }

    function generateId() {
        return constants.generatedPrefix + (staticId++);
    }

    function onWindowResize() {
        $(selectors.spinnerContainers).each(function () {
            var spinnerContainer = $(this),
                targetId = spinnerContainer.data('target');
                    
            if (targetId) {
                var target = $('#' + targetId);
                if (target.length == 1) {
                    target.hideLoading(targetId);
                    target.showLoading(targetId);
                }
            }
        });
        
        return true;
    }

    /**
    * Initializes spinners when CMS loads for the first time
    */
    spinner.init = function () {
        console.log('Initializing spinner CMS');

        $.fn.showLoading = spinner.showLoading;
        $.fn.hideLoading = spinner.hideLoading;
        
        $(window).on('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(onWindowResize, 100);
        });
    };

    spinner.init();
    
})(jquery);
    
});