PowerShell -NonInteractive -NoProfile -Command {Scripts\Pull.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -ne 1) { exit "Failed on Pull" } else { Write-Debug "Pull Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Build.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Build" } else { Write-Debug "Build Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Commit.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Commit" } else { Write-Debug "Commit Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Deploy.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Deploy" } else { Write-Debug "Deploy Succeeded"}