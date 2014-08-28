param($installPath, $toolsPath, $package, $project)

# Add reference.
$project.Object.References | Where-Object { $_.Name -eq 'FluentMigrator.Runner' } | ForEach-Object { $_.Remove() }
$project.Object.References.Add("$installPath\..\FluentMigrator.Tools.1.0.6.0\tools\AnyCPU\40\FluentMigrator.Runner.dll")

# Copy config file.
$path = [System.IO.Path]
$source = $path::Combine($path::GetDirectoryName($project.FileName), "Views\Web.config")
$destination = $path::Combine($path::GetDirectoryName($project.FileName), "Areas\Web.config")
$destinationFolder = $path::Combine($path::GetDirectoryName($project.FileName), "Areas")

if (!(Test-Path $destination))
{
    if (Test-Path $source)
    {
        if(!(Test-Path $destinationFolder))
        {
            New-Item $destinationFolder -type directory
        }
        Copy-Item $source $destination
        $project.ProjectItems.AddFromFile($destination)
    }
}


# Update Global.asax

function Insert-Content {param ( [String]$Path )process {$( ,$_; Get-Content $Path -ea SilentlyContinue) | Out-File $Path}}

try
{
  $globalAsaxItem = $project.ProjectItems.Item("global.asax").ProjectItems.Item("global.asax.cs")
}
catch [System.Management.Automation.MethodInvocationException]
{
}

if($globalAsaxItem)
{
	$terminator = ";"
	$win = $globalAsaxItem.Open("{7651A701-06E5-11D1-8EBD-00A0C90F26EA}")
	$text = $win.Document.Object("TextDocument");

	$edit = $text.StartPoint.CreateEditPoint();
	$edit.Insert("using System.Security.Principal;")
	$edit.Insert([Environment]::NewLine)
	$edit.Insert("using BetterCms.Core;")
	$edit.Insert([Environment]::NewLine)
	$edit.Insert("using BetterCms.Core.Environment.Host;")
	$edit.Insert([Environment]::NewLine)

	$namespace = $globalAsaxItem.FileCodeModel.CodeElements | where-object {$_.Kind -eq 5}
	$class = $namespace.Children | where-object {$_.Kind -eq 1}

	$edit = $class.StartPoint.CreateEditPoint();
	$edit.LineDown()
	$edit.LineDown()
	$edit.Insert([Environment]::NewLine)
	$edit.Insert("	private static ICmsHost cmsHost;")
	$edit.Insert([Environment]::NewLine)

	# Update Application_Start
	$methods = $class.Children | where-object {$_.Name -eq "Application_Start"}
	if (!$methods)
	{
		$edit = $class.StartPoint.CreateEditPoint();
		$edit.LineDown()
		$edit.LineDown()
		$edit.Insert([Environment]::NewLine)
		$edit.Insert("	private static ICmsHost cmsHost;")
		$edit.Insert([Environment]::NewLine)

		$edit.Insert("	protected void Application_Start(){")
		$edit.Insert([Environment]::NewLine)

		$edit.Insert("		cmsHost = CmsContext.RegisterHost();")
		$edit.Insert([Environment]::NewLine)

		$edit.Insert("		cmsHost.OnApplicationStart(this);}")
		$edit.Insert([Environment]::NewLine)
	}
	else
	{
		$edit = $methods.StartPoint.CreateEditPoint();
		$edit.LineDown()
		$edit.LineDown()
		$edit.Insert([Environment]::NewLine)
		$edit.Insert("		cmsHost = CmsContext.RegisterHost();")
		$edit.Insert([Environment]::NewLine)
	}

	#Update Application_BeginRequest
	$methods = $class.Children | where-object {$_.Name -eq "Application_BeginRequest"}
	if (!$methods)
	{
		$edit = $class.StartPoint.CreateEditPoint();
		$edit.LineDown()
		$edit.LineDown()
		$edit.Insert([Environment]::NewLine)
		$edit.Insert("	protected void Application_BeginRequest(){cmsHost.OnBeginRequest(this);}")
		$edit.Insert([Environment]::NewLine)
	}

	#Update Application_EndRequest
	$methods = $class.Children | where-object {$_.Name -eq "Application_EndRequest"}
	if (!$methods)
	{
		$edit = $class.StartPoint.CreateEditPoint();
		$edit.LineDown()
		$edit.LineDown()
		$edit.Insert([Environment]::NewLine)
		$edit.Insert("	protected void Application_EndRequest(){cmsHost.OnEndRequest(this);}")
		$edit.Insert([Environment]::NewLine)
	}

	#Update Application_Error
	$methods = $class.Children | where-object {$_.Name -eq "Application_Error"}
	if (!$methods)
	{
		$edit = $class.StartPoint.CreateEditPoint();
		$edit.LineDown()
		$edit.LineDown()
		$edit.Insert([Environment]::NewLine)
		$edit.Insert("	protected void Application_Error(){cmsHost.OnApplicationError(this);}")
		$edit.Insert([Environment]::NewLine)
	}

	#Update Application_End
	$methods = $class.Children | where-object {$_.Name -eq "Application_End"}
	if (!$methods)
	{
		$edit = $class.StartPoint.CreateEditPoint();
		$edit.LineDown()
		$edit.LineDown()
		$edit.Insert([Environment]::NewLine)
		$edit.Insert("	protected void Application_End(){cmsHost.OnApplicationEnd(this);}")
		$edit.Insert([Environment]::NewLine)
	}

	#Update Application_AuthenticateRequest
	$methods = $class.Children | where-object {$_.Name -eq "Application_AuthenticateRequest"}
	if (!$methods)
	{
		$edit = $class.StartPoint.CreateEditPoint();
		$edit.LineDown()
		$edit.LineDown()
		$edit.Insert([Environment]::NewLine)
		$edit.Insert("	protected void Application_AuthenticateRequest(object sender, EventArgs e){")
		$edit.Insert([Environment]::NewLine)

		$edit.Insert("		// Uncomment following source code for a quick Better CMS test if you don't have implemented users authentication. // Do not use this code for production!")
		$edit.Insert([Environment]::NewLine)

		$edit.Insert("		/* var roles = new[] { ""BcmsEditContent"", ""BcmsPublishContent"", ""BcmsDeleteContent"", ""BcmsAdministration"" };")
		$edit.Insert([Environment]::NewLine)

		$edit.Insert("		var principal = new GenericPrincipal(new GenericIdentity(""TestUser""), roles);")
		$edit.Insert([Environment]::NewLine)

		
		$edit.Insert("		HttpContext.Current.User = principal; */")
		$edit.Insert([Environment]::NewLine)

		$edit.Insert("		cmsHost.OnAuthenticateRequest(this);}")
		$edit.Insert([Environment]::NewLine)		
	}
}