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

foreach ($line in $outputLines) {

    if($line.StartsWith($leadingString)){
        $packageLine =  $line.Substring($leadingString.Length)

        if($packageLine.Contains(" High ") -or $packageLine.Contains(" Critical ") -or $packageLine.Contains(" Moderate ")){
            throw "Vulnerability detected!"
        }
    }
}