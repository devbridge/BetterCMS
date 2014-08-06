(function() {
  var a = {
    modes: { wysiwyg: 1, source: 1 },
    exec: function(editor) {
      editor.InsertDynamicRegion(editor);
    }
  },
    b = 'cms-dynamicregion';
  CKEDITOR.plugins.add(b, {
    onLoad: function() {
      if (CKEDITOR.addCss) {
        CKEDITOR.addCss(
          '.bcms-draggable-region {' +
            '  position: relative;' +
            '  height: 35px;' +
            '  line-height: 35px;' +
            '  padding-left: 34px;' +
            '  margin: 0 -2px 0 0;' +
            '  color: #0099ee;' +
            "  background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAFRlDm/AAAAHUlEQVQI12P4//8/AxRDqQcPHjAAMUIQyoFQEAAA0/UuXeg1e3EAAAAASUVORK5CYII=') repeat 0 0;" +
            '  border: 1px dashed #0099ee;' +
            '  font-size: 12px;' +
            '  font-weight: 700;' +
            '}' +
            '  .bcms-draggable-region:after {' +
            '    content: "";' +
            '    position: absolute;' +
            '    top: 50%;' +
            '    left: 11px;' +
            '    width: 15px;' +
            '    height: 18px;' +
            '    margin-top: -9px;' +
            '    background: url("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA8AAAASCAYAAACEnoQPAAAAV0lEQVQ4y2NgmPX6Pw78D4hLgJgBJ8ajGYZridGMLolsQAclmv+TqhndkOGsmRCf+poJYeprJj6AIOJclGgGYW1CmvHhr0CcQHXNDEQkGB1cziakGR5gAEOrBIwBImPOAAAAAElFTkSuQmCC") no-repeat 0 0;' +
            '  }'
        );
      }
    },
    init: function(e) {
      if (e.DynamicRegionsEnabled !== true) {
        return;
      }
      var c = e.addCommand(b, a);
      e.ui.addButton('CmsDynamicRegion', {
        title: 'Insert Dynamic Region',
        label: 'Region',
        icon: 'cmsdynamicregion',
        command: b
      });
    },
    afterInit: function(e) {
      if (e.DynamicRegionsEnabled !== true) {
        return;
      }
      var dp = e.dataProcessor,
        hf = dp && dp.htmlFilter,
        df = dp && dp.dataFilter;
      if (df) {
        df.addRules({
            elements: {
              div: function(el) {
                var regexp = /^{{DYNAMIC_REGION\:([a-zA-Z0-9\-_]{1,200})}}$/i;
                if (el.children.length == 1 && CKEDITOR.htmlParser.text.prototype.isPrototypeOf(el.children[0]) && regexp.test(el.children[0].value)) {
                  var f = e.createFakeParserElement(el, 'bcms-draggable-region', 'cmsdynamicregion', false);
                  f.attributes.title = 'Dynamic Region';
                  //f.attributes.contenteditable = 'false';
                  f.attributes.isregion = 'true';
                  f.name = "div";
                  delete f.attributes["alt"];
                  delete f.attributes["align"];
                  delete f.attributes["src"];
                  f.add(new CKEDITOR.htmlParser.text('Content to add'));
                  f.isEmpty = false;
                  return f;
                }
                return null;
              }
            }
          }, {
            applyToAll: true
          });
      }
      if (hf) {
        hf.addRules({
            elements: {
              div: function(el) {
                if (el.attributes.isregion !== 'true') {
                  return null;
                }
                var attributes = el.attributes,
                  html = attributes && attributes['data-cke-realelement'],
                  fragment = html && new CKEDITOR.htmlParser.fragment.fromHtml(decodeURIComponent(html));
                return fragment && fragment.children[0];
              }
            }
          }, {
            applyToAll: true
          });
      }
    }
  });
})();