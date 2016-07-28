PowerShell -NonInteractive -NoProfile -Command {Scripts\Pull.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -ne 1) { exit "Failed on Pull" }
PowerShell -NonInteractive -NoProfile -Command {Scripts\Build.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Build" }
#PowerShell -NonInteractive -NoProfile -Command {Scripts\Commit.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Commit" }
PowerShell -NonInteractive -NoProfile -Command {Scripts\Deploy.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -NE 1) { exit "Failed on Deploy" }