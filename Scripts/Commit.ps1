$curDate = Get-Date
svn commit -m "$curDate"
git tag Release -m "Committed to SVN: $curDate"
git push --tags
exit 1