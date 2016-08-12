msbuild /m /nologo /noconlog .\src\Main\AllyisApps.sln /t:Clean
$result = msbuild /m /nologo .\src\Main\AllyisApps.sln /t:Build
foreach($s in $result) {if($s.Contains("FAILED")) {$res+=$s}}
if($res -eq "")
{
	exit 1
}
else
{
	exit 0
}