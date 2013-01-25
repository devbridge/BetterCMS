(function () {
  var a = {
    modes: { wysiwyg: 1, source: 1 },
    exec: function (editor) {
        editor.InsertFileClicked(editor);
    }
  },
  b = 'cms-filemanager';
  CKEDITOR.plugins.add(b, {
    init: function (c) {
      var d = c.addCommand(b, a);
      c.ui.addButton('CmsFileManager', {
        label: 'Insert File',
        icon: 'templates',
        command: b
      });
    }
  });
})();