(function () {

    if (!jasmine) {
        throw new Exception("jasmine library does not exist in global namespace!");
    }

    function elapsed(startTime, endTime) {
        return (endTime - startTime) / 1000;
    }

    function ISODateString(d) {
        function pad(n) { return n < 10 ? '0' + n : n; }

        return d.getFullYear() + '-'
            + pad(d.getMonth() + 1) + '-'
            + pad(d.getDate()) + 'T'
            + pad(d.getHours()) + ':'
            + pad(d.getMinutes()) + ':'
            + pad(d.getSeconds());
    }

    function trim(str) {
        return str.replace(/^\s+/, "").replace(/\s+$/, "");
    }

    function escapeInvalidXmlChars(str) {
        return str.replace(/\&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/\>/g, "&gt;")
            .replace(/\"/g, "&quot;")
            .replace(/\'/g, "&apos;");
    }

    /**
     * PhantomJS Reporter generates NUnit XML for the given spec run.
     * Allows the test results to be used in .NET based CI.
     * It appends some DOM elements/containers, so that a PhantomJS script can pick that up.
     *
     * @param {boolean} consolidate whether to save nested describes within the
     *                  same file as their parent; default: true
     * @param {boolean} useDotNotation whether to separate suite names with
     *                  dots rather than spaces (ie "Class.init" not
     *                  "Class init"); default: true
     */
    var PhantomJSReporter = function (consolidate, useDotNotation) {        
        this.totalCount = 0;
        this.totalSuccessCount = 0;
        
        this.consolidate = consolidate === jasmine.undefined ? true : consolidate;
        this.useDotNotation = useDotNotation === jasmine.undefined ? true : useDotNotation;
    };

    PhantomJSReporter.prototype = {        

        reportRunnerStarting: function (runner) {
            this.log("Runner Started.");
        },

        reportSpecStarting: function (spec) {
            spec.startTime = new Date();

            if (!spec.suite.startTime) {
                spec.suite.startTime = spec.startTime;
            }            
        },

        reportSpecResults: function (spec) {
            var results = spec.results();
            spec.didFail = !results.passed();
            spec.status = spec.didFail ? 'FAILED' : 'Pass';
            
            if (results.skipped) {
                spec.status = 'Skipped';
            }
            
            this.log(spec.status + ' - ' + spec.suite.description + ' : ' + spec.description);

            spec.duration = elapsed(spec.startTime, new Date());
            
            
           
            var resultItems = results.getItems();
            var resultType = "Success";
            
            for (var i = 0; i < resultItems.length; i++) {
                var result = resultItems[i];
                if (result.type == 'expect' && result.passed) {                    
                    if (!result.passed()) {
                        resultType = escapeInvalidXmlChars(result.message);
                        
                        this.log(resultType);
                        this.log(' ');
                        
                        
                    }                  
                }
            }
            
            spec.output = '\n      <test-case name="' + escapeInvalidXmlChars(spec.description) + '"' +
                              ' executed="true" result="' + resultType + '"' +
                              ' time="' + spec.duration + '"' +
                              ' success="' + (resultType === "Success" ? "True" : "False") + '" />';
        },

        reportSuiteResults: function (suite) {
            var results = suite.results();
            var specs = suite.specs();
            var specOutput = "";
            
            var failedCount = 0;

            suite.status = results.passed() ? 'Passed.' : 'Failed.';
            suite.statusPassed = results.passed();
            if (results.totalCount === 0) { // todo: change this to check results.skipped
                suite.status = 'Skipped.';
            }

            // if a suite has no (active?) specs, reportSpecStarting is never called
            // and thus the suite has no startTime -- account for that here
            suite.startTime = suite.startTime || new Date();
            suite.duration = elapsed(suite.startTime, new Date());

            for (var i = 0; i < specs.length; i++) {
                failedCount += specs[i].didFail ? 1 : 0;
                specOutput += "\n  " + specs[i].output;
            }
            
            suite.output = '\n<test-suite type="TestFixture"' +
                           ' name="' + this.getFullName(suite) + '"' +
                           ' result="' + (failedCount === 0 ? 'Success' : 'Failure') + '"' +
                           ' success="' + (failedCount === 0 ? 'True' : 'False') + '"' +
                           ' asserts="' + specs.length + '"' +                           
                           ' time="' + suite.duration + '">';

            suite.output += '\n   <results>';
            suite.output += specOutput;
            suite.output += '\n    </results>';
            suite.output += "\n</test-suite>";

            this.totalCount += results.totalCount;
            this.totalSuccessCount += results.passedCount;
        },

        reportRunnerResults: function (runner) {
            this.log("Runner Finished.");
            var suites = runner.suites(),
                passed = true;
            for (var i = 0; i < suites.length; i++) {
                var suite = suites[i],
                    fullName = this.getFullName(suite),
                    filename = fullName + '-Results' + '.xml',
                    output = '<?xml version="1.0" encoding="UTF-8" standalone="no" ?>';

                passed = !suite.statusPassed ? false : passed;

                // if we are consolidating, only write out top-level suites
                if (this.consolidate && suite.parentSuite) {
                    continue;
                }
                else if (this.consolidate) {
                    output += '\n<test-results' +
                                    ' name="' + fullName + '"' +
                                    ' total="' + suites.length + '"' + '>';
                    output += '\n<culture-info current-culture="en-US" current-uiculture="en-US" />\n';
                    output += this.getNestedOutput(suite);                    
                    output += "\n</test-results>";
                    this.createSuiteResultContainer(filename, output);
                }
                else {
                    output += suite.output;
                    this.createSuiteResultContainer(filename, output);
                }
            }

            if (this.totalSuccessCount < this.totalCount) {
                this.log("Test run failed: " + this.totalSuccessCount + " of " + this.totalCount + " expectations passed and " + (this.totalCount - this.totalSuccessCount) + " expectations failed.");
            } else {
                this.log("Test run succeeded. All expectations " + this.totalSuccessCount + " passed.");
            }

            this.createTestFinishedContainer(passed);
        },

        getNestedOutput: function (suite) {
            var output = suite.output;
            for (var i = 0; i < suite.suites().length; i++) {
                output += this.getNestedOutput(suite.suites()[i]);
            }
            
            return output;
        },

        createSuiteResultContainer: function (filename, xmloutput) {
            jasmine.phantomjsXMLReporterResults = jasmine.phantomjsXMLReporterResults || [];
            jasmine.phantomjsXMLReporterResults.push({
                "xmlfilename": filename,
                "xmlbody": xmloutput
            });
        },

        createTestFinishedContainer: function (passed) {
            jasmine.phantomjsXMLReporterPassed = passed;
        },

        getFullName: function (suite, isFilename) {
            var fullName;
            if (this.useDotNotation) {
                fullName = suite.description;
                for (var parentSuite = suite.parentSuite; parentSuite; parentSuite = parentSuite.parentSuite) {
                    fullName = parentSuite.description + '.' + fullName;
                }
            }
            else {
                fullName = suite.getFullName();
            }

            // Either remove or escape invalid XML characters
            if (isFilename) {
                return fullName.replace(/[^\w]/g, "");
            }
            return escapeInvalidXmlChars(fullName);
        },

        log: function (str) {
            var console = jasmine.getGlobal().console;

            if (console && console.log) {
                console.log(str);
            }
        }
    };

    // export public
    jasmine.PhantomJSReporter = PhantomJSReporter;
})();