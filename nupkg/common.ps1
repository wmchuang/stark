# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

function Write-Info   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Black -BackgroundColor Green

	try 
	{
	   $host.UI.RawUI.WindowTitle = $text
	}		
	catch 
	{
		#Changing window title is not suppoerted!
	}
}

function Write-Error   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Red -BackgroundColor Black 
}

function Seperator   
{
	Write-Host ("_" * 100)  -ForegroundColor gray 
}

   

function Read-File {
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $filePath
    )
		
	$pathExists = Test-Path -Path $filePath -PathType Leaf
	if ($pathExists)
	{
		return Get-Content $filePath		
	}
	else{
		Write-Error  "$filePath path does not exist!"
	}
}

# List of solutions
$solutions = (
    "framework",
	"service/Modules"
)

# List of projects
$projects = (

# Starter 
	 "framework/Stark.Starter.Cap",
	 "framework/Stark.Starter.Core",
	 "framework/Stark.Starter.DDD",
	 "framework/Stark.Starter.Job",
	 "framework/Stark.Starter.Redis",
	 "framework/Stark.Starter.Web",
	 "framework/Stark.Starter.Work.Weixin",
	 
# Modue
	 "service/Modules/Stark.Module.AI",
	 "service/Modules/Stark.Module.Inf",
	 "service/Modules/Stark.Module.System"
)
