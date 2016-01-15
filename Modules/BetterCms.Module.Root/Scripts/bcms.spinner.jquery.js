/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.spinner.jquery.js" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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