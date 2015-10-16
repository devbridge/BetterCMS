module.exports = function () {

    var utils = {},
        $ = require('gulp-load-plugins')({ lazy: true }),
        defaultColor = 'yellow',
        path = require('path');

    utils.log = function (msg, color, consoleLog) {

        if (color === undefined) {

            color = defaultColor;
        }

        if (typeof (msg) === 'object') {

            for (var item in msg) {

                if (msg.hasOwnProperty(item)) {

                    if (!!consoleLog) {
                        console.log($.util.colors[color](msg[item]));
                    } else {
                        $.util.log($.util.colors[color](msg[item]));
                    }
                }
            }

        } else {

            if (!!consoleLog) {
                console.log($.util.colors[color](msg));
            } else {
                $.util.log($.util.colors[color](msg));
            }
        }
    };

    utils.consoleLog = function (msg, color) {
        utils.log(msg, color, true);
    };

    utils.color = function (text, color) {

        if (color === undefined) {

            color = defaultColor;
        }

        return $.util.colors[color](text);
    };

    utils.replaceAll = function (str, find, replace) {

        return str.replace(new RegExp(find, 'g'), replace);
    };

    utils.sleep = function (millis) {

        var date = new Date();
        var curDate = null;

        do {
            curDate = new Date();
        }
        while (curDate - date < millis);
    };

    utils.alignedString = function (text, width, alignLeft) {

        var i,
            diff;

        text = text + '';

        diff = width - text.length;

        if (diff > 0) {

            for (i = 0; i < diff; i++) {

                if (alignLeft) {

                    text = text + ' ';
                } else {

                    text = ' ' + text;
                }
            }
        }

        return text;
    };

    utils.parsePath = function (filePath) {

        var extname = path.extname(filePath);

        return {
            dirname: path.dirname(filePath),
            basename: path.basename(filePath, extname),
            extname: extname.substr(1)
        };
    };

    return utils;
};
