// TODO: remove after tests
window.cms = {};

/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.content', ['bcms.jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var content = {},

        // Selectors used in the module to locate DOM elements:
        selectors = {
            contentOverlay: '#bcms-content-overlay',
            contentDelete: '.bcms-content-delete',
            contentEdit: '.bcms-content-edit',
            contentEditInnerDiv: '.bcms-content-edit .bcms-content-icon',
            contentHistory: '.bcms-content-history',
            contentConfigure: '.bcms-content-configure',
            enterChildContent: '.bcms-enter-child-content',

            regionsAndContents: '.bcms-region-start, .bcms-region-end, .bcms-content-start, .bcms-content-end',
            regionOverlay: '#bcms-region-overlay-template',

            regionAddContentButtons: '.bcms-region-addcontent',
            regionSortButtons: '.bcms-region-sortcontent',
            regionSortDoneButtons: '.bcms-region-sortdone',
            regionSortCancelButtons: '.bcms-region-sortcancel',
            regionButtons: '.bcms-region-button',
            regionSortWrappers: '.bcms-sort-wrapper',
            regionSortBlock: '.bcms-sorting-block',
            regionTreeButtons: '.bcms-region-contentstree',

            masterPagesPathContainer: '.bcms-layout-path',
            masterPagesPathHandler: '.bcms-layout-path-handle',
            masterPagesPathItem: '.bcms-layout-path-item',
            masterPagesPathInnerContainer: '.bcms-layout-path-inner',
            masterPagesPathSliderLeft: '.bcms-path-arrow-left',
            masterPagesPathSliderRight: '.bcms-path-arrow-right'
        },
        classes = {
            regionStart: 'bcms-region-start',
            regionEnd: 'bcms-region-end',
            contentStart: 'bcms-content-start',
            contentEnd: 'bcms-content-end',
            regionSortOverlay: 'bcms-show-overlay',
            masterPagesPathToggler: 'bcms-path-toggler',
            masterPagesPathInactiveArrow: 'bcms-path-arrow-inactive',
            masterPagesPathItem: 'bcms-layout-path-item',
            masterPagesPathChildContentItem: 'bcms-path-child-content',
            masterPagesPathChildContentActiveItem: 'bcms-path-child-content-active',
            masterPagesPathPageItem: 'bcms-path-page'
        },
        keys = {
            showMasterPagesPath: 'bcms.showMasterPagesPath',
        },
        resizeTimer,
        currentContentDom,
        regionRectangles = $(),
        contentRectangles = $(),
        links = {},
        globalization = {
            showMasterPagesPath: null,
            hideMasterPagesPath: null,
            currentPage: null
        },
        pageViewModel,
        opacityAnimationSpeed = 50,
        isSortMode = false,
        masterPagesModel = null;

    // Assign objects to module
    content.selectors = selectors;
    content.links = links;
    content.globalization = globalization;

    /**
    * Shows overlay over content region:
    */
    content.showOverlay = function (contentViewModel) {
        var overlay = contentViewModel.overlay;

        overlay.animate({ 'opacity': 1 }, opacityAnimationSpeed);
    };

    /**
    * Forces each regoin and content outline to update it's position
    */
    content.refreshOverlays = function () {
        content.refreshRegionsPosition();
        content.refreshContentsPosition();
    };

    /**
    * Forces each content outline to update it's position:
    */
    content.refreshContentsPosition = function () {

        if (!bcms.editModeIsOn() || pageViewModel == null) {
            return;
        }

        $.each(pageViewModel.contents, function () {
            if (this.isInvisible) {
                return;
            }

            if (!pageViewModel.isContentVisible(this)) {
                this.overlay.hide();

                return;
            } else {
                this.overlay.show();
            }

            this.recalculatePositions();
        });

        contentRectangles.each(function () {

            var rectangle = $(this),
                contentViewModel = rectangle.data('target'),
                width = contentViewModel.width + 'px',
                height = contentViewModel.height,
                top = contentViewModel.top + 'px',
                left = contentViewModel.left + 'px';

            if (height < 20) {
                height = 20;
            }

            rectangle.css({
                'width': width,
                'height': height + 'px',
                'top': top,
                'left': left,
                'opacity': 0
            });
        });
    };

    /**
    * Forces each region outline to update it's position:
    */
    content.refreshRegionsPosition = function (regionViewModels) {

        if (!bcms.editModeIsOn() || pageViewModel == null) {
            return;
        }

        regionViewModels = regionViewModels || pageViewModel.regions;

        $.each(regionViewModels, function () {
            if (this.isInvisible) {
                return;
            }

            if (!pageViewModel.isRegionVisible(this)) {
                this.overlay.hide();

                return;
            } else {
                this.overlay.show();
            }

            this.recalculatePositions();
        });

        $.each(regionViewModels, function () {
            if (this.isInvisible || !pageViewModel.isRegionVisible(this)) {
                return;
            }

            var regionViewModel = this,
                overlay = regionViewModel.overlay,
                width = regionViewModel.width,
                height = regionViewModel.height;

            overlay.css({
                top: regionViewModel.top + 'px',
                left: regionViewModel.left + 'px',
                width: width + 'px',
                height: height + 'px'
            });
        });
    };

    /**
    * Hides content overlay:
    */
    content.hideOverlay = function (contentViewModel) {
        currentContentDom = null;

        contentViewModel.overlay.animate({ 'opacity': 0 }, opacityAnimationSpeed);
    };

    /**
    * Saves content sorting and other changes
    */
    content.saveContentChanges = function (regionViewModels, onSuccess) {
        var models = [],
            i, l, regionViewModel;

        for (i = 0, l = regionViewModels.length; i < l; i++) {
            regionViewModel = regionViewModels[i];

            $.each(regionViewModel.contents, function () {

                this.parentContent = regionViewModel.parentContent;
                this.parentPageContentId = regionViewModel.parentPageContentId;
                this.region = regionViewModel;

                models.push({
                    'RegionId': regionViewModel.id,
                    'PageContentId': this.pageContentId,
                    'ParentPageContentId': this.parentPageContentId,
                    'Version': this.pageContentVersion
                });
            });
        }

        bcms.trigger(bcms.events.sortPageContent, {
            models: models,
            onSuccess: onSuccess
        });
    };

    /**
    * Checks if contents order has changed
    */
    content.hasContentsOrderChanged = function (before, after) {
        if (before.length != after.length) {
            return true;
        }

        for (var i = 0; i < before.length; i++) {
            if (!after[i] || after[i].contentId != before[i].contentId) {
                return true;
            }
        }

        return false;
    };

    /**
    * Turns region content sorting mode OFF:
    */
    content.turnSortModeOff = function (cancel, leaveSortModeOpen) {

        var changedRegions = [];

        $.each(pageViewModel.regions, function () {
            if (this.isInvisible || !pageViewModel.isRegionVisible(this)) {
                return;
            }

            var regionContents = [],
                regionViewModel = this;

            if (!leaveSortModeOpen) {
                $(selectors.regionButtons, regionViewModel.overlay).show();
                $(selectors.regionSortDoneButtons, regionViewModel.overlay).hide();
                $(selectors.regionSortCancelButtons, regionViewModel.overlay).hide();
                $(selectors.regionTreeButtons, regionViewModel.overlay).hide();

                if (isSortMode) {
                    regionViewModel.sortBlock.sortable('destroy');
                }
                regionViewModel.overlay.removeClass(classes.regionSortOverlay);
            }

            $(selectors.regionSortWrappers, regionViewModel.overlay).each(function () {
                var viewModel = $(this).data('target');
                regionContents.push(viewModel);

                if (!leaveSortModeOpen) {
                    $(this).remove();
                }
            });

            if (!cancel) {
                if (content.hasContentsOrderChanged(regionViewModel.contents, regionContents)) {
                    changedRegions.push(regionViewModel);
                    regionViewModel.changedContents = regionContents;
                }
                if (!leaveSortModeOpen) {
                    regionViewModel.setContents(regionContents);
                }
            }

            if (!leaveSortModeOpen) {
                $.each(regionContents, function () {
                    if (this.isInvisible) {
                        return;
                    }

                    this.overlay.show();
                });
            }
        });

        pageViewModel.currentParentContent = null;
        masterPagesModel.removeParentContents();

        if (!leaveSortModeOpen) {
            content.refreshOverlays();
            isSortMode = false;
        }

        return changedRegions;
    };

    /**
    * Turns region content sorting mode ON:
    */
    content.turnSortModeOn = function() {
        isSortMode = true;

        $.each(pageViewModel.regions, function () {
            if (this.isInvisible || !pageViewModel.isRegionVisible(this)) {
                return;
            }

            var regionViewModel = this;

            $(selectors.regionButtons, regionViewModel.overlay).hide();
            $(selectors.regionSortDoneButtons, regionViewModel.overlay).show();
            $(selectors.regionSortCancelButtons, regionViewModel.overlay).show();
            $(selectors.regionTreeButtons, regionViewModel.overlay).show();

            $(regionViewModel.contents).each(function () {
                var sortWrapper = $('<div class="bcms-sort-wrapper" />');

                $('<div class="bcms-sort-content" />').html(this.title).appendTo(sortWrapper);
                sortWrapper.append('<div class="bcms-sort-overlay bcms-content-overlaybg" />');

                // Store reference to content so it can be sorted later:
                sortWrapper.data('target', this);

                this.overlay.hide();

                regionViewModel.sortBlock.append(sortWrapper);
            });

            regionViewModel.sortBlock.sortable({
                connectWith: '.bcms-sorting-block',
                dropOnEmpty: true,
                placeholder: "bcms-sort-wrapper-placeholder",
                tolerance: "intersect"
            });

            regionViewModel.overlay.addClass(classes.regionSortOverlay);
        });

        content.refreshRegionsPosition();
    };

    /**
    * Function calculates top, left positions and width and height for specified list of DOM elements
    */
    function calculatePositions(start, end) {
        var $start = $(start),
            $end = $(end),
            startOffset = $start.offset() || {},
            endOffset = $end.offset() || {},
            endWidth = $end.outerWidth(true),
            endHeight = $end.outerHeight(true),
            top = startOffset.top,
            left = startOffset.left,
            right = endOffset.left + endWidth,
            bottom = endOffset.top + endHeight;

        return {
            left: left,
            top: top,
            width: right - left,
            height: bottom - top
        };
    }

    /**
    * Page view model
    */
    function PageViewModel() {
        var self = this;

        self.regions = [];
        self.contents = [];
        self.currentParentContent = null;

        self.isRegionVisible = function (regionViewModel) {
            return regionViewModel.parentContent == self.currentParentContent;
        };

        self.isContentVisible = function (contentViewModel) {
            return contentViewModel.parentContent == self.currentParentContent;
        };
    }

    /**
    * Page region view model
    */
    content.RegionViewModel = function(regionStart, regionEnd, regionContents, parentRegionId, parentPageContentId) {
        var self = this;

        self.id = null;
        self.title = null;
        self.regionStart = regionStart;
        self.regionEnd = regionEnd;
        self.contents = regionContents;
        self.changedContents = [];
        self.overlay = null;
        self.sortBlock = null;
        self.parentRegionId = parentRegionId;
        self.parentRegion = null;
        self.parentPageContentId = parentPageContentId;
        self.parentContent = null;
        self.isInvisible = false;

        if (self.regionStart) {
            self.isInvisible = self.regionStart.data("invisible") === true;
            self.id = self.regionStart.data('id');
            self.title = self.regionStart.data('identifier');
        }

        self.left = 0;
        self.top = 0;
        self.width = 0;
        self.height = 0;

        self.recalculatePositions = function() {
            var positions = calculatePositions(self.regionStart, self.regionEnd);

            self.left = positions.left;
            self.top = positions.top;
            self.width = positions.width;
            self.height = positions.height;

            if (isSortMode) {
                for (var i = 0; i < self.contents.length; i++) {
                    var sortPositions = calculatePositions(self.regionStart, self.contents[i].contentEnd);
                    if (sortPositions.height > self.height) {
                        self.height = sortPositions.height;
                    }
                }
            }
        };

        self.initializeRegion = function() {

            if (self.parentRegionId && !self.parentRegion) {
                for (i = 0; i < pageViewModel.regions.length; i++) {
                    if (pageViewModel.regions[i].id == self.parentRegionId) {
                        self.parentRegion = pageViewModel.regions[i];

                        break;
                    }
                }
            }

            if (self.parentPageContentId && !self.parentContent) {
                for (i = 0; i < pageViewModel.contents.length; i++) {
                    if (pageViewModel.contents[i].pageContentId == self.parentPageContentId) {
                        self.parentContent = pageViewModel.contents[i];
                        break;
                    }
                }
            }

            self.onAddContent = function(onSuccess, includeChildRegions) {
                bcms.trigger(bcms.events.addPageContent, {
                    regionViewModel: self,
                    onSuccess: onSuccess,
                    includeChildRegions: includeChildRegions
                });
            };

            if (self.isInvisible) {
                return;
            }

            var container = $(selectors.regionOverlay),
                template = container.html(),
                rectangle = $(template),
                i;

            rectangle.data('target', self);
            rectangle.insertBefore(container);
            regionRectangles = regionRectangles.add(rectangle);

            if (bcms.editModeIsOn()) {
                rectangle.show();
            }

            self.overlay = rectangle;
            self.sortBlock = this.overlay.find(selectors.regionSortBlock);

            $(selectors.regionAddContentButtons, self.overlay).on('click', function() {
                self.onAddContent();
            });

            $(selectors.regionSortButtons, self.overlay).on('click', function() {
                content.turnSortModeOn(self);
            });

            $(selectors.regionTreeButtons, self.overlay).on('click', function() {
                bcms.trigger(bcms.events.editContentsTree, {
                    pageViewModel: pageViewModel,
                    regionViewModel: self
                });
            });

            $(selectors.regionSortDoneButtons, self.overlay).on('click', function() {
                var changedRegions = content.turnSortModeOff();

                if (changedRegions.length > 0) {
                    content.saveContentChanges(changedRegions);
                }
            });

            $(selectors.regionSortCancelButtons, self.overlay).on('click', function() {
                content.cancelSortMode();
            });
        };

        self.setContents = function(changedContents) {
            self.contents = changedContents;
        };
    };

    /**
    * Page content view model
    */
    content.ContentViewModel = function (contentStart, contentEnd, parentPageContentId) {
        var self = this;

        self.contentStart = contentStart;
        self.contentEnd = contentEnd;
        self.overlay = null;
        self.region = null;
        self.parentPageContentId = parentPageContentId;
        self.parentContent = null;
        self.childRegions = null;
        self.hideEndingDiv = false;
        self.isInvisible = false;
        self.draft = false;

        self.title = null;
        self.contentId = null;
        self.pageContentId = null;
        self.contentVersion = null;
        self.pageContentVersion = null;
        self.contentType = null;

        if (contentEnd) {
            self.hideEndingDiv = contentEnd.data('hide') === true;
        }

        if (contentStart) {
            self.contentId = contentStart.data('contentId');
            self.pageContentId = contentStart.data('pageContentId');
            self.contentVersion = contentStart.data('contentVersion');
            self.pageContentVersion = contentStart.data('pageContentVersion');
            self.contentType = contentStart.data('contentType');
            self.draft = contentStart.data('draft');
            self.title = contentStart.data('contentTitle');
            self.isInvisible = contentStart.data("invisible") === true;
        }

        self.left = 0;
        self.top = 0;
        self.width = 0;
        self.height = 0;

        self.visibleButtons = {
            'configure': true,
            'edit': true,
            'history': true,
            'delete': true,
            'enterChildContent': true,
            'draft': false
        };

        self.initializeContent = function () {

            // Setup parent content
            if (self.parentPageContentId && !self.parentContent) {
                for (var i = 0; i < pageViewModel.contents.length; i++) {
                    if (pageViewModel.contents[i].pageContentId == self.parentPageContentId) {
                        self.parentContent = pageViewModel.contents[i];
                        break;
                    }
                }
            }

            bcms.trigger(bcms.events.contentModelCreated, self);

            if (self.isInvisible) {
                return;
            }

            var container = $(selectors.contentOverlay),
                template = container.html(),
                rectangle = $(template);

            rectangle.data('target', self);
            rectangle.insertBefore(container);
            contentRectangles = contentRectangles.add(rectangle);

            if (bcms.editModeIsOn()) {
                rectangle.show();
            }

            self.overlay = rectangle;

            if (self.getChildRegions().length == 0) {
                self.visibleButtons.enterChildContent = false;
            }

            showHideButtons();

            $(selectors.contentDelete, rectangle).on('click', function () {
                self.onDeleteContent();
            });

            $(selectors.contentEdit, rectangle).on('click', function () {
                self.onEditContent();
            });

            $(selectors.contentHistory, rectangle).on('click', function () {
                self.onContentHistory();
            });

            $(selectors.contentConfigure, rectangle).on('click', function () {
                self.onConfigureContent();
            });

            $(selectors.enterChildContent, rectangle).on('click', function () {
                self.onEnterChildContent();
            });

            rectangle.on('mouseleave', function () {
                bcms.logger.trace('Content mouse leave');
                content.hideOverlay(self);
            });

            rectangle.on('mouseover', function () {
                if (!bcms.editModeIsOn() || currentContentDom === rectangle) {
                    bcms.logger.trace('Exit content mouse over');
                    return;
                }

                bcms.logger.trace('Content mouse over');
                currentContentDom = rectangle;
                content.showOverlay(self);
            });

            if (!bcms.editModeIsOn() && self.hideEndingDiv) {
                self.contentEnd.hide();
            }
        };

        self.recalculatePositions = function () {
            var positions = calculatePositions(self.contentStart, self.contentEnd);

            self.left = positions.left + 1;
            self.top = positions.top + 1;
            self.width = positions.width;
            self.height = positions.height;
        };

        self.onEditContent = function () { };
        self.onDeleteContent = function () { };
        self.onConfigureContent = function () { };
        self.onContentHistory = function () { };

        self.onEnterChildContent = function () {
            pageViewModel.currentParentContent = self;

            content.refreshOverlays();

            if (masterPagesModel != null) {
                masterPagesModel.calculatePathPositions();
                masterPagesModel.addParentContent(self.title, null, function () {
                    pageViewModel.currentParentContent = self;

                    content.refreshOverlays();
                });
            }
        };

        self.getChildRegions = function () {
            if (self.childRegions == null) {
                self.childRegions = [];

                for (var i = 0; i < pageViewModel.regions.length; i++) {
                    if (pageViewModel.regions[i].parentContent == self) {
                        self.childRegions.push(pageViewModel.regions[i]);
                    }
                }
            }

            return self.childRegions;
        };

        function showHideButtons() {
            if (!self.visibleButtons.configure) {
                self.overlay.find(selectors.contentConfigure).remove();
            }
            if (!self.visibleButtons.edit) {
                self.overlay.find(selectors.contentEdit).remove();
            }
            if (!self.visibleButtons.history) {
                self.overlay.find(selectors.contentHistory).remove();
            }
            if (!self.visibleButtons["delete"]) {
                self.overlay.find(selectors.contentDelete).remove();
            }
            if (!self.visibleButtons.enterChildContent) {
                self.overlay.find(selectors.enterChildContent).remove();
            }
            if (self.visibleButtons.draft) {
                self.overlay.find(selectors.contentEditInnerDiv).html('<div>*</div>');
            }
        }

        return self;
    };

    function collectRegionsAndContents(tags) {
        var tagsCount = tags.length,
            currentTag,
            i,
            allContents = [],
            allRegions = [],
            regionId,
            pageContentId,
            currentContent,
            currentRegion,
            parentRegionId,
            parentPageContentId;

        for (i = 0; i < tagsCount; i++) {
            currentTag = $(tags[i]);

            if (currentTag.hasClass(classes.regionStart)) {
                // Region start
                regionId = currentTag.data('id');
                currentRegion = {
                    regionId: regionId,
                    contents: [],
                    startTag: currentTag
                };
                allRegions.push(currentRegion);
            } else if (currentTag.hasClass(classes.regionEnd)) {
                // Region end
                currentRegion = allRegions.pop();
                parentRegionId = null;
                parentPageContentId = null;
                if (allRegions.length > 0) {
                    parentRegionId = allRegions[allRegions.length - 1].regionId;
                    parentPageContentId = allContents[allContents.length - 1].pageContentId;
                }

                var regionViewModel = new content.RegionViewModel(currentRegion.startTag, currentTag, currentRegion.contents, parentRegionId, parentPageContentId);
                pageViewModel.regions.push(regionViewModel);

                $.each(currentRegion.contents, function () {
                    pageViewModel.contents.push(this);
                    this.region = regionViewModel;
                });
            } else if (currentTag.hasClass(classes.contentStart)) {
                // Content start
                pageContentId = currentTag.data('pageContentId');
                currentContent = {
                    pageContentId: pageContentId,
                    startTag: currentTag
                };
                allContents.push(currentContent);
            } else if (currentTag.hasClass(classes.contentEnd)) {
                // Content end
                currentContent = allContents.pop();
                currentRegion = allRegions[allRegions.length - 1];
                parentPageContentId = null;
                if (allContents.length > 0) {
                    parentPageContentId = allContents[allContents.length - 1].pageContentId;
                }

                var contentViewModel = new content.ContentViewModel(currentContent.startTag, currentTag, parentPageContentId);
                currentRegion.contents.push(contentViewModel);
            }
        }
    }

    /**
    * Initializes events for regions:
    */
    content.initRegions = function () {
        bcms.logger.trace('Highlight regions');

        pageViewModel = new PageViewModel();

        // TODO: remove after tests
        window.cms.page = pageViewModel;
        window.cms.path = masterPagesModel;

        var tags = $(selectors.regionsAndContents).toArray();
        collectRegionsAndContents(tags, 0);

        $.each(pageViewModel.regions, function () {
            this.initializeRegion();
        });

        $.each(pageViewModel.contents, function () {
            this.initializeContent();
        });

        content.refreshOverlays();

        $(window).on('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {
                content.refreshOverlays();

                if (masterPagesModel != null) {
                    masterPagesModel.calculatePathPositions();
                }
            }, 100);
        });
    };

    /**
    * Cancels sort mode after the 'editModeOff' event is triggered.
    */
    content.cancelSortMode = function () {
        content.turnSortModeOff(true);

        bcms.logger.trace('Cancel Sort Mode');
    };

    /**
    * Occurs when edit mode is turned off
    */
    function onEditModeOff() {
        content.cancelSortMode();

        if (pageViewModel != null) {
            $.each(pageViewModel.contents, function () {
                if (this.isInvisible) {
                    return;
                }

                this.overlay.hide();

                if (this.hideEndingDiv) {
                    this.contentEnd.hide();
                }
            });

            $.each(pageViewModel.regions, function () {
                if (this.isInvisible) {
                    return;
                }

                this.overlay.hide();
            });
        }
    }

    /**
    * Occurs when edit mode is turned on
    */
    function onEditModeOn() {
        if (pageViewModel != null) {
            $.each(pageViewModel.contents, function () {
                if (this.isInvisible || !pageViewModel.isContentVisible(this)) {
                    return;
                }

                this.overlay.show();

                if (this.hideEndingDiv) {
                    this.contentEnd.show();
                }
            });
        }

        content.refreshOverlays();
        content.refreshMasterPagesPath();
    }

    /**
    * Master page path item view model
    */
    function PathViewModel(element, index) {
        var self = this;

        self.element = element;
        self.index = index;

        return self;
    }

    /**
    * View model for controling the path of master pages
    */
    function MasterPagesPathModel() {
        var self = this,
            pathContainer = $(selectors.masterPagesPathContainer),
            innerContainer = pathContainer.find(selectors.masterPagesPathInnerContainer),
            handle = pathContainer.find(selectors.masterPagesPathHandler),
            leftSlider = pathContainer.find(selectors.masterPagesPathSliderLeft),
            rightSlider = pathContainer.find(selectors.masterPagesPathSliderRight),
            items = [],
            currentItem = 0,
            maxItem = 0,
            currentPage = null;

        function hasPath() {
            return items.length > 0;
        }

        function getPathVisibility() {
            var showPage = localStorage.getItem(keys.showMasterPagesPath);

            if (showPage === undefined) {
                showPage = 1;
                setPathVisibility(showPage);
            }

            return showPage;
        }

        function setPathVisibility(isVisible, doNotChangeUpdateStorage) {
            if (!doNotChangeUpdateStorage) {
                localStorage.setItem(keys.showMasterPagesPath, isVisible);
            }

            if (isVisible == 1) {
                pathContainer.removeClass(classes.masterPagesPathToggler);
                handle.html(globalization.hideMasterPagesPath);
            } else {
                pathContainer.addClass(classes.masterPagesPathToggler);
                handle.html(globalization.showMasterPagesPath);
            }
        }

        function onHandleClick() {
            if (pathContainer.hasClass(classes.masterPagesPathToggler)) {
                setPathVisibility(1);
            } else {
                setPathVisibility(0);
            }
        }

        function slide(step) {
            var itemNr = currentItem + step,
                length = items.length,
                margin,
                i,
                margins = [];

            if (itemNr >= 0 && itemNr <= maxItem) {
                currentItem += step;
                for (i = 0; i < length; i++) {
                    if (i < currentItem) {
                        margin = -items[i].element.outerWidth();
                    } else {
                        margin = 0;
                    }

                    margins.push({
                        item: i,
                        margin: margin
                    });
                }
            }

            for (i = 0; i < margins.length; i++) {
                items[margins[i].item].element.css('margin-left', margins[i].margin);
            }

            updateSliders();
        }

        function updateSliders() {
            if (currentItem > 0) {
                leftSlider.removeClass(classes.masterPagesPathInactiveArrow);
            } else {
                leftSlider.addClass(classes.masterPagesPathInactiveArrow);
            }

            if (currentItem < maxItem) {
                rightSlider.removeClass(classes.masterPagesPathInactiveArrow);
            } else {
                rightSlider.addClass(classes.masterPagesPathInactiveArrow);
            }
        }

        function slideToTheFirstParent() {
            if (!hasPath()) {
                return;
            }

            var width = innerContainer.width(),
                length = items.length,
                sum = items[length - 1].element.outerWidth(),
                i,
                slidesToLeave = 0,
                step;

            for (i = length - 2; i >= 0; i--) {
                sum += items[i].element.outerWidth();
                if (sum <= width) {
                    slidesToLeave++;
                }
                bcms.logger.trace('Sliding to first: slidesToLeave: ' + slidesToLeave + ' sum: ' + sum + ' currentItem: ' + items[i].element.outerWidth() + ' width: ' + width);
            }

            maxItem = length - 1 - slidesToLeave;
            bcms.logger.trace('Slide to: ' + maxItem);
            step = maxItem - currentItem;
            if (step > 0) {
                slide(step);
            } else {
                updateSliders();
            }
        }

        function redraw(onAfterCalculate) {
            if (!hasPath()) {
                pathContainer.hide();

                return;
            } else {
                pathContainer.show();
            }

            self.calculatePathPositions(onAfterCalculate);
        }

        self.calculatePathPositions = function (onAfterCalculate) {
            if (!hasPath()) {
                return;
            }

            var ww = $(window).width(),
            cw = ww * 0.8,
            totalItemsWidth = leftSlider.outerWidth() + leftSlider.outerWidth() + 30;

            $.each(items, function (index) {
                totalItemsWidth += items[index].element.outerWidth();
                console.log('Item: ' + items[index].element.outerWidth() + '; total: ' + totalItemsWidth + '; cw: ' + cw);
            });

            pathContainer.css('width', cw > totalItemsWidth ? totalItemsWidth : cw);
            pathContainer.css('left', ww / 2);
            pathContainer.css('margin-left', cw > totalItemsWidth ? totalItemsWidth / -2 : cw / -2);

            if ($.isFunction(onAfterCalculate)) {
                onAfterCalculate();
            }
        };

        self.initialize = function () {
            setPathVisibility(0, true);

            pathContainer.find(selectors.masterPagesPathItem).each(function (index) {
                var item = $(this),
                    html = item.html();

                if (html.length > 50) {
                    item.attr('title', html);
                    html = html.substr(0, 50) + '...';
                    item.html(html);
                }
                items.push(new PathViewModel(item, index));

                item.on('click', function () {
                    var url = $(this).data('url');

                    window.location.href = url;
                });
            });

            handle.on('click', onHandleClick);

            leftSlider.on('click', function () {
                slide(-1);
            });
            rightSlider.on('click', function () {
                slide(1);
            });

            redraw(slideToTheFirstParent);
            self.calculatePathPositions();
            setPathVisibility(getPathVisibility());
        };

        function setLastParentInactive() {
            var lastItem = items[items.length - 1].element;
            if (lastItem.hasClass(classes.masterPagesPathChildContentActiveItem)) {
                lastItem.removeClass(classes.masterPagesPathChildContentActiveItem);
                lastItem.addClass(classes.masterPagesPathChildContentItem);
            }
        }

        function setLastParentActive() {
            var lastItem = items[items.length - 1].element;
            if (lastItem.hasClass(classes.masterPagesPathChildContentItem)) {
                lastItem.removeClass(classes.masterPagesPathChildContentItem);
                lastItem.addClass(classes.masterPagesPathChildContentActiveItem);
            }
        }

        self.addParentContent = function (title, cssClass, onClick) {

            if (!currentPage) {
                currentPage = true;
                currentPage = self.addParentContent(globalization.currentPage, classes.masterPagesPathPageItem, function () {
                    items.pop();
                    currentPage.remove();
                    currentPage = null;

                    pageViewModel.currentParentContent = null;

                    content.refreshOverlays();
                });
            }

            var div = $('<div></div>'),
                index = items.length,
                model = new PathViewModel(div, index),
                onItemClick = function () {
                    var currentItemIndex = $(this).data('index'),
                        total = items.length,
                        i,
                        item;

                    if (currentItemIndex == total - 1) {
                        return;
                    }

                    for (i = currentItemIndex + 1; i < total; i++) {
                        item = items.pop();
                        item.element.remove();
                    }

                    setLastParentActive();

                    if ($.isFunction(onClick)) {
                        onClick();
                    }

                    redraw();
                };

            div.addClass(classes.masterPagesPathItem);
            if (cssClass) {
                div.addClass(cssClass);
            } else {
                setLastParentInactive();
                div.addClass(classes.masterPagesPathChildContentActiveItem);
            }
            div.html(title);
            div.data('index', index);
            div.on('click', onItemClick);

            innerContainer.append(div);
            items.push(model);

            redraw(slideToTheFirstParent);
            setPathVisibility(1);

            return div;
        };

        self.removeParentContents = function() {
            if (currentPage && $.isFunction(currentPage.click)) {
                currentPage.click();
            }
        };

        return self;
    }

    /**
    * Recalculates and shows / hides master page path
    */
    content.refreshMasterPagesPath = function () {
        if (masterPagesModel != null) {
            masterPagesModel.calculatePathPositions();
        }
    };

    /**
    * Initializes contents module.
    */
    content.init = function () {
        bcms.logger.debug('Initializing content module');

        masterPagesModel = new MasterPagesPathModel();
        masterPagesModel.initialize();

        content.initRegions();
    };

    /**
    * Subscribe to event, so that when edit mode is turned off cancel sort mode
    */
    bcms.on(bcms.events.editModeOff, onEditModeOff);
    bcms.on(bcms.events.editModeOn, onEditModeOn);

    /**
    * Register initialization
    */
    bcms.registerInit(content.init);

    return content;
});