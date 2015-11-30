bettercms.define('bcms.spinner.jquery', ['bcms.jquery', 'bcms'], function(jquery, bcmsjs) {

/*global bettercms */

(function ($, bcms) {
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
        staticId = 1,
        activeSpinners = [];

    spinner.showLoading = function (indicatorId, hideAll) {
        if (typeof hideAll !== "boolean") {
            hideAll = true;
        }
        
        if (hideAll) {
            stopAllLoaders();
        } else {
            spinner.hideLoading(indicatorId);
        }

        var target = $(this);
        if (target.length == 0) {
            return null;
        }

        var targetId = target.attr('id');
        if (!targetId) {
            targetId = createTargetId();
            target.attr('id', targetId);
        }

        var id = createIndicatorId(target, indicatorId);
        activeSpinners.push(id);

        startLoading(target, id);

        return this;
    };

    spinner.hideLoading = function (indicatorId) {
        var id = createIndicatorId($(this), indicatorId);

        for (var i = 0; i < activeSpinners.length; i++) {
            if (activeSpinners[i] == id) {
                activeSpinners.splice(i, 1);
                break;
            }
        }

        stopLoading(id);

        return this;
    };

    function startLoading(target, id) {

        var targetId = target.attr('id'),
            overlay_width = parseInt(target.width()) + parseInt(target.css('padding-right')) + parseInt(target.css('padding-left')),
            overlay_height = parseInt(target.height()) + parseInt(target.css('padding-top')) + parseInt(target.css('padding-bottom'));

        // Create overlay div
        var overlayDiv = $('<div class="bcms-spinner-container" id="' + id + '" style="display: none; left: 0; top: 0; position: absolute;"></div>');

        $(overlayDiv).css('width', overlay_width.toString() + 'px');
        $(overlayDiv).css('height', overlay_height.toString() + 'px');
        $(overlayDiv).css('z-index', '5000');
        $(overlayDiv).data('target', targetId);

        // Create loader div
        var loaderDiv = $('<div class="bcms-loader-2"></div>');
        $(overlayDiv).html(loaderDiv);

        target.append(overlayDiv);
        $(overlayDiv).show();

        return id;
    }

    function stopLoading(id) {
        var loadingDiv = $('#' + id);
        loadingDiv.remove();
    }

    function stopAllLoaders() {
        
        $(selectors.spinnerContainers).each(function () {
            var spinnerContainer = $(this);
            spinnerContainer.remove();
        });

        activeSpinners = [];
    }

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

    function createTargetId() {
        return constants.generatedPrefix + (staticId++);
    }

    function onWindowResize() {
        $(selectors.spinnerContainers).each(function () {
            var spinnerContainer = $(this),
                indicatorId = spinnerContainer.data('target'),
                id = spinnerContainer.attr('id');
                    
            if (indicatorId) {
                var target = $('#' + indicatorId);
                if (target.length == 1) {

                    stopLoading(id);
                        
                    var stillActive = false;
                    for (var i = 0; i < activeSpinners.length; i++) {
                        if (id == activeSpinners[i]) {
                            stillActive = true;
                            break;
                        }
                    }

                    if (stillActive) {
                        startLoading(target, id);
                    }
                }
            }
        });
        
        return true;
    }

    /**
    * Initializes spinners when CMS loads for the first time
    */
    spinner.init = function () {
        bcms.logger.debug('Initializing spinner CMS');

        $.fn.showLoading = spinner.showLoading;
        $.fn.hideLoading = spinner.hideLoading;
        
        $(window).on('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(onWindowResize, 100);
        });
    };

    spinner.init();
    
})(jquery, bcmsjs);
    
});