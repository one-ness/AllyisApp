Start-Transcript -Append -Path ..\ExecLog.log -IncludeInvocationHeader
PowerShell -NonInteractive -NoProfile -Command {Scripts\Pull.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -ne 1) { Write-Host "Failed on Pull"; exit } else { Write-Host "Pull Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Build.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { Write-Host "Failed on Build"; exit } else { Write-Host "Build Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Commit.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { Write-Host "Failed on Commit"; exit } else { Write-Host "Commit Succeeded"}
PowerShell -NonInteractive -NoProfile -Command {Scripts\Deploy.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { Write-Host "Failed on Deploy"; exit } else { Write-Host "Deploy Succeeded"}
Stop-Transcript