var gulp = require('gulp'),
    $ = require('gulp-load-plugins')({
        lazy: true
    }),
    config = require('../gulp.config.js')(),
    gcmq = require('gulp-group-css-media-queries'),
    utils = require('../utils/utils.js')();

module.exports = task;

function compileSass(source, destination, omitPrefix) {
    var cssFilter = $.filter('**/*.css');

    var pipe = gulp.src(source)
        .pipe($.plumber(config.plumber.info))
        .pipe($.sourcemaps.init())
        .pipe($.sass({
            includePaths: require('node-bourbon').includePaths,
            outputStyle: 'expanded'
        }));

    if (!omitPrefix) {
        pipe.pipe($.rename({prefix: 'bcms.'}))
    }

    pipe
        .pipe(gulp.dest(destination))
        .pipe(cssFilter)
        .pipe(gcmq())
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
    compileSass('./Scss/**/editor.scss', './Scripts/ckeditor/skins/bettercms', true);
    //temp local use only
    compileSass('./Scss/**/root.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-root/Content/Styles');
    compileSass('./Scss/**/blog.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-blog/Content/Styles');
    compileSass('./Scss/**/installation.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-installation/Content/Styles');
    compileSass('./Scss/**/media.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-media/Content/Styles');
    compileSass('./Scss/**/pages.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-pages/Content/Styles');
    compileSass('./Scss/**/users.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-users/Content/Styles');
    compileSass('./Scss/**/editor.scss', '../../Sandbox/BetterCms.Sandbox.Mvc4/file/bcms-root/Scripts/ckeditor/skins/bettercms', true);
}
