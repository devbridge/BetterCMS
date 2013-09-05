-- Set login widget's option as non-deletable
UPDATE bcms_root.ContentOptions
SET IsDeletable = 0
WHERE ContentId = 'DE0E47B2-728D-4BE6-904D-ED99CDDEDA4A'
	AND [Key] IN (
		'LogInUrl', 
		'LogOutUrl', 
		'TitleText',
		'UserNameLabelText',
		'PasswordLabelText',
		'ShowFieldRememberMe',
		'RememberMeLabelText',
		'LogInButtonText',
		'LogOutButtonText',
		'UserLoggedInText',
		'MainDivCssClass')
