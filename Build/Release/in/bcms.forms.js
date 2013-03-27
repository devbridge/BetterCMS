/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.forms', ['bcms.jquery', 'bcms', 'bcms.messages', 'bcms.tabs'], function ($, bcms, messages, tabs) {
    'use strict';

    var forms = {},

    // Selectors used in the module to locate DOM elements:
        selectors = {
            ajaxHide: '.bcms-ajax-hide',
            ajaxShow: '.bcms-ajax-show',

            firstInvalidField: '.input-validation-error:first',
            
            checkboxLabels: '.bcms-checkbox-holder:has(input[type="checkbox"]) .bcms-edit-label, .bcms-edit-check-field:has(input[type="checkbox"]) .bcms-edit-label',
            checkboxParents: '.bcms-checkbox-holder, .bcms-edit-check-field',
            checkbox: 'input[type="checkbox"]'
        },
        submitting = 'submitting',
        links = {},
        globalization = {};

    // Assign objects to module
    forms.selectors = selectors;
    forms.links = links;
    forms.globalization = globalization;

    /**
    * Parses path to the JavaScript object/function:
    */
    forms.getObject = function (path) {
        if (!path) {
            return null;
        }

        var obj = window,
            parts = path.split('.');

        $.each(parts, function (i, part) {
            obj = obj[part];
            return !!obj;
        });

        return obj;
    };

    forms.valid = function (form) {
        if (form && form.valid) {
            if (form.valid()) {
                return true;
            } else {
                try {
                    var firstInvalidField = $(form).find(selectors.firstInvalidField);

                    // Check if first invalid field is on tab. 
                    var tabPanel = tabs.getTabPanelOfElement(firstInvalidField);
                    if (tabPanel) {
                        tabPanel.selectTabOfElement(firstInvalidField);
                    }
                } catch (ex) {
                    console.log('Failed to select not valid' + ex.message);
                }
                return false;
            }
        }
        return true;
    };

    forms.ajaxForm = function (formElement, options) {

        options = $.extend({
            beforeSubmit: null,
            success: null,
            error: null,
            complete: null
        }, options);

        $(formElement).on('submit', function () {
            var form = $(this);

            if (!forms.valid(form)) {
                return false;
            }

            // Prevent duplicate submission:
            if (form.data(submitting)) {
                return false;
            }

            var ajaxLoadingHide = form.find(selectors.ajaxHide),
                ajaxLoadingShow = form.find(selectors.ajaxShow);

            // Callback function expected after Ajax request is completed:
            var onComplete = function (json) {
                form.data(submitting, false);

                ajaxLoadingHide.show();
                ajaxLoadingShow.hide();

                messages.refreshBox(form, json);

                // Restore form state:
                if (json.Success) {
                    if ($.isFunction(options.success)) {
                        options.success(json);
                    }
                } else {
                    if ($.isFunction(options.error)) {
                        options.error(json);
                    }
                }

                if ($.isFunction(options.complete)) {
                    options.complete(json);
                }
            };

            // Mark form as submitting, to prevent duplicate submission:
            form.data(submitting, true);

            if ($.isFunction(options.beforeSubmit)) {
                options.beforeSubmit();
            }

            ajaxLoadingHide.hide();
            ajaxLoadingShow.show();

            $.ajax({
                type: 'POST',
                contentType: 'application/x-www-form-urlencoded',
                dataType: 'json',
                cache: false,
                url: form.attr('action'),
                data: form.serialize()
            })
                .done(function (response) {
                    onComplete(response);
                })

                .fail(function (response) {
                    onComplete(bcms.parseFailedResponse(response));
                });

            return false;
        });
    };

    forms.bindCheckboxes = function (container, options) {
        
        options = $.extend({
            checkboxSelector: selectors.checkbox,
            checkboxParentsSelector: selectors.checkboxParents,
            checkboxLabelsSelector: selectors.checkboxLabels
        }, options);

        container.find(options.checkboxLabelsSelector).on('click', function () {
            var checkBox = $(this).parents(options.checkboxParentsSelector).first().find(options.checkboxSelector);
            checkBox.trigger('click').trigger('change');
            return false;
        });
    };

    return forms;
});
