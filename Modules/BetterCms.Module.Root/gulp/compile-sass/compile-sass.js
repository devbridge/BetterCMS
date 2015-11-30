var gulp = require('gulp'),
    $ = require('gulp-load-plugins')({
        lazy: true
    }),
    config = require('../gulp.config.js')(),
    gcmq = require('gulp-group-css-media-queries'),
    utils = require('../utils/utils.js')(),
    wrapper = require('gulp-wrapper');

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
        pipe.pipe($.rename({ prefix: 'bcms.' }));
        pipe.pipe(wrapper({ header: 
            '/* --------------------------------------------------------------------------------------------------------------------\r\n' +
            '// <copyright file="${filename}" company="Devbridge Group LLC">\r\n' +
            '//\r\n' +
            '// Copyright (C) 2015,2016 Devbridge Group LLC\r\n' +
            '//\r\n' +
            '// This program is free software: you can redistribute it and/or modify\r\n' +
            '// it under the terms of the GNU Lesser General Public License as published by\r\n' +
            '// the Free Software Foundation, either version 3 of the License, or\r\n' +
            '// (at your option) any later version.\r\n' +
            '//\r\n' +
            '// This program is distributed in the hope that it will be useful,\r\n' +
            '// but WITHOUT ANY WARRANTY; without even the implied warranty of\r\n' +
            '// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the\r\n' +
            '// GNU General Public License for more details.\r\n' +
            '//\r\n' +
            '// You should have received a copy of the GNU Lesser General Public License\r\n' +
            '// along with this program.  If not, see http://www.gnu.org/licenses/.\r\n' +
            '// </copyright>\r\n' +
            '//\r\n' +
            '// <summary>\r\n' +
            '// Better CMS is a publishing focused and developer friendly .NET open source CMS.\r\n' +
            '//\r\n' +
            '// Website: https://www.bettercms.com\r\n' +
            '// GitHub: https://github.com/devbridge/bettercms\r\n' +
            '// Email: info@bettercms.com\r\n' +
            '// </summary>\r\n' +
            '// ------------------------------------------------------------------------------------------------------------------*/\r\n'
        }));
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
