param(
    [Parameter(Mandatory=$true)]
    [String]$InterfaceName,
    [Parameter(Mandatory=$true)]
    [String]$ProfileName,
    [Parameter(Mandatory=$false)]
    [timespan]$RefreshInterval = (New-TimeSpan -Seconds 30),
    [Parameter(Mandatory=$false)]
    [timespan]$MaximumExecutionTime = (New-TimeSpan -Hours 24)
)

Write-Verbose "Interface: $InterfaceName, Profile: $ProfileName, Refresh: $RefreshInterval, MaxExec: $MaximumExecutionTime"

function Invoke-NetShellWlan
{
    param([string[]] $Params)

    $result = &{netsh.exe wlan $Params}

    return $result
}

Write-Host "Attempting to maintain connection to profile $ProfileName on interface $InterfaceName every $RefreshInterval";

$stopwatch =  [system.diagnostics.stopwatch]::StartNew()
$secondsToSleep = $([int]($RefreshInterval.TotalSeconds))

Write-Verbose "Each iteration will sleep for $secondsToSleep s."

$iteration = 0
$reconnects = 0

while ($stopwatch.Elapsed -lt $MaximumExecutionTime)
{
    $iteration++
    Write-Host "Starting iteration $iteration" -ForegroundColor Green

    $consoleLines = Invoke-NetShellWlan "show", "interfaces"

    $consoleLines | Write-Verbose
    $interfaceDetected = $false
    $connected = $false

    foreach ($line in $consoleLines)
    {
        $parts = $line.Split(':')
        if ($parts.Length -ne 2)
        {
            continue
        }

        $key = $parts[0].Trim()
        $value = $parts[1].Trim()

        if ($key -eq 'Name')
        {
            $interfaceDetected = ($value -eq $InterfaceName)
            continue
        }

        if (-not $interfaceDetected)
        {
            continue
        }

        if ($key -eq 'State')
        {
            $connected = $value -eq 'connected'
            break;
        }
    }

    if (-not $interfaceDetected)
    {
        Write-Warning "Interface $InterfaceName not detected"
    }

    Write-Host "Interface $InterfaceName connected: $connected"

    if (-not $connected)
    {
        Invoke-NetShellWlan "connect", "name=$ProfileName", "interface=$InterfaceName" | Write-Verbose
        Write-Host 'Attempted reconnection'
        $reconnects++
    }

    Write-Host "Finished iteration $iteration. Reconnects: $reconnects. Sleeping for $secondsToSleep s." -ForegroundColor Green
    Write-Host ''
    Start-Sleep -Seconds $secondsToSleep
}