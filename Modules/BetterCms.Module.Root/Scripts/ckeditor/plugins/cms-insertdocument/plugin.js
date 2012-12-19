(function () {
  var a = {
    modes: { wysiwyg: 1, source: 1 },
    exec: function (editor) {
      CMS.files.loadInsertFileDialog(editor);
    }
  },
  b = 'cms-insertdocument';
  CKEDITOR.plugins.add(b, {
    init: function (c) {
      var d = c.addCommand(b, a);
      c.ui.addButton('CmsFileManager', {
        label: 'Insert Link to File',
        icon: CKEDITOR.plugins.getPath(b) + 'icn-file.png',
        command: b
      });
    }
  });
})();