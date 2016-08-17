$srcPath = ".\src\Main\aaweb\"
$destPath = "c:\aa\"

Write-Host "Copying Files"
Copy-Item -Path ($srcPath + "App_Data\") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "Areas\") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "bin\") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "Content\") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "fonts\") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "Scripts\") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "Views\") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "favicon.ico") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "Global.asax") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "packages.config") -Destination $destPath -Recurse -Force
Copy-Item -Path ($srcPath + "Parameters.xml") -Destination $destPath -Recurse -Force
Copy-Item -Path ("c:\aa\Web.config") -Destination $destPath -Recurse -Force

exit 1