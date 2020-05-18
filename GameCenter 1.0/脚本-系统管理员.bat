@echo off

SP.StudioCore\Tools\curl -k https://api.a8.to/Permission -F "file=@Web.System\Properties\Permission.xml" -H "action:XML" -t utf-8  > SystemAdmin.tmp

for /f %%i in ("SystemAdmin.tmp") do set size=%%~zi
if %size% equ 0 (
	echo 发生错误
	del SystemAdmin.tmp
	
	exit;
) else (
	echo 下载成功
	move /Y SystemAdmin.tmp "Web.System\Properties\Permission.xml"	
)
SP.StudioCore\Tools\curl -k https://api.a8.to/Permission -F "file=@Web.System\Properties\Permission.xml" -H "action:CS" -H "name:Permission" -t utf-8  > SystemAdmin.tmp
for /f %%i in ("SystemAdmin.tmp") do set size=%%~zi
if %size% equ 0 (
	echo 发生错误
	del SystemAdmin.tmp
	exit;
) else (
	echo 下载成功
	move /Y SystemAdmin.tmp "Web.System\Utils\Permission.cs"	
)

