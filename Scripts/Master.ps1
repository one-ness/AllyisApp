#Master.ps1
Start-Transcript -Append -Path ..\ExecLog.log -IncludeInvocationHeader

#Pull.ps1
Write-Output "Fetch"
git fetch
Write-Output "Delete old files"
foreach ($t in dir -Directory | Select-Object Name) {if($t.Name -ne "Scripts") { del -Force -Recurse $t.Name}}
Write-Output "Reset to master"
git reset --hard origin/master

Write-Output "Pull Finished" "Begin Build" "Clean"


#Build.ps1
&"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo /noconlog .\src\Main\AllyisApps.sln /t:Clean /p:Configuration=Release
Write-Output "Restore"
&"F:\WorkingDir\nuget.exe" restore .\src\Main\AllyisApps.sln
Write-Output "Build"
$result = &"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo .\src\Main\AllyisApps.sln /t:Build /p:Configuration=Release
Write-Output $result
$res = ""
foreach($s in $result) {if($s.Contains("FAILED")) {$res+=$s}}

    $curDate = Get-Date -Format "MM-dd-yyyy_HH.mm.ss"
    $curCommit = git log -n1 --oneline
    $lastCommit = type ..\curCommit.log

if($res -EQ "" -and $curCommit -ne $lastCommit ) 
{
	Write-Output "Build Succeeded" $curDate + ": " + $curCommit + ": " + $lastCommit
    #Commit.ps1
    Write-Output "Commiting"
    svn commit -m "$curDate"
    Write-Output "Tagging"
    git tag "Releases/$curDate"
    Write-Output "Pushing Tags"

    $gitJob = Start-Job  -ErrorAction SilentlyContinue {
    git push --tags
    }
    Get-Job -Id $gitJob.Id | Wait-Job $gitJob.Id  -Timeout 5

    if($gitJob.State -ne "Completed")
    {
        Get-Job -Id $gitJob.Id | Remove-Job -Force
        Write-Output "Tag push failed after 5 seconds"
        Receive-Job -Id $gitJob.Id | Write-Output
        Remove-Job $gitJob.Id
    }
    $curCommit > ..\curCommit.log

    #Deploy.ps1
	Write-Output "Commit Finished"
	$srcPath = ".\src\Main\aaweb\"
    $destPath = "c:\aa\"

    Write-Output "Copying Files"
    Copy-Item -Path ($srcPath + "App_Data\") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "Areas\") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "bin\") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "Content\") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "fonts\") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "Scripts\") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "Views\") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "favicon.ico") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "Global.asax") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "packages.config") -Destination $destPath -Recurse -Force
    Copy-Item -Path ($srcPath + "Parameters.xml") -Destination $destPath -Recurse -Force
    Copy-Item -Path ("c:\Web.config") -Destination $destPath -Recurse -Force
}
else 
{
    if($curCommit -eq $lastCommit)
    {
        Write-Output "No changes to branch"
        Write-Output "Nothing to commit/deploy"
    }
    else
    {
        Write-Output "Failed on Build"
    }
 } 

Stop-Transcript