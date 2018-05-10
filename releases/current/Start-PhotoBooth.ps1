param(
    [Parameter(Mandatory=$true)]
    [String]$FlashAirInterface,
    [Parameter(Mandatory=$true)]
    [String]$FlashAirProfile,
    [Parameter(Mandatory=$true)]
    [String]$InternetInterface,
    [Parameter(Mandatory=$true)]
    [String]$InternetProfile,
    [Parameter(Mandatory=$true)]
    [String]$Source,
    [Parameter(Mandatory=$true)]
    [String]$Destination,
    [Parameter(Mandatory=$true)]
    [String]$PublishDirectory,
    [Parameter(Mandatory=$false)]
    [String]$PublishNamePattern = '{0}',
    [Parameter(Mandatory=$false)]
    [int]$PublishFilesPerSubfolder = 100,
    [Parameter(Mandatory=$false)]
    [String]$CompleteNamePattern = '{0}.complete',
    [Parameter(Mandatory=$false)]
    [String]$Filter = '*.jpg',
    [Parameter(Mandatory=$false)]
    [timespan]$RefreshInterval = (New-TimeSpan -Seconds 5),
    [Parameter(Mandatory=$false)]
    [timespan]$MaximumExecutionTime = (New-TimeSpan -Hours 24),
    [Parameter(Mandatory=$false)]
    [Switch]$DeleteFromSource = $false

)

$verbose = ($VerbosePreference -eq "Continue")

Start-Process -FilePath 'powershell.exe' -ArgumentList ".\wifi-utilities\Connect-WifiAuto.ps1 -InterfaceName `"$FlashAirInterface`" -ProfileName `"$FlashAirProfile`" -Verbose:`$$verbose" -Verb Open
Start-Process -FilePath 'powershell.exe' -ArgumentList ".\wifi-utilities\Connect-WifiAuto.ps1 -InterfaceName `"$InternetInterface`" -ProfileName `"$InternetProfile`" -Verbose:`$$verbose" -Verb Open
Start-Process -FilePath 'flashair-slideshow.exe' -WorkingDirectory ".\flashair-slideshow" -Verb Open
. .\flashair-downloader\Start-FlashAirDownloader.ps1 -Source $Source -Destination $Destination -PublishDirectory $PublishDirectory -PublishNamePattern $PublishNamePattern -PublishFilesPerSubfolder $PublishFilesPerSubfolder -CompleteNamePattern $CompleteNamePattern -Filter $Filter -RefreshInterval $RefreshInterval -MaximumExecutionTime $MaximumExecutionTime -DeleteFromSource:$DeleteFromSource -Verbose:$verbose
