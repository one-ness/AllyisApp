In order to be able to run the app locally you will need to:
1. Open the solution in Visual Studio.
2. Right click on AllyisApps.Database and publish it.
3. Add a new log in for aaUser in your sql server and have it have access to AllyisApps.Database DB. 
note: The connection string has User Id=aaUser;password=BlueSky23#
so if you give it a different password rememeber to update it on the connection string.

Please install SSDTSetup:(after installing SMS)
http://download.microsoft.com/download/E/E/4/EE429DFD-563B-4CC7-BF9F-EF9449818192/EN/SSDTSetup.exe

use images in 
SMSInstall Help for help installing SQL Managment Studio


Install Web Essentials and make sure you grab an appropriate update for Visual Studio
(Update 5 for VS2013 as of writing this).
**Make sure you have .NET 3.5. If not, download a copy of Windows 10,
(https://www.microsoft.com/en-us/software-download/windows10)
mount the .iso onto your optical drive, and run in an elevated prompt:

Dism /online /enable-feature /featurename:NetFX3 /All /Source:D:\sources\sxs /LimitAccess



Configurations for allowing Visual Studio/IIS Express to bind to a non-localhost alias.

(1) Change IIS Express and Visual Studio to always run as admin.

Run regedit.
In HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers,
add two string values with Name and Data (without quotes):

"C:\Program Files (x86)\IIS Express\iisexpress.exe"                              "~ RUNASADMIN"
"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe"     "~ RUNASADMIN"

* The key for Layers might not exist, in that case, simply create a new key to contain the above string values.
Also, an additional value for

"C:\Program Files (x86)\Common Files\Microsoft Shared\MSEnv\VSLauncher.exe"      "~ RUNASADMIN"

may be required.


(2)
(a) Change binding config in applicationhost.config to look like the following, except for the id.
FILE LOCATION: C:\Users\[current user]\Documents\IISExpress\config\applicationhost.config

	<site name="AllyisApps" id="1">
	    <application path="/" applicationPool="Clr4IntegratedAppPool">
	        <virtualDirectory path="/" physicalPath="C:\Users\user1\aa\main\AllyisApps" />
	    </application>
	    <bindings>
	        <binding protocol="http" bindingInformation="*:62707:" />
	        <binding protocol="http" bindingInformation="*:62707:www.apps.local" />
	    </bindings>
	    <applicationDefaults applicationPool="Clr2IntegratedAppPool" />
	</site>


(b) Add www.apps.local AND apps.local -> 127.0.0.1 in hosts.
FILE LOCATION: C:\Windows\System32\drivers\etc\hosts

127.0.0.1    apps.local
127.0.0.1    www.apps.local


(c) Also add [subdomain].apps.local -> 127.0.0.1 in hosts.

127.0.0.1    stringoftesting.apps.local

*NOTE: The hosts file does not support wildcard subdomains, so you'll need to add more subdomains as we go for
local development. Failure to do so will result in your browser being unable to resolve to localhost (http 400).
Also, the first time you try to visit your local development server, you may have to type in www.apps.local:62707,
but subsequent requests will not require you to do so.
