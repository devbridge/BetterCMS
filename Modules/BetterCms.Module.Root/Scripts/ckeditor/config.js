CKEDITOR.editorConfig = function (c) {
  c.ForcePasteAsPlainText = true;
  c.extraPlugins = 'cms-imagemanager,cms-insertdocument';
  c.resize_minWidth = 200;
  c.height = '500px';
  c.toolbarCanCollapse = false;
  c.toolbar = 'CMS';
  c.language = 'en';
  c.defaultLanguage = 'en';
  // Toolbar Guide: http://docs.cksource.com/CKEditor_3.x/Developers_Guide/Toolbar
  c.toolbar_CMS =
	[
		['Undo', 'Redo', '-', 'Bold', 'Italic', 'Strike', 'RemoveFormat', '-', 'NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'],
		['Link', 'Unlink', 'CmsImageManager', 'CmsFileManager', 'Table'],
    ['Format', 'FontSize', 'Source']
	];
  //Custom styles dropdown:
  //c.stylesSet = 'CMS';
  //c.font_names = 'RockwellWeb;Arial, Sans Serif;';
  c.format_tags = 'p;h1;h2;h3;h4;pre;div';
  // Custom font sizes: 'label/cssSize;...'
  //c.fontSize_sizes = '10/10px;12/12px';
};

CKEDITOR.toolbar_CMS = 
	[
		['Undo','Redo','-','Bold','Italic','Strike','RemoveFormat','-','NumberedList','BulletedList','-','Outdent','Indent'],
		['Link','Unlink','CmsImageManager','CmsFileManager','Table'],
    ['Format','FontSize','Source']
	];

CKEDITOR.stylesSet.add('CMS', [
    { name:'Quote', element:'div', attributes:{ 'style':'padding:10px;background:#EEE;font-szie:22px;margin:10px;' } },
    { name:'Small Print', element:'div', attributes:{ 'style':'font-size:11px; color:#CCC;' } }
  ]);


CKEDITOR.on('instanceReady', function(ev){
  var w = ev.editor.dataProcessor.writer,
  s0 = {indent:true,breakBeforeOpen:true,breakAfterOpen:false,breakBeforeClose:false,breakAfterClose:true},
  s1 = {indent:true,breakBeforeOpen:true,breakAfterOpen:false,breakBeforeClose:false,breakAfterClose:true},
  s2 = {indent:true,breakBeforeOpen:false,breakAfterOpen:false,breakBeforeClose:false,breakAfterClose:false},
  s3 = {indent:true,breakBeforeOpen:true,breakAfterOpen:true,breakBeforeClose:true,breakAfterClose:true};
  w.indentationChars = '  ';
  w.setRules('p', s0);
  w.setRules('h1',s1);
  w.setRules('h2',s1);
  w.setRules('h3',s1);
  w.setRules('h4',s1);
  w.setRules('h5',s1);
  w.setRules('h6',s1);
  w.setRules('pre',s2);
  w.setRules('div',s3);
  w.setRules('li',s3);
});

