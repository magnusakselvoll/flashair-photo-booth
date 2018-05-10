param(
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

$stopwatch =  [system.diagnostics.stopwatch]::StartNew()
$secondsToSleep = $([int]($RefreshInterval.TotalSeconds))
$iteration = 0
$completedFiles = @{}
[System.Collections.Queue]$filesToDelete = @()
$publishedFileCounter = 0

function Get-PublishFolder ([String] $baseFolder, [int] $filesPerSubfolder, [int] $fileNumber) {
    if (-not (Test-Path($baseFolder)))
    {
        New-Item -Path $baseFolder -ItemType Directory > $null
    }

    if ($filesPerSubfolder -le 0)
    {
        return $baseFolder
    }

    $folderNumber = [int] [Math]::Floor($fileNumber / $filesPerSubfolder)
    $folderName = "$($folderNumber * $filesPerSubfolder)-$((($folderNumber+1) * $filesPerSubfolder) - 1)"

    $publishFolder = "$baseFolder\$folderName"

    if (-not (Test-Path($publishFolder)))
    {
        New-Item -Path $publishFolder -ItemType Directory > $null
    }

    return $publishFolder
}
 
while ($stopwatch.Elapsed -lt $MaximumExecutionTime)
{
    $iteration++
    Write-Host "Starting iteration $iteration" -ForegroundColor Green

    $remoteFiles = Get-ChildItem -Path $Source -Filter $Filter -File

    foreach ($remoteFile in $remoteFiles)
    {
        if ($completedFiles[$remoteFile.Name])
        {
            Write-Verbose "File '$($remoteFile.Name)' already downloaded. Skipping."
            continue
        }

        $tokenFilePath = "$Destination\$CompleteNamePattern" -f $remoteFile.Name
        if (Test-Path($tokenFilePath))
        {
            Write-Warning "File '$($remoteFile.Name)' already downloaded in previous session. Skipping."
            $completedFiles[$remoteFile.Name] = $true
            continue
        }

        Write-Host "Downloading file '$($remoteFile.Name)'"

        $remoteFile | Copy-Item -Destination $Destination
        $copiedFile = Get-Item "$Destination\$($remoteFile.Name)"
        
        $publishFolder = Get-PublishFolder $PublishDirectory $PublishFilesPerSubfolder $publishedFileCounter

        while (Test-Path("$publishFolder\$PublishNamePattern" -f "$publishedFileCounter$($copiedFile.Extension)"))
        {
            $publishedFileCounter++;
            $publishFolder = Get-PublishFolder $PublishDirectory $PublishFilesPerSubfolder $publishedFileCounter
        }

        $publishPath = "$publishFolder\$PublishNamePattern" -f "$publishedFileCounter$($copiedFile.Extension)"
        Write-Verbose "Moving file to $publishPath"

        $copiedFile | Move-Item -Destination $publishPath
        New-Item $tokenFilePath | Write-Verbose
        Write-Verbose "Created token file at $tokenFilePath"
        

        if ($DeleteFromSource)
        {            
            $filesToDelete.Enqueue($remoteFile.Name)
        }

        $completedFiles[$remoteFile.Name] = $true
        $publishedFileCounter++
    }

    if ($DeleteFromSource)
    {
        while ($filesToDelete.Count -gt 0)
        {
            $fileName = $filesToDelete.Dequeue()
            $sourceFilePath = Join-Path $Source $fileName
            if (-not (Test-Path $sourceFilePath))
            {
                Write-Warning "Unable to delete '$sourceFilePath'. File does not exist."
                continue
            }
            Remove-Item $sourceFilePath
            Write-Host "Deleted file '$sourceFilePath'"
        }
    }

    Write-Host "Finished iteration $iteration. Sleeping for $secondsToSleep s." -ForegroundColor Green
    Write-Host ''
    Start-Sleep -Seconds $secondsToSleep
}