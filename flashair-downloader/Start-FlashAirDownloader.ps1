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
    [Switch]$DeleteFromSource = $false,
    [Parameter(Mandatory=$false)]
    [timespan]$DeleteDelay = (New-TimeSpan -Seconds 0)
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
            $deleteObject = New-Object -TypeName psobject -Property `
                (@{
                    'SourceFile'=$remoteFile;
                    'TimeStamp'=Get-Date
                })
            $filesToDelete.Enqueue($deleteObject)
        }

        $completedFiles[$remoteFile.Name] = $true
    }

    if ($DeleteFromSource)
    {
        $deleteCutoffTimeStamp = (Get-Date).Subtract($DeleteDelay)
        
        while ($filesToDelete.Count -gt 0 -and $filesToDelete.Peek().TimeStamp -lt $deleteCutoffTimeStamp)
        {
            $deleteObject = $filesToDelete.Dequeue()
            $filesToDelete.SourceFile | Remove-Item
            Write-Host "Deleted file '$($filesToDelete.SourceFile.Name)'"
        }
    }

    Write-Host "Finished iteration $iteration. Sleeping for $secondsToSleep s." -ForegroundColor Green
    Write-Host ''
    Start-Sleep -Seconds $secondsToSleep
}

