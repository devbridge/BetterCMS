/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.antiXss.js" company="Devbridge Group LLC">
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
bettercms.define("bcms.antiXss", ['bcms'],
    function (bcms) {
        "use strict";

        var antiXss = {},
            globalization = {
                antiXssContainsHtmlError: null
            };

        /**
        * Assign objects to module.
        */
        antiXss.globalization = globalization;

        function MakeTextHtmlSafe(str) {
            return String(str)
                .replace(/&/g, "&amp;")
                .replace(/</g, "&lt;")
                .replace(/>/g, "&gt;")
                .replace(/"/g, "&quot;")
                .replace(/'/g, "&#039;")
                .replace(/\//g, "&#x2F;");
        }

        antiXss.encodeHtml = function(html) {
            return MakeTextHtmlSafe(html);
        }

        /**
        * Registers bcms antiXss module
        */
        antiXss.init = function() {
            bcms.logger.debug('Initializing bcms.antiXss module');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(antiXss.init);

        return antiXss;
    });