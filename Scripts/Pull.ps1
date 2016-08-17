Write-Host "Fetch"
git fetch
Write-Host "Delete old files"
foreach ($t in dir -Directory | Select-Object Name) {if($t.Name -ne "Scripts") { del -Force -Recurse $t.Name}}
Write-Host "Reset to master"
git reset --hard origin/master
exit 1