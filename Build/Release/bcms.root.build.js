/*global console, require */

(function () {
    'use strict';

    console.log('Optimize BetterCMS JavaScript modules.');

    var requirejs = require('../build/r.js'),
        config = {
            baseUrl: '../',
            name: 'main',
            paths: {
                jquery: 'empty:',
                jqueryui: 'empty:',
				knockout: 'empty:',
				jqueryvalidateunobtrusive: 'empty:'
            },
            out: '../build/main-built.js'
        };
	
    requirejs.optimize(config,
		
		function (buildResponse) {
			console.log('Optimization completed.');

			//buildResponse is just a text output of the modules
			//included. Load the built file for the contents.

			console.log(buildResponse);
		},
		
		function (err) {
			console.log(err);
		});

}());

