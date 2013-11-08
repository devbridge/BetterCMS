(function () {
  var a = {
    modes: { wysiwyg: 1, source: 0 },
    exec: function (editor) {
        editor.InsertDynamicRegion(editor);
    }
  },
  b = 'cms-dynamicregion';
  CKEDITOR.plugins.add(b, {
    onLoad : function() {
      if (CKEDITOR.addCss) {
          CKEDITOR.addCss(
            'img.cke_cmsdynamicregion {' +
                'outline: 1px dashed #009AEB;' +
                'width:100%;' +
                'height:30px;' +
            '}'
          );
      }
    },
    init: function (e) {
      if (e.DynamicRegionsEnabled !== true) {
        return;
      }
      var c = e.addCommand(b, a);
      e.ui.addButton('CmsDynamicRegion', {
        label: 'Insert Dynamic Region',
        icon: 'cmsdynamicregion',
        command: b
      });
    },
    afterInit: function (e) {
        if (e.DynamicRegionsEnabled !== true) {
            return;
        }
        var dp = e.dataProcessor,
            hf = dp && dp.htmlFilter,
            df = dp && dp.dataFilter;
        if (df) {
            df.addRules({
                elements: {
                    div: function (re) {
                        var regexp = /^{{DYNAMIC_REGION\:([a-zA-Z0-9]{8}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{12})}}$/i;
                        if (re.children.length == 1 && CKEDITOR.htmlParser.text.prototype.isPrototypeOf(re.children[0]) && regexp.test(re.children[0].value)) {
                            var f = e.createFakeParserElement(re, 'cke_cmsdynamicregion', 'cmsdynamicregion', false);
                            f.attributes.alt = 'Dynamic Region';
                            f.attributes.title = 'Dynamic Region';
                            return f;
                        }
                        return undefined;
                    }
                }
            }, 3);
        }
        if (hf) {
            hf.addRules({
                elements: {
                    $: function (re) {
                    }
                }
            });
        }
    }
  });
})();