$versionNumber = "1.3.2";

$root = Split-Path -parent $MyInvocation.MyCommand.Definition

$currentPath = [System.IO.Directory]::GetParent($root).FullName;

$slnPath = [System.IO.Path]::Combine($currentPath, "Coding4Fun.Phone.sln");

$assemblyFiles = [System.IO.Directory]::GetFiles($currentPath, "AssemblyInfo.cs", [System.IO.SearchOption]::AllDirectories);
$nuspecFiles = [System.IO.Directory]::GetFiles($currentPath, "*.nuspec", [System.IO.SearchOption]::AllDirectories);

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
#$engine = New-Object Microsoft.Build.BuildEngine.Engine;
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

cmd /c C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe $slnPath /p:Configuration=Release

if($LastExitCode -ne 0)
{
	echo "BUILD FAILURE!!"
	exit 0
}
	
echo "done building"

echo "start nuget packaging"

cd $root
$env:Path = $root + ";" + $env:Path

cd ../bin/nuget
del *.nupkg

foreach($file in $nuspecFiles)
{
	nuget 'pack' $file '-b' '../'
}

echo "done nuget packaging"
echo "start nuget push"

$nupkgFiles = [System.IO.Directory]::GetFiles($currentPath, "*.nupkg", [System.IO.SearchOption]::AllDirectories);
foreach($file in $nupkgFiles)
{
	nuget 'push' $file
}

echo "done nuget push"