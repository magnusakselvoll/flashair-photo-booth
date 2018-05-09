param(
    [Parameter(Mandatory=$false)]
    [String]$version = 'current'
)

if (Test-Path $version)
{
    Remove-Item -Path $version -Recurse
}

New-Item -Path "$version" -ItemType Directory
New-Item -Path "$version\flashair-downloader" -ItemType Directory
New-Item -Path "$version\flashair-slideshow" -ItemType Directory
New-Item -Path "$version\wifi-utilities" -ItemType Directory
Copy-Item -Path "..\flashair-downloader\Start-FlashAirDownloader.ps1" -Destination "$version\flashair-downloader"
Copy-Item -Path "..\flashair-slideshow\bin\Release\*" -Destination "$version\flashair-slideshow"
Copy-Item -Path "..\wifi-utilities\Connect-WifiAuto.ps1" -Destination "$version\wifi-utilities"
