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
