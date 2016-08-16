Start-Transcript -Append -Path ..\ExecLog.log -IncludeInvocationHeader
PowerShell -NonInteractive -NoProfile -Command {Scripts\Pull.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -ne 1) { exit "Failed on Pull" } else { Write-Host "Pull Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Build.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Build" } else { Write-Host "Build Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Commit.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Commit" } else { Write-Host "Commit Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Deploy.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Deploy" } else { Write-Host "Deploy Succeeded"}
Stop-Transcript