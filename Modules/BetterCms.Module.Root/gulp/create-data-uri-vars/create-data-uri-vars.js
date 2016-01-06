var gulp = require('gulp'),
    $ = require('gulp-load-plugins')({ lazy: true }),
    config = require('../gulp.config.js')(),
    utils = require('../utils/utils.js')(),

    through = require('through2'),
    path = require("path");

module.exports = task;

function task() {

    utils.log('Creating Data Uri variables');

    var encodedFiles = [],
        firstFile,
        firstFileSet = false;

    return gulp.src(config.dataUri.src)
        .pipe(through.obj(convertFile, saveVariables))
        .pipe(gulp.dest(config.dataUri.dest));

    function convertFile(file, enc, done) {

        if (!firstFileSet) {

            firstFile = file.clone();

            firstFileSet = true;
        }

        try {
            
            var encodedFile = {
                path: config.dataUri.varPrefix + utils.parsePath(file.path).basename,
                contents: file.contents.toString('base64'),
                ext: utils.parsePath(file.path).extname
            };

            encodedFiles.push(encodedFile);

        } catch (e) {

            var message = $.util.colors['red']('\nError: ') + e;

            utils.log(message, 'grey');
        }

        done();
    }

    function saveVariables(done) {

        var files = '';

        encodedFiles.forEach(function (item) {

            var ext = item.ext;

            if(ext === 'svg'){

                ext += '+xml';
            }

            files = files + item.path + ': "data:image/' + ext + ';base64,' + item.contents + '";' + '\n';
        });

        firstFile.path = path.join(firstFile.base, config.dataUri.resultFile);
        firstFile.contents = new Buffer(files);

        this.push(firstFile);

        done();
    }
}
