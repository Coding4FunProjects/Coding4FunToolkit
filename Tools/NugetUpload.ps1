$versionNumber = "2.0.0";
$solutionName = "Coding4Fun.Toolkit.sln";

$zipFileName = "Coding4Fun.Toolkit ({0}).zip";
$zipFullPaths = @();

$platforms = @("Windows Phone 7", "Windows Phone 8", "Windows Store");
$platformTargets = @("", "AnyCpu", "AnyCpu");

$root = Split-Path -parent $MyInvocation.MyCommand.Definition

$currentPath = [System.IO.Directory]::GetParent($root).FullName;
$slnPath = [System.IO.Path]::Combine($currentPath, "source", $solutionName);
$contentDirectory =  [System.IO.Path]::Combine($currentPath, "bin", "content");
$contentDirectory = new-object System.IO.DirectoryInfo $contentDirectory;

$releaseDir = [System.IO.Directory]::GetParent($root).GetDirectories("bin", [System.IO.SearchOption]::TopDirectoryOnly)[0];
$releaseDir = $releaseDir.GetDirectories("Release", [System.IO.SearchOption]::TopDirectoryOnly)[0];
$releaseDirs = @();

foreach($platform in $platforms)
{
	$zipFullPaths += @([System.IO.Path]::Combine($root, [string]::Format($zipFileName, $platform)));
}

for ($index = 0; $index -lt $platforms.Count; $index++) 
{
	$platform = $platforms[$index];
	$platformTarget = $platformTargets[$index];
	
	$path = [System.IO.Path]::Combine($releaseDir.FullName, $platform, $platformTarget);
	$dir = new-object System.IO.DirectoryInfo $path;
	$releaseDirs += @($dir);
}

$assemblyFiles = [System.IO.Directory]::GetFiles(
	[System.IO.Path]::Combine($currentPath, "source"),
	"AssemblyInfo.cs", [System.IO.SearchOption]::AllDirectories);
	
$nuspecFiles = [System.IO.Directory]::GetFiles($currentPath, "nuget/*.nuspec", [System.IO.SearchOption]::TopTopDirectoryOnly);

echo "AssemblyInfo.cs Count: " $assemblyFiles.Length
echo "NuSpec Count: " $nuspecFiles.Length;
echo "Path: " $currentPath;
echo "Sln: " $slnPath;

foreach($file in $assemblyFiles)
{
	$text = [System.IO.File]::ReadAllText($file);
	$text = $text -replace "(?<=\[assembly: Assembly(File)?Version\(`")([\d\.]+)(?=`"\)\])", $versionNumber;
	[System.IO.File]::WriteAllText($file, $text);
}

foreach($file in $nuspecFiles)
{
	$text = [System.IO.File]::ReadAllText($file);
	$text = $text -replace "(?<=<version>)([\d\.]+)(?=</version>)", $versionNumber;
	[System.IO.File]::WriteAllText($file, $text);
}

echo "Updated all files to " $versionNumber;
echo "starting build"

# can't seem to get this to properly work
#[void][System.Reflection.Assembly]::Load('Microsoft.Build.Engine, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a');
#$project = New-Object Microsoft.Build.BuildEngine.Project($engine);
#
#$engine.RegisterLogger((New-Object Microsoft.Build.BuildEngine.ConsoleLogger));
#
#$project.Load($slnPath);
#$project.SetProperty("Configuration", "Release");
#
#$success = $project.Build();
#
#$engine.UnregisterAllLoggers();

cmd /c C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe $slnPath /p:Configuration=Release /verbosity:quiet /fl /t:Rebuild

if($LastExitCode -ne 0)
{
	echo "BUILD FAILURE!!"
	exit 0
}
echo "done building"

function AppendToZip($zipPkg, $targetZipFile, $appendPath)
{
	$uriLocation = "/" + $appendPath + $targetZipFile.Name;
	
	$partName = new-object System.Uri($uriLocation, [System.UriKind]::Relative);
	$part = $zipPkg.CreatePart($partName, "application/octet-stream", [System.IO.Packaging.CompressionOption]"Maximum");
	
	$stream = $part.GetStream();	
	$bytes = [System.IO.File]::ReadAllBytes($targetZipFile.FullName);
	$stream.Write($bytes, 0, $bytes.Length);
	$stream.Dispose();
}

[System.Reflection.Assembly]::Load("WindowsBase, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")
if($zipPkg -ne $null) 
{
	$zipPkg.Close();
}

for ($index = 0; $index -lt $platforms.Count; $index++) 
{
	$releaseDir = $releaseDirs[$index];
	$zipFullPath = $zipFullPaths[$index];
	$platform = $platforms[$index];
	
	$zipPkg = [System.IO.Packaging.ZipPackage]::Open($zipFullPath, [System.IO.FileMode]"Create", [System.IO.FileAccess]"ReadWrite");

	#creating relative URI
	$files = $releaseDir.GetFiles("*.dll");
	$files += $releaseDir.GetFiles("*.pri");
	
	foreach($file In $files)
	{
		AppendToZip $zipPkg $file "";
	}

	#images for picker page
	if($platform -eq "Windows Phone 7" -or 
		$platform -eq "Windows Phone 8")
	{
		$images = $contentDirectory.GetFiles("*.png", [System.IO.SearchOption]::AllDirectories);
		
		foreach($file In $images)
		{
			AppendToZip $zipPkg $file "Toolkit.Content/";
		}
	}
	
	# winstore requires theme file
	if($platform -eq "Windows Store")
	{
		$xamlFiles = $releaseDir.GetFiles("generic.xaml", [System.IO.SearchOption]::AllDirectories);
		
		if($xamlFiles.Count -eq 1)
		{
			AppendToZip $zipPkg $xamlFiles[0] "themes/";
		}
	}

	#Close the package when we're done.
	$zipPkg.Close();
}

echo "start nuget packaging"

cd $root
$env:Path = $root + ";" + $env:Path

cd ../nuget
del *.nupkg

foreach($file in $nuspecFiles)
{
	nuget 'pack' $file '-BasePath' '../bin'
}

echo "done nuget packaging"
echo "start nuget push"

$nupkgFiles = [System.IO.Directory]::GetFiles($currentPath, "*.nupkg", [System.IO.SearchOption]::AllDirectories);
foreach($file in $nupkgFiles)
{
	echo $file;
	nuget 'push' $file
}

echo "done nuget push"