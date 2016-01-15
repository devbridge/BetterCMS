/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.forms.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.forms', ['bcms.jquery', 'bcms', 'bcms.messages', 'bcms.tabs'], function ($, bcms, messages, tabs) {
    'use strict';

    var forms = {},

    // Selectors used in the module to locate DOM elements:
        selectors = {
            ajaxHide: '.bcms-ajax-hide',
            ajaxShow: '.bcms-ajax-show',

            firstInvalidField: '.bcms-input-validation-error:first',
            
            checkboxLabels: '.bcms-checkbox-holder:has(input[type="checkbox"]) .bcms-js-edit-label, .bcms-edit-check-field:has(input[type="checkbox"]) .bcms-edit-label',
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
                    bcms.logger.error('Failed to select not valid' + ex.message);
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
            complete: null,

            contentType: 'application/x-www-form-urlencoded',
            dataType: 'json',
            serialize: function(form) {
                return form.serialize();
            }
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
                if (options.beforeSubmit() === false) {
                    form.data(submitting, false);
                    return false;
                };
            }

            ajaxLoadingHide.hide();
            ajaxLoadingShow.show();

            $.ajax({
                type: 'POST',
                contentType: options.contentType,
                dataType: options.dataType,
                cache: false,
                url: form.attr('action'),
                data: options.serialize(form)
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

    forms.setFieldsReadOnly = function (form) {
        form.find('input:visible').attr('readonly', 'readonly');
        form.find('textarea:visible').attr('readonly', 'readonly');
        form.find('input[type=text]:visible:not([data-bind])').parent('div').css('z-index', 100);
        form.find('textarea:visible:not([data-bind])').attr('readonly', 'readonly').parent('div').css('z-index', 100);
    };

    forms.serializeToObject = function (form, skipNulls) {
        var o = {},
            a = form.serializeArray();

        $.each(a, function () {
            var arrayStart = this.name.indexOf('['),
                arrayEnd = this.name.indexOf(']'),
                propertyStart = this.name.indexOf('.'),
                propertyName,
                parentObject,
                arrayName,
                arrayIndex,
                length;

            if (arrayStart > 0 && arrayEnd - arrayStart > 1) {
                // If array
                arrayName = this.name.substr(0, arrayStart);
                if (!o[arrayName]) {
                    o[arrayName] = [];
                }

                arrayIndex = parseInt(this.name.substr(arrayStart + 1, arrayEnd - arrayStart - 1));
                length = o[arrayName].length;

                for (var i = length; i < arrayIndex + 1; i++) {
                    o[arrayName].push({});
                }

                propertyStart = this.name.indexOf('.');
                if (propertyStart == arrayEnd + 1) {
                    propertyName = this.name.substr(propertyStart + 1, this.name.length - arrayEnd);
                    o[arrayName][arrayIndex][propertyName] = this.value;
                } else {
                    o[arrayName][arrayIndex] = this.value;
                }

                return;
            } else if (propertyStart > 0 && propertyStart < this.name.length - 1) {
                parentObject = this.name.substr(0, propertyStart);
                propertyName = this.name.substr(propertyStart + 1, this.name.length);
                if (!o[parentObject]) {
                    o[parentObject] = {};
                }
                o[parentObject][propertyName] = this.value || '';

                return;
            } else if (skipNulls && (this.value === undefined || this.value === null || this.value === '')) {
                // If skipping null value
                return;
            }
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });

        return o;
    };

    return forms;
});
