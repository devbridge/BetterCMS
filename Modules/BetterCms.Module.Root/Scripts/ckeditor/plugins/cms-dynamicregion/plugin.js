(function () {
  var a = {
    modes: { wysiwyg: 1, source: 0 },
    exec: function (editor) {
        editor.InsertDynamicRegion(editor);
    }
  },
  b = 'cms-dynamicregion';
  CKEDITOR.plugins.add(b, {
    init: function (c) {
      var d = c.addCommand(b, a);
      c.ui.addButton('CmsDynamicRegion', {
        label: 'Insert Dynamic Region',
        icon: 'cmsdynamicregion',
        command: b
      });
    }
  });
})();