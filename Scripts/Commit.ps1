$curDate = Get-Date -Format "MM-dd-yyyy_HH.mm.ss"
$curCommit = git log -n1 --oneline
$lastCommit = type ..\..\curCommit.log
if($curCommit -ne $lastCommit)
{
    $curCommit > ..\..\curCommit.log
    svn commit -m "$curDate"
    git tag "Release_$curDate"
    git push --tags
}
exit 1