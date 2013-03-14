/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

/**
* Source taken from http://luke.breuer.com/tutorial/javascript-context-menu-tutorial.htm#firefox_option and modified by the DevBridge BetterCMS team.
*/

define('bcms.contextMenu', ['bcms.jquery', 'bcms'],
    function ($, bcms) {
        'use strict';

        var menu = { },
            selectors = {
                links: 'a'
            },
            links = {
                
            },
            globalization = {
                
            },
            classes = {
                topLeft: 'bcms-media-context-tl',
                bottomLeft: 'bcms-media-context-bl',
                topRight: 'bcms-media-context-tr',
                bottomRight: 'bcms-media-context-br'
            },
            
            /**
            * Replace the system context menu?
            */
            replaceContext = false,
            
            /**
            * Is the mouse over the context menu?
            */
            mouseOverContext = false,
            
            /**
            * Is context menu disabled?
            */
            noContext = false,

            /**
            * Context menu container
            */
            contextMenuContainer = null;

        /**
        * Assign objects to module.
        */
        menu.links = links;
        menu.globalization = globalization;
        menu.selectors = selectors;

        function setContextMenuContainer(container) {
            if (contextMenuContainer != null) {
                menu.closeContext();
            }
            contextMenuContainer = container;
        }

        /**
        * Init context menu
        */
        menu.initContext = function (menuContainer, targetContainer, bind) {
            setContextMenuContainer(menuContainer);

            menuContainer.on('mouseover', function () { mouseOverContext = true; });
            menuContainer.on('mouseout', function () { mouseOverContext = false; });

            menuContainer.find(selectors.links).on('click', menu.closeContext);

            if (bind === true) {
                targetContainer.on('contextmenu', function (event) {
                    menu.contextShow(event, null);
                });
            }
        };

        /**
        * Call from the onMouseDown event, passing the event if standards compliant
        */
        menu.contextMouseDown = function (event) {
            if (noContext || mouseOverContext)
                return;

            // IE is evil and doesn't pass the event object
            if (event == null)
                event = window.event;

            // only show the context menu if the right mouse button is pressed
            if (event.button == 2)
                replaceContext = true;
            else if (!mouseOverContext) {
                menu.closeContext();
            }
        };

        /**
        * Close context menu
        */
        menu.closeContext = function () {
            mouseOverContext = false;
            if (contextMenuContainer != null) {
                contextMenuContainer.hide();
            }
        };

        /**
        * Call from the onContextMenu event, passing the event
        * if this function returns false, the browser's context menu will not show up
        */
        menu.contextShow = function (event, menuContainer) {
            if (noContext || mouseOverContext)
                return true;

            // IE is evil and doesn't pass the event object
            if (event == null)
                event = window.event;

            if (replaceContext) {
                
                if (menuContainer) {
                    setContextMenuContainer(menuContainer);
                }

                // hide the menu first to avoid an "up-then-over" visual effect
                contextMenuContainer.hide();
                
                var leftPadding = 10,
                    topPadding = 14,
                    target = $(event.currentTarget),
                    left = event.pageX - target.offset().left,
                    top = event.pageY - target.offset().top,
                    scrollHeight = contextMenuContainer.scrollParent().height(),
                    scrollWidth = contextMenuContainer.scrollParent().width(),
                    scrollParentOffset = contextMenuContainer.scrollParent().offset(),
                    scrollTop = scrollParentOffset ? scrollParentOffset.top : 0,
                    scrollLeft = scrollParentOffset ? scrollParentOffset.left : 0,
                    menuHeight = contextMenuContainer.outerHeight(),
                    menuWidth = contextMenuContainer.outerWidth(),
                    parentOffset = contextMenuContainer.parent().offset(),
                    totalHeight = (parentOffset ? parentOffset.top : 0) + top + menuHeight - scrollTop,
                    totalWidth = (parentOffset ? parentOffset.left : 0) + left + menuWidth - scrollLeft + leftPadding,
                    addBottomClass = false,
                    addRightClass = false,
                    className;
                
                // Calculate top and left
                if (totalHeight > scrollHeight) {
                    top = top - menuHeight + topPadding;
                    addBottomClass = true;
                } else {
                    top -= topPadding;
                }

                if (totalWidth > scrollWidth) {
                    left = left - menuWidth - leftPadding;
                    addRightClass = true;
                } else {
                    left += leftPadding;
                }

                // Switch css class
                contextMenuContainer.removeClass(classes.bottomLeft);
                contextMenuContainer.removeClass(classes.bottomRight);
                contextMenuContainer.removeClass(classes.topLeft);
                contextMenuContainer.removeClass(classes.topRight);
                
                className = (addBottomClass)
                    ? (addRightClass ? classes.bottomRight : classes.bottomLeft)
                    : (addRightClass ? classes.topRight : classes.topLeft);
                contextMenuContainer.addClass(className);

                contextMenuContainer.css('left', left + 'px');
                contextMenuContainer.css('top', top + 'px');

                contextMenuContainer.css('z-index', bcms.getHighestZindex() + 1);
                contextMenuContainer.show();

                replaceContext = false;

                return false;
            }

            return true;
        };

        /**
        * Disables context menu
        */
        menu.disableContext = function () {
            noContext = true;
            menu.closeContext();

            return false;
        };

        /**
        * Enables context menu
        */
        menu.enableContext = function() {
            noContext = false;
            mouseOverContext = false; // this gets left enabled when "disable menus" is chosen

            return false;
        };

        /**
        * Initializes context menu module.
        */
        menu.init = function () {
            console.log('Initializing bcms.contextMenu module.');
            
            $(document).on('mousedown', menu.contextMouseDown);
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(menu.init);

        return menu;
    });