&"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo /noconlog .\src\Main\aaweb\AllyisApps.csproj /t:Clean /p:Configuration=Release
$result = &"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo .\src\Main\aaweb\AllyisApps.csproj /t:Build /p:Configuration=Release
Write-Host $result
foreach($s in $result) {if($s.Contains("FAILED")) {$res+=$s}}
if($res -eq "")
{
	exit 1
}
else
{
	exit 0
}