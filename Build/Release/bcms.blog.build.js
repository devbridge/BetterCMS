/*global console, require */

(function () {
    'use strict';

    console.log('Optimize BetterCMS JavaScript bcms.blog.*.js modules.');

    var requirejs = require('../release/r.js'),
        config = {
			appDir: '../../',
            baseUrl: 'Modules',
            dir: '../../min',
			modules: [{
				name : 'main'
			}]
        };
	
    requirejs.optimize(config,
		
		function (buildResponse) {
			console.log('bcms.blog.*.js optimization completed.');			
			console.log(buildResponse);
		},
		
		function (err) {
			console.log(err);
		});

}());

