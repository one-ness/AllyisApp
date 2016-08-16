$curDate = Get-Date -Format "MM-dd-yyyy_HH.mm.ss"
$curCommit = git log -n1 --oneline
$lastCommit = type ..\curCommit.log
if($curCommit -ne $lastCommit)
{
    svn commit -m "$curDate"
    git tag "Releases/$curDate"
    git push --tags
	$curCommit > ..\curCommit.log
}
exit 1