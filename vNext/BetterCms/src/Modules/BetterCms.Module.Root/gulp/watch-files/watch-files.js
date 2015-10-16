var gulp = require('gulp'),
    $ = require('gulp-load-plugins')({lazy: true}),
    config = require('../gulp.config.js')(),
    utils = require('../utils/utils.js')(),

    runSequence = require('run-sequence');

module.exports = task;

function task() {

    utils.log('Watching Sass and Image files');

    $.watch(config.sass.src, function () {

        runSequence('compile-sass');
    });

    $.watch(config.dataUri.src, function () {

        runSequence('create-data-uri-vars');
    });
}
