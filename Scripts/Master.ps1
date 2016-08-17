Start-Transcript -Append -Path ..\ExecLog.log -IncludeInvocationHeader
.\Scripts\Pull.ps1
if($LASTEXITCODE -EQ 1) 
{ 
	Write-Host "Pull Succeeded"
	.\Scripts\Build.ps1
	if($LASTEXITCODE -EQ 1) 
	{
		Write-Host "Build Succeeded"
		.\Scripts\Commit.ps1
		if($LASTEXITCODE -EQ 1) 
		{ 
			Write-Host "Commit Succeeded"
			.\Scripts\Deploy.ps1
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