/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.content', ['jquery', 'bcms', 'bcms.modal'], function ($, bcms, modal) {
    'use strict';

    var content = {},

        // Selectors used in the module to locate DOM elements:
        selectors = {
            contentOverlay: '#bcms-content-overlay',
            contentOverlayBg: '.bcms-content-overlaybg',
            content: '.bcms-content',
            contentDelete: '.bcms-content-delete',
            contentEdit: '.bcms-content-edit',
            contentHistory: '.bcms-content-history',
            contentConfigure: '.bcms-content-configure',
            region: '.bcms-region',
            regionTop: '.bcms-region-top',
            regionBottom: '.bcms-region-bottom',
            regionLeft: '.bcms-region-left',
            regionRight: '.bcms-region-right',
            regionActions: '.bcms-region-actions'
        },
        resizeTimer,
        contents,
        overlay,
        currentContentDom,
        regionRectangles = $(),
        sortableRegions = [],
        links = {},
        globalization = {};

    // Assign objects to module
    content.selectors = selectors;
    content.links = links;
    content.globalization = globalization;

    /**
    * Shows overlay over content region:
    */
    content.showOverlay = function (element) {

        console.log('Show Overlay');

        var padding = 7,
            extra = padding * 2,
            width = (element.width() + extra) + 'px',
            height = (element.height() + extra) + 'px',
            adjustPosition = '-' + padding + 'px',
            background = $(selectors.contentOverlayBg, overlay),
            offset = element.offset();

        background.css({
            'width': width,
            'height': height
        });

        overlay.css({
            'width': width,
            'height': height,
            'top': adjustPosition,
            'left': adjustPosition,
            'opacity': 0
        })
            .show()
            .offset({ top: offset.top - padding, left: offset.left - padding })
            .animate({ 'opacity': 1 }, 300);
    };

    /**
    * Draws visual line over CMS region:
    */
    content.highlightRegion = function (element) {
        var container = $('#bcms-region-overlay-template'),
            template = container.html(),
            rectangle = $(template).css('z-index', 1000);

        rectangle.data('target', element);
        rectangle.insertBefore(container);
        regionRectangles = regionRectangles.add(rectangle);
    };

    /**
    * Forces each region outline to update it's position:
    */
    content.refreshRegionsPosition = function () {
        console.log('Resizing content');

        var actionsContainerWidth = regionRectangles.first().find(selectors.regionActions).width() + 4;

        regionRectangles.each(function () {
            var padding = 10,
                rectangle = $(this),
                element = rectangle.data('target'),
                offset = element.offset(),
                width = element.width() + (padding * 2),
                height = element.height();

            $(selectors.regionTop, rectangle).css({ width: width + 'px' });
            $(selectors.regionBottom, rectangle).css({ width: width + 'px', top: (height - 2) + 'px' });
            $(selectors.regionLeft, rectangle).css({ height: height + 'px' });
            $(selectors.regionRight, rectangle).css({ height: height + 'px', left: (width - 2) + 'px' });
            $(selectors.regionActions, rectangle).css({ left: width - actionsContainerWidth + 'px' });

            rectangle.css({
                top: offset.top + 'px',
                left: offset.left - padding + 'px'
            });
        });
    };

    /**
    * Initializes events for content overlay:
    */
    content.initOverlayEvents = function () {

        contents = $(selectors.content);

        var html = $(selectors.contentOverlay).html(),
            $html = $(html).hide();

        overlay = $html.appendTo('body');

        $(selectors.contentDelete, overlay).on('click', function () {
            bcms.trigger(bcms.events.deleteContent, $(currentContentDom));
        });

        $(selectors.contentEdit, overlay).on('click', function () {
            bcms.trigger(bcms.events.editContent, $(currentContentDom));
        });

        $(selectors.contentHistory, overlay).on('click', function () {
            bcms.trigger(bcms.events.contentHistory, $(currentContentDom));
        });

        $(selectors.contentConfigure, overlay).on('click', function () {
            bcms.trigger(bcms.events.configureContent, $(currentContentDom));
        });

        overlay.on('mouseleave', function () {
            content.hideOverlay();

            bcms.trigger(bcms.events.hideOverlay);
        });

        contents.on('mouseover', function () {
            if (!bcms.editModeIsOn() || currentContentDom === this) {
                // console.log('Exit content mouse over');
                return;
            }

            console.log('Content mouse over');
            currentContentDom = this;

            var element = $(this);
            // element.css('outline', '1px solid red');
            content.showOverlay(element);

            bcms.trigger(bcms.events.showOverlay, element);
        });
    };

    /**
    * Hides content overlay:
    */
    content.hideOverlay = function () {
        currentContentDom = null;
        overlay.hide();
    };

    /**
    * Initializes events for region buttons:
    */
    content.initRegionEvents = function (region) {
        var target = region.data('target'),
            regionId = target.data('id');

        $('.bcms-region-addcontent', region).on('click', function () {
            bcms.trigger(bcms.events.addPageContent, regionId);
        });

        $('.bcms-region-sortcontent', region).on('click', function () {
            content.turnSortModeOn(region);
        });

        $('.bcms-region-sortdone', region).on('click', function () {
            content.turnSortModeOff(region);
            
            var pageContents = [];
            $(content.selectors.content, target).each(function () {
                pageContents.push({ 'Id': $(this).data('pageContentId'), 'Version': $(this).data('pageContentVersion') });
            });
            var model = { region: region, data: { 'pageId': bcms.pageId, 'regionId': regionId, 'pageContents': pageContents } };

            bcms.trigger(bcms.events.sortPageContent, model);
        });
    };

    /**
    * Turns region content sorting mode OFF:
    */
    content.turnSortModeOff = function (region, cancel) {
        var target = region.data('target');

        $('.bcms-region-button', region).show();
        $('.bcms-region-sortdone', region).hide();

        $(target).sortable('destroy');

        $('.bcms-sort-wrapper', target).each(function () {
            if (cancel) {
                $(this).data('content').show();
            } else {
                // Get reference to content and append to region in same order as sorted items:
                $(this).data('content').appendTo(target).show();
            }

            $(this).remove();
        });

        content.refreshRegionsPosition();
    };

    /**
    * Turns region content sorting mode ON:
    */
    content.turnSortModeOn = function (region) {
        var target = region.data('target');

        sortableRegions.push(region);

        $('.bcms-region-button', region).hide();
        $('.bcms-region-sortdone', region).show();

        $(selectors.content, target).each(function () {
            var contentDiv = $(this),
                sortWrapper = $('<div class="bcms-sort-wrapper" />'),
                contentCopy = contentDiv.html(),
                contentCopyClean = content.removeScripts(contentCopy);

            $('<div class="bcms-sort-content" />').html(contentCopyClean).appendTo(sortWrapper);
            sortWrapper.append('<div class="bcms-sort-overlay bcms-content-overlaybg" />');

            // Store reference to content so it can be sorted later:
            sortWrapper.data('content', contentDiv);

            contentDiv.hide();
            target.append(sortWrapper);
        });

        $(target).sortable();
    };

    /**
    * Updates region content versions:
    */
    content.updateRegionContentVersions = function (region, listOfIdVersion) {
        var pageContentItems = $(selectors.content, region.data('target'));
        for (var i = 0; i < listOfIdVersion.length; i++) {
            pageContentItems.each(function() {
                if ($(this).data('pageContentId') == listOfIdVersion[i].Id) {
                    $(this).data('pageContentVersion', listOfIdVersion[i].Version);
                }
            });
        }
    };

    /**
    * Removes script blocks from HTML string:
    */
    content.removeScripts = function (html) {
        var regex = new RegExp('<script.*?>.*?</script>', 'gi');
        return html.replace(regex, '');
    };

    /**
    * Initializes events for regions:
    */
    content.initRegions = function () {
        console.log('Highlight regions');

        $(selectors.region).each(function (index, element) {
            var target = $(element);
            content.highlightRegion(target);
        });

        regionRectangles.each(function () {
            content.initRegionEvents($(this));
        });

        content.refreshRegionsPosition();

        $(window).on('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(content.refreshRegionsPosition, 100);
        });
    };

    /**
    * Cancels sort mode after the 'editModeOff' event is triggered.
    */
    content.cancelSortMode = function () {
        $.each(sortableRegions, function (i, region) {
            content.turnSortModeOff(region, true);
        });

        sortableRegions = [];
        console.log('Cancel Sort Mode');
    };

    /**
    * Initializes sidebar module.
    */
    content.init = function () {
        console.log('Initializing content module');
        content.initOverlayEvents();
        content.initRegions();
    };

    /**
    * Subscribe to event, so that when edit mode is turned off cancel sort mode
    */
    bcms.on(bcms.events.editModeOff, content.cancelSortMode);

    /**
    * Register initialization
    */
    bcms.registerInit(content.init);

    return content;
});
