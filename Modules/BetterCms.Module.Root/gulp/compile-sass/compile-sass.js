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
        .pipe(gulp.dest(destination))
        .pipe($.sourcemaps.write(config.sass.map))
        .pipe(cssFilter)
        .pipe($.combineMediaQueries())
        .pipe($.csso())
        .pipe($.rename({suffix: '.min'}))
        .pipe(gulp.dest(destination))
        .pipe(cssFilter.restore());
}

function task() {
    utils.log('Compiling SASS');

    compileSass('./Scss/**/baseTMP.scss', './Content/Styles');
    compileSass('./Scss/**/blogTMP.scss', '../../Modules/BetterCms.Module.Blog/Content/Styles');
    compileSass('./Scss/**/installationTMP.scss', '../../Modules/BetterCms.Module.Installation/Content/Styles');
    compileSass('./Scss/**/mediaTMP.scss', '../../Modules/BetterCms.Module.MediaManager/Content/Styles');
    compileSass('./Scss/**/pagesTMP.scss', '../../Modules/BetterCms.Module.Pages/Content/Styles');
    compileSass('./Scss/**/usersTMP.scss', '../../Modules/BetterCms.Module.Users/Content/Styles');
}
