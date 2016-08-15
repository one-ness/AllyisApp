msbuild /m /nologo /noconlog .\src\Main\aaweb\AllyisApps.csproj /t:Clean /p:Configuration=Release
$result = msbuild /m /nologo .\src\Main\aaweb\AllyisApps.csproj /t:Build /p:Configuration=Release
foreach($s in $result) {if($s.Contains("FAILED")) {$res+=$s}}
if($res -eq "")
{
	exit 1
}
else
{
	exit 0
}