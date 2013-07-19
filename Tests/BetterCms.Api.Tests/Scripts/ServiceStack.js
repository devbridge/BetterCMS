/**
 * JSON Service Client Gateway for http://ServiceStack.net Web Services.
 * @requires jQuery.ajax or goog closure libarary XhrIo
 * @param {string} the baseUri of ServiceStack web services.
 * @constructor
 */
var servicestack = (function(){
    var JSC = function(baseUri) {
        this.baseSyncReplyUri = path.combine(baseUri, "Json/SyncReply");
        this.baseAsyncOneWayUri = path.combine(baseUri, "Json/AsyncOneWay");
    };
    var P = JSC.prototype;
    /**
     * Example Usages:
     * client.send({GetUser:1}) == client.send({GetUser:{Id:1}})
     * client.send('GetUser')   == client.send({GetUser:{}}) == client.send({GetUser:null})
     * @param request can be a string or object
     */
    P.send = function(request, onSuccess, onError, ajaxOptions) {
        ajaxOptions = ajaxOptions || {};
        var data = {}, webMethod = (typeof request == 'string') ? request : null;
        if (webMethod === null) {
            for (webMethod in request) {
                data = request[webMethod] || {};
                if (typeof data != 'object') data = {Id:data};
                if (ajaxOptions.contentType === jsonContentType) data = JSON.stringify(data);
                break;
            }
        }

        var startCallTime = new Date();
        var requestUrl = path.combine(this.baseSyncReplyUri, webMethod);
        var id = JSC.id++;

        var options = {
            type: "GET",
            url: requestUrl,
            data: data,
            dataType: "json",
            success: function(response) {
                var endCallTime = new Date();
                var callDuration = endCallTime.getTime() - startCallTime.getTime();
                if (!response) {
                    if (onSuccess) onSuccess(null);
                    return;
                }

                var status = JSC.parseResponseStatus(response.ResponseStatus);
                if (status.isSuccess) {
                    if (onSuccess) onSuccess(response);
                    JSC.onSuccess({ id: id, webMethod: webMethod, request: data,
                        response: response, durationMs: callDuration
                    });
                }
                else {
                    if (onError) onError(status);
                    JSC.onError({ id: id, webMethod: webMethod, request: data,
                        error: status, durationMs: callDuration
                    });
                }
            },
            error: function(xhr, desc, exceptionobj) {
                var endCallTime = new Date();
                var callDuration = endCallTime.getTime() - startCallTime.getTime();

                try {
                    var response = xhr.responseText;
                    try { response = JSON.parse(response); } catch (e) { }
                    if (onError) onError(response);
                }
                catch (e) { }
                JSC.onError({ id: id, webMethod: webMethod, request: data,
                    error: xhr.responseText, durationMs: callDuration
                });
            }
        };

        for (var k in ajaxOptions) options[k] = ajaxOptions[k];

        var ajax = JSC.ajax(options);
    };
    /**
     * Sends a HTTP 'GET' request on the QueryString
     */
    P.getFromService = function(request, onSuccess, onError) {
        var options = document.all ? { cache: false} : null;
        this.send(request, onSuccess, onError, options);
    };
    /**
     * Sends a HTTP 'POST' request as key value pair formData
     */
    P.postFormDataToService = function(request, onSuccess, onError) {
        this.send(request, onSuccess, onError, { type: "POST" });
    };
    /**
     * Sends a HTTP 'POST' request as JSON
     */
    P.postToService = function(request, onSuccess, onError) {
        this.send(request, onSuccess, onError, { type: "POST", processData: false, contentType: jsonContentType });
    };
    /**
     * Sends a HTTP 'PUT' request as JSON
     */
    P.putToService = function(request, onSuccess, onError) {
        this.send(request, onSuccess, onError, { type: "PUT", processData: false, contentType: jsonContentType });
    };
    /**
     * Sends a HTTP 'DELETE' request as JSON
     */
    P.deleteFromService = function(request, onSuccess, onError) {
        this.send(request, onSuccess, onError, { type: "DELETE", processData: false, contentType: jsonContentType });
    };
    
    JSC.parseResponseStatus = function(status) {
        if (!status) return { isSuccess: true };

        var result =
        {
            isSuccess: status.ErrorCode === undefined || status.ErrorCode === null,
            errorCode: status.ErrorCode,
            message: status.Message,
            errorMessage: status.ErrorMessage,
            stackTrace: status.StackTrace,
            fieldErrors: [],
            fieldErrorMap: {}
        };

        if (status.FieldErrors) {
            for (var i = 0, len = status.FieldErrors.length; i < len; i++) {
                var err = status.FieldErrors[i];
                var error = { errorCode: err.ErrorCode, fieldName: err.FieldName, errorMessage: err.ErrorMessage || '' };
                result.fieldErrors.push(error);

                if (error.fieldName) {
                    result.fieldErrorMap[error.fieldName] = error;
                }
            }
        }
        return result;
    };

    JSC.id = 0;
    JSC.onError = function() { };
    JSC.onSuccess = function() { };
    JSC.toJsonDate = function(date) {
        var jsDate = is.Date(date) ? date : new Date(date);
    }

    var jsonContentType = "application/json; charset=utf-8",
        rquery = /\?/,
        rts = /(\?|&)_=.*?(&|$)/,
        rurl = /^(\w+:)?\/\/([^\/?#]+)/,
        ajaxSettings = {
            type: "GET",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            accepts: {
                xml: "application/xml, text/xml",
                html: "text/html",
                script: "text/javascript, application/javascript",
                json: "application/json, text/javascript",
                text: "text/plain",
                _default: "*/*"
            }
        };

    JSC.ajax = function(s) {
        if (typeof window.$ !== undefined)
            return $.ajax(s);

        for (var k in ajaxSettings) if (!s[k]) s[k] = ajaxSettings[k];

        var xhr = new goog.net.XhrIo();
        goog.events.listen(xhr, "complete", function(){
            if (xhr.isSuccess())
            {
                if (!s.success) return;
                if (s.dataType == "json") {
                    s.success(xhr.getResponseJson());
                } else if (s.dataType == "xml") {
                    s.success(xhr.getResponseXml());
                } else {
                    s.success(xhr.getResponseText());
                }
            }
            else
            {
                if (!s.error) return;
                s.error(xhr, xhr.getLastErrorCode(), xhr.getLastError());
            }
        });

        if (s.cache === false && type === "GET" )
        {
            var ts = (new Date).getTime();
            var ret = s.url.replace(rts, "$1_=" + ts + "$2");
            s.url = ret + ((ret === s.url) ? (rquery.test(s.url) ? "&" : "?") + "_=" + ts : "");
        }

        var headers = {'Content-Type':s.contentType};
        var data = goog.Uri.QueryData.createFromMap(new goog.structs.Map(s.data));

        if (s.type == "GET")
        {
            s.url += (rquery.test(s.url) ? "&" : "?") + data.toString();
            xhr.send(s.url, "GET", null, headers);
        }
        else
        {
            var strData = (s.contentType == jsonContentType)
                ? s.data
                : data.toString();

            xhr.send(s.url, s.type, strData, headers);
        }
    }
    var is = {
        Null: function(a) {
            return a === null;
        },
        Undefined: function(a) {
            return a === undefined;
        },
        Empty: function(a) {
            return (a === null || a === undefined || a === "");
        },
        Function: function(a) {
            return (typeof (a) === 'function') ? a.constructor.toString().match(/Function/) !== null : false;
        },
        String: function(a) {
            if (a === null || a === undefined || a.type) return false;
            return (typeof (a) === 'string') ? true : (typeof (a) === 'object') ? a.constructor.toString().match(/string/i) !== null : false;
        },
        Array: function(a) {
            if (is.Empty(a) || a.type) return false;
            return (typeof (a) === 'object') ? a.constructor.toString().match(/array/i) !== null || a.length !== undefined : false;
        },
        Boolean: function(a) {
            if (is.Empty(a) || a.type) return false;
            return (typeof (a) === 'boolean') ? true : (typeof (a) === 'object') ? a.constructor.toString().match(/boolean/i) !== null : false;
        },
        Date: function(a) {
            if (is.Empty(a) || a.type) return false;
            return (typeof (a) === 'date') ? true : (typeof (a) === 'object') ? a.constructor.toString().match(/date/i) !== null : false;
        },
        Number: function(a) {
            if (is.Empty(a) || a.type) return false;
            return (typeof (a) === 'number') ? true : (typeof (a) === 'object') ? a.constructor.toString().match(/Number/) !== null : false;
        },
        ValueType: function(a) {
            if (is.Empty(a) || a.type) return false;
            return is.String(a) || is.Date(a) || is.Number(a) || is.Boolean(a);
        }
    };

    /**
     * String Utils
     */
    var S = {};
    S.rtrim = function(str, chars) {
        chars = chars || "\\s";
        return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
    };
    S.toString = function() {
        if (arguments.length == 0 || !arguments[0]) return null;

        var s = "";
        for (var i = 0; i < arguments.length; i++) {
            var arg = arguments[i];

            if (s) s += "/";

            if (is.String(arg)) s += arg;
            else if (is.ValueType(arg)) s += arg.toString();
            else if (is.Array(arg)) s += '[' + A.join(arg, ",") + ']';
            else {
                var o = "";
                for (var name in arg) {
                    if (o) o += ",";
                    o += name + ":" + S.safeString(arg[name]);
                }
                s += '{' + o + '}';
            }
        }
        return s;
    };
    S.safeString = function(str) {
        if (!str) return str;
        if (S.containsAny(str, ['[', ']', '{', '}', ','])) {
            return '"' + str + '"';
        }
        return str;
    };
    S.containsAny = function(str, tests) {
        if (!is.String(str)) return;
        for (var i = 0, len = tests.length; i < len; i++) {
            if (str.indexOf(tests[i]) != -1) return true;
        }
        return false;
    };
    S.startsWith = function(text, startsWith) {
        if (!text || !startsWith) return false;
        return text.lastIndexOf(startsWith, 0) == 0;
    };
    S.pad = function(text, padLen, padChar, rpad) {
        var padChar = padChar || (rpad ? " " : "0");
        text = text.toString();
        while (text.length < padLen) {
            text = rpad
              ? text + padChar
              : padChar + text;
        }
        return text;
    }
    S.padLeft = function(text, padLen, padChar) {
        return S.pad(text, padLen, padChar, false);
    }
    S.padRight = function(text, padLen, padChar) {
        return S.pad(text, padLen, padChar, true);
    }
    S.lpad = S.padLeft;
    S.rpad = S.padRight;


    /**
     * Array Utils
     */
    var A = {};
    A.each = function(array, fn) {
        if (!array) return;
        for (var i = 0, len = array.length; i < len; i++)
            fn(array[i]);
    };
    A.convertAll = function(array, convertFn) {
        var to = [];
        for (var i = 0, len = array.length; i < len; i++)
            to[i] = convertFn(array[i]);
        return to;
    };
    A.join = function(array, on) {
        var s = "";
        on = on || ",";
        for (var i = 0, len = array.length; i < len; i++) {
            if (s) s += on;
            s += array[i];
        }
        return s;
    };
    A.toTable = function(array, tableFormatFns) {
        tableFormatFns = tableFormatFns || {};
        var cols = [], sb = [];
        for (var i = 0, len = array.length; i < len; i++) {
            var obj = array[i];
            if (!obj) continue;
            if (i == 0) {
                sb.push("<table><thead><tr>");
                for (var k in obj) {
                    cols.push(k);
                    sb.push("<th>" + k + "</th>");
                }
                sb.push("</tr></thead><tbody>");
            }
            sb.push("<tr>");
            for (var j = 0, colsLen = cols.length; j < colsLen; j++) {
                var k = cols[j];
                var data = tableFormatFns[k] ? tableFormatFns[k](obj[k]) : dto.formatValue(obj[k]);

                sb.push("<td>" + data + "</td>");
            }
            sb.push("</tr>");
        }
        sb.push("</tbody></table>");
        return sb.join('');
    }
    /**
     *Object Utils
     */
    var O = {};
    O.keys = function(obj) {
        var keys = [];
        for (var key in obj) keys.push(key);
        return keys;
    };
    /**
     * Path Utils
     */
    var path = {};
    path.combine = function() {
        var paths = "";
        for (var i = 0, len = arguments.length; i < len; i++) {

            if (paths.length > 0)
                paths += "/";

            paths += S.rtrim(arguments[i], '/');
        }
        return paths;
    };
    path.getFirstArg = function(path)
    {
        if (!path) return null;
        return path.split('/')[0];
    };
    path.getFirstValue = function(path)
    {
        if (!path || path.indexOf('/') == -1) return null;
        return path.substr(path.indexOf('/') + 1);
    };
    path.getArgs = function(path)
    {
        if (!path) return null;
        return path.split('/');
    };

    var urn = {};
    urn.toId = function(urn) {
        return urn.replace(/:/g, '_');
    };
    urn.getIdValue = function(urn) {
        return urn.split(':')[2];
    };
    urn.fromId = function(urn) {
        return urn.replace(/_/g, ':');
    };

    var dto = {};
    dto.toArray = function(array) {
        return is.Array(array)
            ? S.toString(array)
            : "[" + S.toString(array) + "]";
    };
    dto.toUtcDate = function(date) {
        return date.getUTCFullYear()
            + '-' + S.lpad(date.getUTCMonth() + 1, 2)
            + '-' + S.lpad(date.getUTCDate(), 2)
            + 'T' + S.lpad(date.getUTCHours(), 2)
            + ':' + S.lpad(date.getUTCMinutes(), 2)
            + ':' + S.lpad(date.getUTCSeconds(), 2)
            + 'Z';
    };
    dto.isJsonDate = function(str)
    {
        if (!is.String(str)) return false;
        return S.startsWith(str, dto.WcfDatePrefix);
    };
    dto.WcfDatePrefix = "\/Date(";
    dto.toJsonDate = function(date) {
        date = dto.parseJsonDate(date);
        return dto.WcfDatePrefix + date.getTime() + "+0000)\/";
    };
    dto.parseJsonDate = function(date) {
        return is.Date(date)
            ? date
            : (S.startsWith(date, dto.WcfDatePrefix)
                ? new Date(parseInt(date.substring(dto.WcfDatePrefix.length, date.length - 2)))
                : new Date(date));
    };
    dto.formatDate = function(date) {
        //IE needs '/' seperators
        date = dto.parseJsonDate(date);
        return date.getUTCFullYear()
            + '/' + S.lpad(date.getUTCMonth() + 1, 2)
            + '/' + S.lpad(date.getUTCDate(), 2);
    };
    dto.formatValue = function(value)
    {
        if (dto.isJsonDate(value)) return dto.formatDate(value);
        if (is.Empty(value)) return "";
        return value;
    };

    return {
        ClientGateway: JSC,
        dto: dto,
        string: S,
        array: A,
        object: O,
        path: path,
        urn: urn
    };
})();

/* json2.min.js
 * 2008-01-17
 * Public Domain
 * No warranty expressed or implied. Use at your own risk.
 * See http://www.JSON.org/js.html
*/
if(!this.JSON){JSON=function(){function f(n){return n<10?'0'+n:n;}
Date.prototype.toJSON=function(){return this.getUTCFullYear()+'-'+
f(this.getUTCMonth()+1)+'-'+
f(this.getUTCDate())+'T'+
f(this.getUTCHours())+':'+
f(this.getUTCMinutes())+':'+
f(this.getUTCSeconds())+'Z';};var m={'\b':'\\b','\t':'\\t','\n':'\\n','\f':'\\f','\r':'\\r','"':'\\"','\\':'\\\\'};function stringify(value,whitelist){var a,i,k,l,r=/["\\\x00-\x1f\x7f-\x9f]/g,v;switch(typeof value){case'string':return r.test(value)?'"'+value.replace(r,function(a){var c=m[a];if(c){return c;}
c=a.charCodeAt();return'\\u00'+Math.floor(c/16).toString(16)+
(c%16).toString(16);})+'"':'"'+value+'"';case'number':return isFinite(value)?String(value):'null';case'boolean':case'null':return String(value);case'object':if(!value){return'null';}
if(typeof value.toJSON==='function'){return stringify(value.toJSON());}
a=[];if(typeof value.length==='number'&&!(value.propertyIsEnumerable('length'))){l=value.length;for(i=0;i<l;i+=1){a.push(stringify(value[i],whitelist)||'null');}
return'['+a.join(',')+']';}
if(whitelist){l=whitelist.length;for(i=0;i<l;i+=1){k=whitelist[i];if(typeof k==='string'){v=stringify(value[k],whitelist);if(v){a.push(stringify(k)+':'+v);}}}}else{for(k in value){if(typeof k==='string'){v=stringify(value[k],whitelist);if(v){a.push(stringify(k)+':'+v);}}}}
return'{'+a.join(',')+'}';}}
return{stringify:stringify,parse:function(text,filter){var j;function walk(k,v){var i,n;if(v&&typeof v==='object'){for(i in v){if(Object.prototype.hasOwnProperty.apply(v,[i])){n=walk(i,v[i]);if(n!==undefined){v[i]=n;}}}}
return filter(k,v);}
if(/^[\],:{}\s]*$/.test(text.replace(/\\./g,'@').replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,']').replace(/(?:^|:|,)(?:\s*\[)+/g,''))){j=eval('('+text+')');return typeof filter==='function'?walk('',j):j;}
throw new SyntaxError('parseJSON');}};}();}
