$wp = "(windowsphone)";
$wp7 = "(windowsphone7)";
$wp8 = "(windowsphone8)";
$winStore = "(windowsstore)";

$targetPlatform = "";

($args.Length -gt 0)
	$targetPlatform = $args[0];

if(
	$targetPlatform -ne $wp7 -or
	$targetPlatform -ne $wp8 -or
	$targetPlatform -ne $winStore)
{
	$targetPlatform = $winStore;
}

$root = Split-Path -parent $MyInvocation.MyCommand.Definition
$basePath = [System.IO.Directory]::GetParent($root).FullName;

$xamlFiles = [System.IO.Directory]::GetFiles($basePath, "*.xaml", [System.IO.SearchOption]::AllDirectories);

$genericXaml = "";
$innerXamlData = "";
$platStyleXamlData = "";
$commonStyleXamlData = "";

foreach($file in $xamlFiles)
{
#	$text = [System.IO.File]::ReadAllText($file);
#	$text = $text -replace "(?<=\[assembly: Assembly(File)?Version\(`")([\d\.]+)(?=`"\)\])", $versionNumber;
	[System.Xml.XmlDocument] $xamlFile = new-object System.Xml.XmlDocument
	$xamlFile.load($file)

	$nodelist = $xamlFile.selectnodes("/ResourceDictionary") # XPath is case sensitive
	foreach ($testCaseNode in $nodelist) 
	{
	}
}

#[System.IO.File]::WriteAllText($file, $text);
echo "Created generic.xaml for " $targetPlatform;