var gulp = require('gulp'),
    $ = require('gulp-load-plugins')({lazy: true}),
    config = require('../gulp.config.js')(),
    utils = require('../utils/utils.js')();

module.exports = task;

function compileSass(source, destination) {
    var cssFilter = $.filter('**/*.css');

    gulp.src(source)
        .pipe($.plumber(config.plumber.info))
        .pipe($.sourcemaps.init())
        .pipe($.sass({
            includePaths: require('node-bourbon').includePaths,
            outputStyle: 'expanded'
        }))
        .pipe($.rename({prefix: 'bcms.'}))
        .pipe(gulp.dest(destination))
        .pipe(cssFilter)
        .pipe($.combineMediaQueries())
        .pipe($.csso())
        .pipe($.rename({suffix: '.min'}))
        .pipe(gulp.dest(destination))
        .pipe(cssFilter.restore());
}

function task() {
    utils.log('Compiling SASS');

    compileSass('./Scss/**/root.scss', './Content/Styles');
    compileSass('./Scss/**/blog.scss', '../../Modules/BetterCms.Module.Blog/Content/Styles');
    compileSass('./Scss/**/installation.scss', '../../Modules/BetterCms.Module.Installation/Content/Styles');
    compileSass('./Scss/**/media.scss', '../../Modules/BetterCms.Module.MediaManager/Content/Styles');
    compileSass('./Scss/**/pages.scss', '../../Modules/BetterCms.Module.Pages/Content/Styles');
    compileSass('./Scss/**/users.scss', '../../Modules/BetterCms.Module.Users/Content/Styles');
    //temp local use only
    compileSass('./Scss/**/root.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-root/Content/Styles');
    compileSass('./Scss/**/pages.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-pages/Content/Styles');
}
