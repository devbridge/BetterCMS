var $ = require('gulp-load-plugins')({lazy: true}),
    bourbon = require('node-bourbon'),
    utils = require('./utils/utils.js')();

module.exports = function () {

    var config = {
        dataUri: {
            src: ['Scss/base_64/**/*.{png,jpg,gif,svg,ttf}'],
            dest: 'Scss/core',
            resultFile: '_assets.scss',
            varPrefix: '$bcms-'
        },

        sass: {
            src: ['./Scss/**/*.scss'],
            map: './'
        },

        plumber: {
            info: {
                errorHandler: function (error) {
                    var message = $.util.colors['red']('\nError: ') + error.message + '\n' +
                        $.util.colors['white']('File: ') + error.fileName + '\n' +
                        $.util.colors['white']('Line: ') + error.lineNumber;

                    utils.log(message, 'grey');

                    utils.log('READY', 'green');
                }
            }
        },

        build: {
            annotate: true,
            minifyCss: true
        }

    };

    return config;
};
