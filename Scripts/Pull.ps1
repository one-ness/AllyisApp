git fetch
foreach ($t in dir -Directory | Select-Object Name) {if($t.Name -ne "Scripts") { del -Force -Recurse $t.Name}}
git reset --hard origin/master
exit 1