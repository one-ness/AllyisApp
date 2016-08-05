$curDate = Get-Date
svn commit -m "$curDate"
git tag "Release - $curDate"
git push --tags
exit 1