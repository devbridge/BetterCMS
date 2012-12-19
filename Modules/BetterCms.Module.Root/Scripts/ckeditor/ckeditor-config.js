CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

  config.ForcePasteAsPlainText = true;
  config.toolbar = 'CMS';
	config.toolbar_CMS =
	[
		['Undo','Redo'],
		['Bold','Italic','Strike','RemoveFormat'],
		['NumberedList','BulletedList','-','Outdent','Indent'],
		['Link','Unlink','CmsImageManager','Format'],
		['Source']
	];
  
  config.extraPlugins = 'cms-imagemanager,cms-save';
  config.resize_minWidth = 200;
  config.toolbarCanCollapse = false;
	
};

CKEDITOR.on('instanceReady', function( ev ){
        ev.editor.dataProcessor.writer.indentationChars = '  ';
        ev.editor.dataProcessor.writer.setRules( 'p',{indent : true,breakBeforeOpen : true,breakAfterOpen : false,breakBeforeClose : true,breakAfterClose : true});
        ev.editor.dataProcessor.writer.setRules( 'h1',{indent : true,breakBeforeOpen : true,breakAfterOpen : false,breakBeforeClose : false,breakAfterClose : true});
        ev.editor.dataProcessor.writer.setRules( 'h2',{indent : true,breakBeforeOpen : true,breakAfterOpen : false,breakBeforeClose : false,breakAfterClose : true});
        ev.editor.dataProcessor.writer.setRules( 'h3',{indent : true,breakBeforeOpen : true,breakAfterOpen : false,breakBeforeClose : false,breakAfterClose : true});
        ev.editor.dataProcessor.writer.setRules( 'h4',{indent : true,breakBeforeOpen : true,breakAfterOpen : false,breakBeforeClose : false,breakAfterClose : true});
        ev.editor.dataProcessor.writer.setRules( 'h5',{indent : true,breakBeforeOpen : true,breakAfterOpen : false,breakBeforeClose : false,breakAfterClose : true});
        ev.editor.dataProcessor.writer.setRules( 'h6',{indent : true,breakBeforeOpen : true,breakAfterOpen : false,breakBeforeClose : false,breakAfterClose : true});
        ev.editor.dataProcessor.writer.setRules( 'pre',{indent : true,breakBeforeOpen : false,breakAfterOpen : false,breakBeforeClose : false,breakAfterClose : false});
        ev.editor.dataProcessor.writer.setRules( 'div',{indent : true,breakBeforeOpen : true,breakAfterOpen : true,breakBeforeClose : true,breakAfterClose : true});
        ev.editor.dataProcessor.writer.setRules( 'li',{indent : true,breakBeforeOpen : true,breakAfterOpen : false,breakBeforeClose : false,breakAfterClose : true});
        });

 