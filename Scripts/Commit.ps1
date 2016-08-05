$curDate = Get-Date
$curCommit = git log -n1 --oneline
$lastCommit = type ..\..\curCommit.log
if($curCommit -ne $lastCommit)
{
    $curCommit > ..\..\curCommit.log
    svn commit -m "$curDate"
    git tag "Release - $curDate"
    git push --tags
}
exit 1