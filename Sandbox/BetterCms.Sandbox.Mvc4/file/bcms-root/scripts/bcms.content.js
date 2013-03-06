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
            regionActions: '.bcms-region-actions',
            renderedRegions: '.bcms-render-region',
            renderedContents: 'script[type="text/html"]'
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
    content.showOverlay = function (viewModel) {

        console.log('Show Overlay');

        var padding = 7,
            extra = padding * 2,
            // TODO: width = (element.width() + extra) + 'px',
            // TODO: height = (element.height() + extra) + 'px',
            width = (viewModel.width + extra) + 'px',
            height = (viewModel.height + extra) + 'px',
            adjustPosition = '-' + padding + 'px',
            background = $(selectors.contentOverlayBg, overlay);
            //offset = element.offset();

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
            // TODO: .offset({ top: offset.top - padding, left: offset.left - padding })
            .offset({ top: viewModel.top - padding, left: viewModel.left - padding })
            .animate({ 'opacity': 1 }, 200);
    };

    /**
    * Draws visual line over CMS region:
    */
    content.highlightRegion = function (regionViewModel) {
        var container = $('#bcms-region-overlay-template'),
            template = container.html(),
            rectangle = $(template);

        rectangle.data('target', regionViewModel);
        rectangle.insertBefore(container);
        regionRectangles = regionRectangles.add(rectangle);

        regionViewModel.overlay = rectangle;
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
                // TODO: element = rectangle.data('target'),
                regionViewModel = rectangle.data('target'),
                width = regionViewModel.width + (padding * 2),
                height = regionViewModel.height;
                // TODO: offset = element.offset(),
                // TODO: width = element.width() + (padding * 2),
                // TODO: height = element.height();

            $(selectors.regionTop, rectangle).css({ width: width + 'px' });
            $(selectors.regionBottom, rectangle).css({ width: width + 'px', top: (height - 2) + 'px' });
            $(selectors.regionLeft, rectangle).css({ height: height + 'px' });
            $(selectors.regionRight, rectangle).css({ height: height + 'px', left: (width - 2) + 'px' });
            $(selectors.regionActions, rectangle).css({ left: width - actionsContainerWidth + 'px' });

            rectangle.css({
                // TODO: top: offset.top + 'px',
                // TODO: left: offset.left - padding + 'px'
                top: regionViewModel.top + 'px',
                left: regionViewModel.left - padding + 'px'
            });
        });
    };

    /**
    * Initializes events for content overlay:
    */
    content.initOverlayEvents = function (pageViewModel) {

        contents = pageViewModel.contents;

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

        $(contents).each(function () {
            var contentViewModel = this;
            
            contentViewModel.elements.on('mouseover', function () {
                if (!bcms.editModeIsOn() || currentContentDom === contentViewModel.elements) {
                    // console.log('Exit content mouse over');
                    return;
                }

                console.log('Content mouse over');
                currentContentDom = contentViewModel.elements;

                var element = $(this);
                // element.css('outline', '1px solid red');
                content.showOverlay(contentViewModel);

                bcms.trigger(bcms.events.showOverlay, element);
            });
        });

        /*
        TODO:
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
        });*/
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
    content.initRegionEvents = function (regionViewModel) {

        var regionId = regionViewModel.id;

        $('.bcms-region-addcontent', regionViewModel.overlay).on('click', function () {
            bcms.trigger(bcms.events.addPageContent, regionId);
        });

        $('.bcms-region-sortcontent', regionViewModel.overlay).on('click', function () {
            content.turnSortModeOn(regionViewModel);
        });

        $('.bcms-region-sortdone', regionViewModel.overlay).on('click', function () {
            content.turnSortModeOff(regionViewModel);

            /*
            TODO
            var pageContents = [];
            $(content.selectors.content, target).each(function () {
                pageContents.push({ 'Id': $(this).data('pageContentId'), 'Version': $(this).data('pageContentVersion') });
            });
            var model = { region: region, data: { 'pageId': bcms.pageId, 'regionId': regionId, 'pageContents': pageContents } };

            bcms.trigger(bcms.events.sortPageContent, model);*/
        });
    };

    /**
    * Turns region content sorting mode OFF:
    */
    content.turnSortModeOff = function (regionViewModel, cancel) {
        $('.bcms-region-button', regionViewModel.overlay).show();
        $('.bcms-region-sortdone', regionViewModel.overlay).hide();

        regionViewModel.overlay.sortable('destroy');

        // TODO: fix order

        $('.bcms-sort-wrapper', regionViewModel.overlay).each(function () {
            if (cancel) {
                $(this).data('content').show();
            } else {
                // Get reference to content and append to region in same order as sorted items:
                $(this).data('content').appendTo(regionViewModel.overlay).show();
            }

            $(this).remove();
        });

        content.refreshRegionsPosition();
    };

    /**
    * Turns region content sorting mode ON:
    */
    content.turnSortModeOn = function (regionViewModel) {

        sortableRegions.push(regionViewModel);

        $('.bcms-region-button', regionViewModel.overlay).hide();
        $('.bcms-region-sortdone', regionViewModel.overlay).show();

        $(regionViewModel.contents).each(function () {
            var contentDiv = this.elements,
                sortWrapper = $('<div class="bcms-sort-wrapper" />'),
                contentCopy = contentDiv.html(),
                contentCopyClean = content.removeScripts(contentCopy);
            
            $('<div class="bcms-sort-content" />').html(contentCopyClean).appendTo(sortWrapper);
            sortWrapper.append('<div class="bcms-sort-overlay bcms-content-overlaybg" />');

            // Store reference to content so it can be sorted later:
            sortWrapper.data('content', contentDiv);

            contentDiv.hide();
            regionViewModel.overlay.append(sortWrapper);
        });

        regionViewModel.overlay.sortable();
    };

    /**
    * Updates region content versions:
    */
    content.updateRegionContentVersions = function (region, listOfIdVersion) {
        /*var pageContentItems = $(selectors.content, region.data('target'));
        for (var i = 0; i < listOfIdVersion.length; i++) {
            pageContentItems.each(function() {
                if ($(this).data('pageContentId') == listOfIdVersion[i].Id) {
                    $(this).data('pageContentVersion', listOfIdVersion[i].Version);
                }
            });
        }*/
    };

    /**
    * Removes script blocks from HTML string:
    */
    content.removeScripts = function (html) {
        var regex = new RegExp('<script.*?>.*?</script>', 'gi');
        return html.replace(regex, '');
    };

    function calculatePositions(elements) {
        var right = 0,
            bottom = 0,
            maxValue = 9999999,
            top = maxValue,
            left = maxValue;

        elements.each(function () {
            var element = $(this),
                targetOffset = element.offset(),
                targetWidth,
                targetHeight;

            // Exception handling for empty text nodes
            try {
                targetWidth = $(this).outerWidth();
                targetHeight = $(this).outerHeight();
            } catch (exc) {
                // Do nothing
                return true;
            }

            if (targetOffset != null) {
                if (targetOffset.top < top) {
                    top = targetOffset.top;
                }
                if (targetOffset.left < left) {
                    left = targetOffset.left;
                }
                if (targetOffset.left + targetWidth > right) {
                    right = targetOffset.left + targetWidth;
                }
                if (targetOffset.top + targetHeight > bottom) {
                    bottom = targetOffset.top + targetHeight;
                }
            }
        });

        if (left == maxValue) {
            left = 0;
        }
        if (top == maxValue) {
            top = 0;
        }

        return {
            left: left,
            top: top,
            width: right - left,
            height: bottom - top
        };
    }

    function PageViewModel() {
        var self = this;

        self.regions = [];
        self.contents = [];
    }

    function RegionViewModel(id, elements, regionContents) {
        var self = this,
            positions = calculatePositions(elements);

        self.id = id;
        self.elements = elements;
        self.contents = regionContents;
        self.overlay = null;
        
        self.left = positions.left;
        self.top = positions.top;
        self.width = positions.width;
        self.height = positions.height;
    }
    
    function ContentViewModel(elements) {
        var self = this,
            positions = calculatePositions(elements);;

        self.elements = elements;

        self.left = positions.left;
        self.top = positions.top;
        self.width = positions.width;
        self.height = positions.height;

        return self;
    }

    /**
    * Initializes events for regions:
    */
    content.initRegions = function () {
        console.log('Highlight regions');

        var pageViewModel = new PageViewModel();

        // Loop through all the regions
        $(selectors.renderedRegions).each(function () {
            var region = $(this),
                id = region.data('id'),
                regionContents = region.find(selectors.renderedContents),
                regionContentContainer = $('<div></div>'),
                regionContent,
                regionViewModel,
                regionContentViewModels = [];

            if (regionContents.length > 0) {

                // Loop through all the region contents
                regionContents.each(function() {
                    var htmlScript = $($(this).html());

                    regionContentContainer.append(htmlScript);

                    var contentViewModel = new ContentViewModel(htmlScript);
                    regionContentViewModels.push(contentViewModel);
                    pageViewModel.contents.push(contentViewModel);
                });

                // Replace html script with html content
                regionContent = $(regionContentContainer.html());
                region.replaceWith(regionContent);
                
                regionViewModel = new RegionViewModel(id, regionContent, regionContentViewModels);
            } else {
                regionViewModel = new RegionViewModel(id, region, regionContentViewModels);
            }

            content.highlightRegion(regionViewModel);
            pageViewModel.regions.push(regionViewModel);
        });

        $.each(pageViewModel.regions, function () {
            content.initRegionEvents(this);
        });

        content.refreshRegionsPosition();

        $(window).on('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(content.refreshRegionsPosition, 100);
        });

        /*
        content.initOverlayEvents(pageViewModel);
        */
    };

    /**
    * Cancels sort mode after the 'editModeOff' event is triggered.
    */
    content.cancelSortMode = function () {
        $.each(sortableRegions, function (i, regionViewModel) {
            content.turnSortModeOff(regionViewModel, true);
        });

        sortableRegions = [];
        console.log('Cancel Sort Mode');
    };

    /**
    * Initializes sidebar module.
    */
    content.init = function () {
        console.log('Initializing content module');
        // content.initOverlayEvents();
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
