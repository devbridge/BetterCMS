/*global $, document*/

$(function () {
    'use strict';

    var parameters = {};

    /* POPUP INITIALIZATION */
    function initPopup() {
        var html = [];

        html.push('<div class="overlay" id="scrollbox" style="display:none;">');
        html.push('  <div class="pop-box">');
        html.push('    <a id="close-popup" class="remove"></a>');
        html.push('    <div id="popup-content"></div>');
        html.push('  </div>');
        html.push('</div>');

        $(html.join('')).appendTo('body');
    }

    /* SHOW POPUP FUNCTION */
    function showPopup(params) {
        var overlayContainer = $('#scrollbox');

        overlayContainer.show();

        $('.pop-box h3', overlayContainer).html(params.title || '');

        $('html').addClass('lockScreen');

        if (params.className) {
            $('div.pop-box').addClass(params.className);
        }
    }

    /* POPUP CLOSE */
    function closePopup(closeSelector) {
        $(closeSelector).on('click', function () {
            $(this).closest('div.overlay').hide();
            $('html').removeClass('lockScreen');
            $('div.pop-box').removeAttr('style');
            $('div#popup-content').empty();
        });
    }

    /* FEED DATA TO POPUP */
    function loadPopup(params) {
        var mainPopupContainer = $('div.pop-box'),
            mainContentContainer = $('#popup-content');

        if (params.url) {
            showPopup(params);
            $.ajax(params.url)
                .done(function (data) {
                    // populate popup with data from partial view
                    mainContentContainer.html(data);

                    //if width is defined in popup call, resize popup, else leave default css value
                    if (params.width) {
                        mainPopupContainer.css('width', params.width);
                    }

                    //if title in popup call is not empty, rewrite popup title
                    mainContentContainer.find('h3').text(params.title || '');

                }).fail(function () {
                    mainContentContainer.html('<p>Oops, error occurred while processing request.</p>');
                });
        } else {
            // if message attribute is set in popup call
            if (params.message) {
                showPopup(params);
                mainContentContainer.append('<p>' + params.message + '</p>');
                if (params.width) {
                    mainPopupContainer.css('width', params.width);
                }
            }

            closePopup('a.remove');
        }
    }

    /* BIND EVENTS ON POPUP TRIGGERS */
    function initPopupLinks(popupSelectors) {

        if (popupSelectors.length) {
            $(document).on('click', popupSelectors, function (e) {
                e.preventDefault();

                //get data for title, href, and custom width
                parameters.title = $(this).data('title') || $(this).attr('title');
                parameters.url = $(this).data('url') || $(this).attr("href");
                parameters.width = $(this).data('width');
                parameters.message = $(this).data('msg');
                parameters.className = $(this).data('class');

                // if width is undefined, set width to default size
                if (!parameters.width) {
                    parameters.width = 0;
                }
                if (!parameters.msg) {
                    parameters.msg = 0;
                }
                if (!parameters.className) {
                    parameters.className = 0;
                }
                if (!parameters.title) {
                    parameters.title = '';
                }

                loadPopup(parameters);
            });
        }
    }

    /* POPUP LAUNCH */
    function initializePopups() {
        initPopup();
        initPopupLinks('a.show-popup');
    }

    initializePopups();
});