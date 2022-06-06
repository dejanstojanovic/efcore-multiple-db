$workingFolder = $args[0]
$solution = $args[1]

$processrocessInfo = New-Object System.Diagnostics.ProcessStartInfo
$processrocessInfo.FileName = "dotnet"
$processrocessInfo.RedirectStandardError = $true
$processrocessInfo.RedirectStandardOutput = $true
$processrocessInfo.UseShellExecute = $false
$processrocessInfo.WorkingDirectory = $workingFolder
$packageListCommand = 'list ./' + $solution + ' package --vulnerable --include-transitive'

$output=''
$summary='# Nuget package vulnerability scan summary';

    $processrocessInfo.Arguments = $packageListCommand
    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $processrocessInfo
    $process.Start() | Out-Null
    $stdout = $process.StandardOutput.ReadToEnd()
    $stderr = $process.StandardError.ReadToEnd()
    $process.WaitForExit()
    $output = $output + $stdout + [Environment]::NewLine


echo $output

$leadingString = '   > '
$outputLines = $output -split [Environment]::NewLine
$vulnerabilityDetected = $false
foreach ($line in $outputLines) {

    if($line.StartsWith('Project ')){
        $projectName = $line.Split('`')[1]
        $summary = $summary + [Environment]::NewLine + [Environment]::NewLine + '## ' + $projectName + [Environment]::NewLine + [Environment]::NewLine
        $summary = $summary + "| Transitive Package | Resolved | Severity | Advisory URL |" + [Environment]::NewLine
        $summary = $summary + "|:--------|:--------:|--------|--------|"
    }

    if($line.StartsWith($leadingString)){
        $packageLine =  $line.Substring($leadingString.Length)

        $columns = $packageLine.Split(" ")
        $row = "| "
        $columnCount = 0
        foreach($column in $columns){
            if($column.Length -gt 1 ){
                $columnCount = $columnCount + 1
                $value = $column.Trim()
                if($columnCount -eq 4){
                    $value = "[" + $value + "](" + $value + ")"
                }
                $row = $row + $value + " |"
            }
        }

        $summary = $summary + [Environment]::NewLine + $row


        if($packageLine.Contains(" High ") -or $packageLine.Contains(" Critical ") -or $packageLine.Contains(" Moderate ")){

            if($vulnerabilityDetected -eq $false){
                $vulnerabilityDetected = $true
            }
        }
    }
}


if($vulnerabilityDetected){
    # Write to file
    $summaryPath = $workingFolder + "/nuget-vulnerabilty-report.md"
    Set-Content -Path $summaryPath -Value $summary

    # Push to Azure DevOps
    $summaryOut = "##vso[task.uploadsummary]" + $summaryPath
    Write-Host $summaryOut

    throw "Vulnerability detected!"
}