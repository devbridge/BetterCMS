/*global define, console */

(function ($) {
    'use strict';

    var spinner = { },
        spinnerCounter = 0,
        spinnerInterval = null,
        spinnerWidth = 20,
        
        // Selectors used in the module to locate DOM elements:
        selectors = {
            loadingIndicator: '.loading-indicator',
            loadingIndicatorInverse: '.loading-indicator-inverse',
            loadingIndicatorInverseVisible: '.loading-indicator-inverse:visible',
        };

    spinner.enableSpinners = function () {
        var totalSpinners = spinner.countActiveSpinners();
        if (totalSpinners > 0) {
            window.clearInterval(spinnerInterval);
            spinnerInterval = window.setInterval(function () { spinner.spinSpinner(); }, 100);
        }
    };

    spinner.disableSpinners = function () {
        var totalSpinners = spinner.countActiveSpinners();
        if (totalSpinners == 0) {
            window.clearInterval(spinnerInterval);
        }
    };

    spinner.spinSpinner = function () {
        var backgroundPosition = spinnerCounter * spinnerWidth;

        if (backgroundPosition !== 0) {
            backgroundPosition = backgroundPosition * -1;
        }

        $(selectors.loadingIndicator).css('background-position', backgroundPosition + 'px 0');
        $(selectors.loadingIndicatorInverse).css('background-position', backgroundPosition + 'px 20px');
        if (spinnerCounter == 11) {
            spinnerCounter = 0;
        }
        spinnerCounter++;
    };

    spinner.countActiveSpinners = function () {
        var totalSpinners = $(selectors.loadingIndicatorInverseVisible).length;
        if (typeof totalSpinners == 'undefined') {
            totalSpinners = 0;
        };

        return totalSpinners + $(selectors.loadingIndicator).length;
    };

    spinner.showSpinner = function (spinnerContainer) {
        $(spinnerContainer).show();
        spinner.enableSpinners();
    };

    spinner.hideSpinner = function (spinnerContainer) {
        $(spinnerContainer).hide();
        spinner.disableSpinners();
    };

    spinner.showLoading = function (options) {
        var indicatorId;

        var settings = {
            'addClass': '',
            'beforeShow': '',
            'afterShow': '',
            'hPos': 'center',
            'vPos': 'center',
            'indicatorZIndex': 5001,
            'overlayZIndex': 5000,
            'parent': null,
            'marginTop': 0,
            'marginLeft': 0,
            'overlayWidth': null,
            'overlayHeight': null,
            'position': 'absolute',
            'inverse': false
        };

        $.extend(settings, options);

        var loadingDiv = $('<div></div>');
        var overlayDiv = $('<div></div>');
        var inverse = '';

        if (settings.inverse) {
            inverse = '-inverse';
        }

        //
        // Set up ID and classes
        //
        if (settings.indicatorId) {
            indicatorId = settings.indicatorId;
        }
        else {
            indicatorId = $(this).attr('id');
        }

        $(loadingDiv).attr('id', 'loading-indicator-' + indicatorId);

        $(loadingDiv).addClass('loading-indicator' + inverse);

        if (settings.addClass) {
            $(overlayDiv).addClass(settings.addClass + '-overlay');
        }

        if (settings.addClass) {
            $(loadingDiv).addClass(settings.addClass);
        }



        //
        // Create the overlay
        //
        $(overlayDiv).css('display', 'none');

        // Append to body, otherwise position() doesn't work on Webkit-based browsers
        $(this).append(overlayDiv);

        //
        // Set overlay classes
        //
        $(overlayDiv).attr('id', 'loading-indicator-' + indicatorId + '-overlay');

        $(overlayDiv).addClass('loading-indicator-overlay' + inverse);

        if (settings.addClass) {
            $(overlayDiv).addClass(settings.addClass + '-overlay');
        }

        //
        // Set overlay position
        //

        var overlay_width;
        var overlay_height;

        var border_top_width = $(this).css('border-top-width');
        var border_left_width = $(this).css('border-left-width');

        //
        // IE will return values like 'medium' as the default border, 
        // but we need a number
        //
        border_top_width = isNaN(parseInt(border_top_width)) ? 0 : border_top_width;
        border_left_width = isNaN(parseInt(border_left_width)) ? 0 : border_left_width;

        var left = $(this).css('position') == 'relative' ? 0 : $(this).position().left;
        var top = $(this).css('position') == 'relative' ? 0 : $(this).position().top;

        var overlay_left_pos = left + parseInt(border_left_width);
        var overlay_top_pos = top + parseInt(border_top_width);

        if (settings.overlayWidth !== null) {
            overlay_width = settings.overlayWidth;
        }
        else {
            overlay_width = parseInt($(this).width()) + parseInt($(this).css('padding-right')) + parseInt($(this).css('padding-left'));
        }

        if (settings.overlayHeight !== null) {
            overlay_height = settings.overlayWidth;
        }
        else {
            overlay_height = parseInt($(this).height()) + parseInt($(this).css('padding-top')) + parseInt($(this).css('padding-bottom'));
        }


        $(overlayDiv).css('width', overlay_width.toString() + 'px');
        $(overlayDiv).css('height', overlay_height.toString() + 'px');

        $(overlayDiv).css('left', overlay_left_pos.toString() + 'px');
        $(overlayDiv).css('position', settings.position);

        $(overlayDiv).css('top', overlay_top_pos.toString() + 'px');
        $(overlayDiv).css('z-index', settings.overlayZIndex);

        //
        // Set any custom overlay CSS		
        //
        if (settings.overlayCSS) {
            $(overlayDiv).css(settings.overlayCSS);
        }


        //
        // We have to append the element to the body first
        // or .width() won't work in Webkit-based browsers (e.g. Chrome, Safari)
        //
        $(loadingDiv).css('display', 'none');
        $(this).append(loadingDiv);

        $(loadingDiv).css('position', settings.position);
        $(loadingDiv).css('z-index', settings.indicatorZIndex);

        //
        // Set top margin
        //

        var indicatorTop = overlay_top_pos;

        if (settings.marginTop) {
            indicatorTop += parseInt(settings.marginTop);
        }

        var indicatorLeft = overlay_left_pos;

        if (settings.marginLeft) {
            indicatorLeft += parseInt(settings.marginTop);
        }


        //
        // set horizontal position
        //
        if (settings.hPos.toString().toLowerCase() == 'center') {
            $(loadingDiv).css('left', (indicatorLeft + (($(overlayDiv).width() - parseInt($(loadingDiv).width())) / 2)).toString() + 'px');
        }
        else if (settings.hPos.toString().toLowerCase() == 'left') {
            $(loadingDiv).css('left', (indicatorLeft + parseInt($(overlayDiv).css('margin-left'))).toString() + 'px');
        }
        else if (settings.hPos.toString().toLowerCase() == 'right') {
            $(loadingDiv).css('left', (indicatorLeft + ($(overlayDiv).width() - parseInt($(loadingDiv).width()))).toString() + 'px');
        }
        else {
            $(loadingDiv).css('left', (indicatorLeft + parseInt(settings.hPos)).toString() + 'px');
        }

        //
        // set vertical position
        //
        if (settings.vPos.toString().toLowerCase() == 'center') {
            $(loadingDiv).css('top', (indicatorTop + (($(overlayDiv).height() - parseInt($(loadingDiv).height())) / 2)).toString() + 'px');
        }
        else if (settings.vPos.toString().toLowerCase() == 'top') {
            $(loadingDiv).css('top', indicatorTop.toString() + 'px');
        }
        else if (settings.vPos.toString().toLowerCase() == 'bottom') {
            $(loadingDiv).css('top', (indicatorTop + ($(overlayDiv).height() - parseInt($(loadingDiv).height()))).toString() + 'px');
        }
        else {
            $(loadingDiv).css('top', (indicatorTop + parseInt(settings.vPos)).toString() + 'px');
        }




        //
        // Set any custom css for loading indicator
        //
        if (settings.css) {
            $(loadingDiv).css(settings.css);
        }


        //
        // Set up callback options
        //
        var callback_options =
                {
                    'overlay': overlayDiv,
                    'indicator': loadingDiv,
                    'element': this
                };

        //
        // beforeShow callback
        //
        if (typeof (settings.beforeShow) == 'function') {
            settings.beforeShow(callback_options);
        }

        //
        // Show the overlay
        //
        $(overlayDiv).show();

        //
        // Show the loading indicator
        //
        $(loadingDiv).show();

        //
        // afterShow callback
        //
        if (typeof (settings.afterShow) == 'function') {
            settings.afterShow(callback_options);
        }

        spinner.enableSpinners();

        return this;
    };
    
    spinner.hideLoading = function (options) {
        var settings = { },
            indicatorId;

        $.extend(settings, options);

        if (settings.indicatorId) {
            indicatorId = settings.indicatorId;
        }
        else {
            indicatorId = $(this).attr('id');
        }

        var loadingDiv = $(document.body).find('#loading-indicator-' + indicatorId);
        var overlayDiv = $(document.body).find('#loading-indicator-' + indicatorId + '-overlay');

        loadingDiv.remove();
        overlayDiv.remove();

        spinner.disableSpinners();

        return this;
    };

    /**
    * Initializes CMS when it loads for the first time
    */
    spinner.init = function () {
        console.log('Initializing spinner CMS');

        $.fn.showLoading = spinner.showLoading;
        $.fn.hideLoading = spinner.hideLoading;
    };

    spinner.init();
    
})(jQuery);
