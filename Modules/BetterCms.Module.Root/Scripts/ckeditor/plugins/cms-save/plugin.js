(function () {
  var a = {
    modes: { wysiwyg: 1, source: 1 },
    exec: function (editor) {
      var parts = editor.name.split('-');
      var contentId = parts[parts.length - 1];
      if (editor.checkDirty() || dbcms_var_isDirty == true) {
        dbCms.SaveTextEdits(contentId, editor.getData());
      } else {
        dbCms.ShowTextEdits(contentId);
      }
    }
  },
  b = 'cms-save';
  CKEDITOR.plugins.add(b, {
    init: function (c) {
      var d = c.addCommand(b, a);
      c.ui.addButton('CmsSave', {
        label: 'Save',
        icon: 'https://s3.amazonaws.com/dbcms/dbcms-images/page_save.png',
        command: b
      });
    }
  });
})();