:: deploya First Phase

@echo off
echo EXPL_CREATE_BOOT
:: RAMDISK OPTION
%systemroot%\System32\bcdedit.exe /create {ramdiskoptions} /d "AutoInit - Reinstall Windows"
%systemroot%\System32\bcdedit.exe /set {ramdiskoptions} ramdisksdidevice partition=c:
%systemroot%\System32\bcdedit.exe /set {ramdiskoptions} ramdisksdipath \AutoInit\boot.sdi

:: CREATE A NEW BOOT ENTRY FOR RETIRE UTILITY
for /f "tokens=3" %%A in ('%systemroot%\System32\bcdedit.exe -create /d "AutoInit - Reinstall Windows" /application OSLOADER') do set guid1=%%A
echo The GUID is %guid1% > C:\AutoInit\guid

:: SET WHERE THE LOCAL RETIRE BOOT.WIM IS LOCATED
%systemroot%\System32\bcdedit.exe /set %guid1% device ramdisk=[C:]\AutoInit\deploya.wim,{ramdiskoptions}
%systemroot%\System32\bcdedit.exe /set %guid1% path \windows\system32\boot\winload.exe
%systemroot%\System32\bcdedit.exe /set %guid1% osdevice ramdisk=[C:]\AutoInit\deploya.wim,{ramdiskoptions}

:: CREATE A BOOT ENTRY FOR WINDOWS
%systemroot%\System32\bcdedit.exe /set %guid1% systemroot \Windows
%systemroot%\System32\bcdedit.exe /set %guid1% winpe yes
%systemroot%\System32\bcdedit.exe /set %guid1% detecthal yes
%systemroot%\System32\bcdedit.exe /displayorder %guid1% /addlast

:: THIS SETS BOOT TO BE DEFAULT
%systemroot%\System32\bcdedit.exe /default %guid1%

:: SETS TIME TO 5 SECOND
%systemroot%\System32\bcdedit.exe /timeout 5

endlocal
echo 1 > C:\CloudInstall.dat
echo EXPL_REBOOT
:: EXIT WITH SUCCESS
shutdown /r /f /t 3
exit /b 0