/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.role', ['bcms.jquery', 'bcms', 'bcms.autocomplete', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.antiXss'],
    function ($, bcms, autocomplete, ko, kogrid, antiXss) {
        'use strict';

        var role = {},
            selectors = {},
            links = {
                loadSiteSettingsRoleUrl: null,
                loadRolesUrl: null,
                saveRoleUrl: null,
                deleteRoleUrl: null,
                roleSuggestionServiceUrl: null
            },
            globalization = {
                rolesListTabTitle: null,
                deleteRoleConfirmMessage: null
            };

        // Assign objects to module.
        role.links = links;
        role.globalization = globalization;
        role.selectors = selectors;

        /**
        * Initializes roles list
        */
        role.initializeRolesList = function (container, json) {
            var data = (json.Success == true) ? json.Data : null,
                viewModel = new RolesListViewModel(container, data.Items, data.GridOptions);
            
            viewModel.deleteUrl = links.deleteRoleUrl;
            viewModel.saveUrl = links.saveRoleUrl;

            ko.applyBindings(viewModel, container.get(0));
        };

        /**
        * Roles list view model
        */
        var RolesListViewModel = (function (_super) {

            bcms.extendsClass(RolesListViewModel, _super);

            function RolesListViewModel(container, items, gridOptions) {
                _super.call(this, container, links.loadRolesUrl, items, gridOptions);
            }

            RolesListViewModel.prototype.createItem = function (item) {
                var newItem = new RoleViewModel(this, item);
                return newItem;
            };

            return RolesListViewModel;

        })(kogrid.ListViewModel);

        /**
        * Role view model
        */
        var RoleViewModel = (function (_super) {

            bcms.extendsClass(RoleViewModel, _super);

            function RoleViewModel(parent, item) {
                _super.call(this, parent, item);

                var self = this;

                self.name = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name }, activeDirectoryCompliant: "" });    
                self.description = ko.observable().extend({ maxLength: { maxLength: ko.maxLength.name } });
                self.registerFields(self.name, self.description);
                self.name(item.Name);
                self.description(item.Description);
                
                if (item.IsSystematic === true) {
                    self.editingIsDisabled(true);
                    self.deletingIsDisabled(true);
                }
            }

            RoleViewModel.prototype.getDeleteConfirmationMessage = function () {
                return $.format(globalization.deleteRoleConfirmMessage, antiXss.encodeHtml(this.name()));
            };

            RoleViewModel.prototype.getSaveParams = function () {
                var params = _super.prototype.getSaveParams.call(this);
                params.Name = this.name();
                params.Description = this.description();

                return params;
            };

            return RoleViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Roles autocomplete list view model
        */
        var RolesAutocompleteListViewModel = (function (_super) {
            bcms.extendsClass(RolesAutocompleteListViewModel, _super);

            function RolesAutocompleteListViewModel(tagsList) {
                var options = {
                    serviceUrl: links.roleSuggestionServiceUrl,
                    pattern: 'Roles[{0}]'
                };

                _super.call(this, tagsList, options);
            }

            return RolesAutocompleteListViewModel;
        })(autocomplete.AutocompleteListViewModel);

        role.RolesAutocompleteListViewModel = RolesAutocompleteListViewModel;

        /**
        * Initializes role module.
        */
        role.init = function() {
        };

        bcms.registerInit(role.init);

        return role;
    });
