Start-Transcript -Append -Path ..\ExecLog.log -IncludeInvocationHeader
PowerShell -NonInteractive -NoProfile -Command {Scripts\Pull.ps1; exit $LASTEXITCODE}
if($LASTEXITCODE -EQ 1) 
{ 
	Write-Host "Pull Succeeded"
	PowerShell -NonInteractive -NoProfile -Command {Scripts\Build.ps1; exit $LASTEXITCODE}
	if($LASTEXITCODE -EQ 1) 
	{
		Write-Host "Build Succeeded"
		PowerShell -NonInteractive -NoProfile -Command {Scripts\Commit.ps1; exit $LASTEXITCODE}
		if($LASTEXITCODE -EQ 1) 
		{ 
			Write-Host "Commit Succeeded"
			PowerShell -NonInteractive -NoProfile -Command {Scripts\Deploy.ps1; exit $LASTEXITCODE}
			if($LASTEXITCODE -EQ 1)
			{
				Write-Host "Deploy Succeeded"
			}
			else { Write-Host "Failed on Deploy"; exit } 
		}
		else { Write-Host "Failed on Commit"; exit } 
	}
	else { Write-Host "Failed on Build" } 
}
else { Write-Host "Failed on Pull" } 

Stop-Transcript