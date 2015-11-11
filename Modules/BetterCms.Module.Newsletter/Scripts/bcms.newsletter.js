/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */
bettercms.define('bcms.newsletter', ['bcms.jquery', 'bcms', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.antiXss'],
    function ($, bcms, siteSettings, dynamicContent, ko, kogrid, antiXss) {
        'use strict';

        var newsletter = {},
            selectors = {
                downloadSubscribersInCsv: '#download-subscribers-in-csv',
                siteSettingsButtonOpener: ".bcms-btn-opener",
                siteSettingsButtonHolder: ".bcms-btn-opener-holder"
    },
            links = {
                loadSiteSettingsSubscribersUrl: null,
                loadSubscribersUrl: null,
                saveSubscriberUrl: null,
                deleteSubscriberUrl: null,
                downoadCsvUrl: null
            },
            globalization = {
                deleteSubscriberDialogTitle: null
            },
            rowId = 0;

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

            SubscriberViewModel.prototype.getRowId = function () {
                if (!this.rowId) {
                    this.rowId = 'bcms-subscriber-row-' + rowId++;
                }
                return this.rowId;
            };

            return SubscriberViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Initializes loading of list of newsletters subscribers.
        */
        function initializeSiteSettingsNewsletterSubscribers(json) {
            var container = siteSettings.getMainContainer(),
                data = (json.Success == true) ? json.Data : {},
                holder = container.find(selectors.siteSettingsButtonHolder);

            var viewModel = new SubscribersListViewModel(container, data.Items, data.GridOptions);
            viewModel.deleteUrl = links.deleteSubscriberUrl;
            viewModel.saveUrl = links.saveSubscriberUrl;

            ko.cleanNode(container.get(0));
            ko.applyBindings(viewModel, container.get(0));
            
            // Select search.
            var firstVisibleInputField = container.find('input[type=text],textarea,select').filter(':visible:first');
            if (firstVisibleInputField) {
                firstVisibleInputField.focus();
            }

            $(container.find(selectors.downloadSubscribersInCsv)).on('click', function() {
                window.location.href = links.downoadCsvUrl;
            });

            container.find(selectors.siteSettingsButtonOpener).on('click', function (event) {
                bcms.stopEventPropagation(event);
                if (!holder.hasClass('bcms-opened')) {
                    holder.addClass('bcms-opened');
                } else {
                    holder.removeClass('bcms-opened');
                }
            });

            bcms.on(bcms.events.bodyClick, function (event) {
                if (holder.hasClass('bcms-opened')) {
                    holder.removeClass('bcms-opened');
                }
            });

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