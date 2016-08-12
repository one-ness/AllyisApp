$srcPath = ".\src\Main\aaweb\"
$destPath = "c:\aa\"

Copy-Item -Path ($srcPath + "App_Data\") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "Areas\") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "bin\") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "Content\") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "fonts\") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "Scripts\") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "Views\") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "favicon.ico") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "Global.asax") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "packages.config") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "Parameters.xml") -Destination $destPath -Recurse
Copy-Item -Path ($srcPath + "Web.config") -Destination $destPath -Recurse

exit 1