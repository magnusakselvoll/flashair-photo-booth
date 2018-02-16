param(
    [Parameter(Mandatory=$true)]
    [String]$Source,
    [Parameter(Mandatory=$true)]
    [String]$Destination,
    [Parameter(Mandatory=$false)]
    [String]$Filter = '*.jpg',
    [Parameter(Mandatory=$false)]
    [timespan]$RefreshInterval = (New-TimeSpan -Seconds 10),
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

while ($stopwatch.Elapsed -lt $MaximumExecutionTime)
{
    $iteration++
    Write-Host "Starting iteration $iteration" -ForegroundColor Green

    $remoteFiles = Get-ChildItem -Path $Source -Filter $Filter -File

    foreach ($remoteFile in $remoteFiles)
    {
        if ($completedFiles[$remoteFile.Name])
        {
            Write-Warning "File '$($remoteFile.Name)' already downloaded. Skipping."
            continue
        }

        Write-Host "Downloading file '$($remoteFile.Name)'"

        $remoteFile | Copy-Item -Destination $Destination

        if ($DeleteFromSource)
        {            
            $filesToDelete.Enqueue($remoteFile.Name)
        }

        $completedFiles[$remoteFile.Name] = $true
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
            $destinationFilePath = Join-Path $Destination $fileName
            if (-not (Test-Path $destinationFilePath))
            {
                Write-Warning "Won't delete '$sourceFilePath'. File does not exist on destination."
                continue
            }
            $sourceFile = Get-Item $sourceFilePath
            $destinationFile = Get-Item $destinationFilePath

            if ($sourceFile.Length -ne $destinationFile.Length)
            {
                Write-Warning "'$sourceFilePath' and '$destinationFilePath' have different length. Repeating copy."
                $completedFiles.Remove($fileName)
                continue
            }
            $sourceFile.Delete()
            Write-Host "Deleted file '$sourceFilePath'"
        }
    }

    Write-Host "Finished iteration $iteration. Sleeping for $secondsToSleep s." -ForegroundColor Green
    Write-Host ''
    Start-Sleep -Seconds $secondsToSleep
}