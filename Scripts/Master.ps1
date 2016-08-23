#Master.ps1
Start-Transcript -Append -Path ..\ExecLog.log -IncludeInvocationHeader

#Pull.ps1
Write-Output "Fetch"
git fetch
Write-Output "Delete old files"
foreach ($t in dir -Directory | Select-Object Name) {if($t.Name -ne "Scripts") { del -Force -Recurse $t.Name}}
Write-Output "Reset to master"
git reset --hard origin/master

Write-Output "Pull Finished"

$curDate = Get-Date -Format "yyyy/MM/dd_HH.mm.ss"
$curCommit = git log -n1 --oneline
$lastCommit = type ..\curCommit.log

if($curCommit -ne $lastCommit)
{
    Write-Output "Begin Build" "Clean"

    #Build.ps1
    &"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo /noconlog .\src\Main\AllyisApps.sln /t:Clean /p:Configuration=Release
    Write-Output "Restore"
    &"F:\WorkingDir\nuget.exe" restore .\src\Main\AllyisApps.sln
    Write-Output "Build"
    $result = &"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo .\src\Main\AllyisApps.sln /t:Build /p:Configuration=Release
    Write-Output $result
    $res = ""
    foreach($s in $result) {if($s.Contains("FAILED")) {$res+=$s}}    

    if($res -EQ "") 
    {
	    Write-Output "Build Succeeded" $curDate + ": " + $curCommit + ": " + $lastCommit
        #Commit.ps1
        Write-Output "Commiting"
        svn cleanup
        $curStatus = svn status
        
        foreach($s in $curStatus)
        {
            if($s[0] -eq "?")
            {
                svn add $s --non-interactive
                Write-Output "Add" $s
            }
            elseif($s[0] -eq "!")
            {
                svn delete $s --non-interactive
                Write-Output "Delete" $s
            }
        }

        svn commit -m "$curDate" | Write-Output
        Write-Output "Tagging"
        git tag "Releases/$curDate" | Write-Output
        Write-Output "Pushing Tags"

        $gitJob = Start-Job  -ErrorAction SilentlyContinue {
        git push --tags
        }
    
        Wait-Job -Id $gitJob.Id -Timeout 5

        if($gitJob.State -ne "Completed")
        {
            Get-Job -Id $gitJob.Id | Remove-Job -Force
            Write-Output "Tag push failed after 5 seconds"
            Write-Output $gitJob.State
            Remove-Job $gitJob.Id
        }
        $curCommit > ..\curCommit.log

        #Deploy.ps1
	    Write-Output "Commit Finished:"
        svn status | Write-Output

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
        Write-Output "Copying Finished"
    }
    else 
    {
            Write-Output "Failed on Build"
    } 
 }
 else
 {
    Write-Output "No changes to branch" "Nothing to commit/deploy"
 }

Stop-Transcript