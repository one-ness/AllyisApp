Write-Host "Clean"
&"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo /noconlog .\src\Main\AllyisApps.sln /t:Clean /p:Configuration=Release
Write-Host "Restore"
&"F:\WorkingDir\nuget.exe" restore .\src\Main\AllyisApps.sln
Write-Host "Build"
$result = &"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /m /nologo .\src\Main\AllyisApps.sln /t:Build /p:Configuration=Release
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