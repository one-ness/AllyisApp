#Master.ps1
Start-Transcript -Append -Path ..\ExecLog.log -IncludeInvocationHeader

#Pull.ps1
Write-Host "Fetch"
git fetch
Write-Host "Delete old files"
foreach ($t in dir -Directory | Select-Object Name) {if($t.Name -ne "Scripts") { del -Force -Recurse $t.Name}}
Write-Host "Reset to master"
git reset --hard origin/master

Write-Host "Pull Finished`r`nBegin Build`r`nClean"

#Build.ps1
&"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo /noconlog .\src\Main\AllyisApps.sln /t:Clean /p:Configuration=Release
Write-Host "Restore"
&"F:\WorkingDir\nuget.exe" restore .\src\Main\AllyisApps.sln
Write-Host "Build"
$result = &"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo .\src\Main\AllyisApps.sln /t:Build /p:Configuration=Release
Write-Host $result
$res = ""
foreach($s in $result) {if($s.Contains("FAILED")) {$res+=$s}}

    $curDate = Get-Date -Format "MM-dd-yyyy_HH.mm.ss"
    $curCommit = git log -n1 --oneline
    $lastCommit = type ..\curCommit.log

if($res -EQ "" -and $curCommit -ne $lastCommit ) 
{
	Write-Host "Build Succeeded`r`n" + $curDate + ": " + $curCommit + ": " + $lastCommit
    #Commit.ps1
    Write-Host "Commiting"
    svn commit -m "$curDate"
    Write-Host "Tagging"
    git tag "Releases/$curDate"
    Write-Host "Pushing Tags"
    git push --tags
    $curCommit > ..\curCommit.log

    #Deploy.ps1
	Write-Host "Commit Finished"
	$srcPath = ".\src\Main\aaweb\"
    $destPath = "c:\aa\"

    Write-Host "Copying Files"
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
        Write-Host "No changes to branch`r`nNothing to commit/deploy"
    }
    else
    {
        Write-Host "Failed on Build"
    }
 } 

Stop-Transcript