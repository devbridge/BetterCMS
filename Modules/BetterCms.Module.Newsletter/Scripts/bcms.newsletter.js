/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */
bettercms.define('bcms.newsletter', ['bcms.jquery', 'bcms', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.antiXss'],
    function ($, bcms, siteSettings, dynamicContent, ko, kogrid, antiXss) {
        'use strict';

        var newsletter = {},
            selectors = {},
            links = {
                loadSiteSettingsSubscribersUrl: null,
                loadSubscribersUrl: null,
                saveSubscriberUrl: null,
                deleteSubscriberUrl: null
            },
            globalization = {
                deleteSubscriberDialogTitle: null
            };

        /**
        * Assign objects to module.
        */
        newsletter.links = links;
        newsletter.globalization = globalization;
        newsletter.selectors = selectors;

        /**
        * Subscribers list view model
        */
        var SubscribersListViewModel = (function (_super) {

            bcms.extendsClass(SubscribersListViewModel, _super);

            function SubscribersListViewModel(container, items, gridOptions) {
                _super.call(this, container, links.loadSubscribersUrl, items, gridOptions);
            }

            SubscribersListViewModel.prototype.createItem = function (item) {
                var newItem = new SubscriberViewModel(this, item);
                return newItem;
            };

            return SubscribersListViewModel;

        })(kogrid.ListViewModel);

        /**
        * Subscriber view model
        */
        var SubscriberViewModel = (function (_super) {

            bcms.extendsClass(SubscriberViewModel, _super);

            function SubscriberViewModel(parent, item) {
                _super.call(this, parent, item);

                var self = this;

                self.email = ko.observable().extend({ required: "", email: "", maxLength: { maxLength: ko.maxLength.email } });

                self.registerFields(self.email);

                self.email(item.Email);
            }

            SubscriberViewModel.prototype.getDeleteConfirmationMessage = function () {
                return $.format(globalization.deleteSubscriberDialogTitle, antiXss.encodeHtml(this.email()));
            };

            SubscriberViewModel.prototype.getSaveParams = function () {
                var params = _super.prototype.getSaveParams.call(this);
                params.Email = this.email();

                return params;
            };

            return SubscriberViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Initializes loading of list of newsletters subscribers.
        */
        function initializeSiteSettingsNewsletterSubscribers(json) {
            var container = siteSettings.getMainContainer(),
                data = (json.Success == true) ? json.Data : {};

            var viewModel = new SubscribersListViewModel(container, data.Items, data.GridOptions);
            viewModel.deleteUrl = links.deleteSubscriberUrl;
            viewModel.saveUrl = links.saveSubscriberUrl;
            
            ko.applyBindings(viewModel, container.get(0));
            
            // Select search.
            var firstVisibleInputField = container.find('input[type=text],textarea,select').filter(':visible:first');
            if (firstVisibleInputField) {
                firstVisibleInputField.focus();
            }
        }

        /**
        * Loads a newsletters subscribers list to the site settings container.
        */
        newsletter.loadSiteSettingsNewsletterSubscribers = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsSubscribersUrl, {
                contentAvailable: initializeSiteSettingsNewsletterSubscribers
            });
        };

        /**
        * Initializes newsletter module.
        */
        newsletter.init = function () {
            bcms.logger.debug('Initializing bcms.newsletter module.');
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(newsletter.init);

        return newsletter;
    });