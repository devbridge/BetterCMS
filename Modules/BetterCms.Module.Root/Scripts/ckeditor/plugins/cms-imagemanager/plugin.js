(function () {
  var a = {
    modes: { wysiwyg: 1, source: 1 },
    exec: function (editor) {
        editor.InsertImageClicked(editor);
    }
  },
  b = 'cms-imagemanager';
  CKEDITOR.plugins.add(b, {
    init: function (c) {
      var d = c.addCommand(b, a);
      c.ui.addButton('CmsImageManager', {
        label: 'Insert Image',
        icon: 'cmsimage',
        command: b
      });
    }
  });
})();