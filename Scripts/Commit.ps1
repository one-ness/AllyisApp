$curDate = Get-Date -Format "MM-dd-yyyy_HH.mm.ss"
$curCommit = git log -n1 --oneline
$lastCommit = type ..\curCommit.log
Write-Host $curDate + ": " + $curCommit + ": " + $lastCommit
if($curCommit -ne $lastCommit)
{
	Write-Host "Commiting"
    svn commit -m "$curDate"
	Write-Host "Tagging"
    git tag "Releases/$curDate"
	Write-Host "Pushing Tags"
    git push --tags
	$curCommit > ..\curCommit.log
}
exit 1