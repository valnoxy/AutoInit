<h1 align="center"><br><img src="https://dl.exploitox.de/other/AutoInit.png" alt="AutoInit Logo" width=175px></h1>

<h3 align="center">AutoInit</h3>
<p align="center">
    Configure Windows automatically
    <br />
    <strong>Version: </strong>1.1.0
    <br />
    <br />
    <a><strong>No downloads available</strong></a>
    <br />
    <br />
    <a href="https://github.com/valnoxy/AutoInit/issues">Report Bug</a>
    ·
    <a href="https://github.com/valnoxy/AutoInit/discussions/">Discussions</a>
  </p>
</p>

![-----------------------------------------------------](https://dl.exploitox.de/t440p-oc/rainbow.png)

## 🔔 Information
AutoInit is a little application for initializing a Windows machine without Image modification. 


- Switch to Administrator by activating the account and removing the "User"-Account.
- Remove Bloatware apps that are shipped with Windows.
- Install applications like Firefox, Adobe Reader and Remote Management Control with winget.
- Plays music when opening the application (Typical music from cracking software).
- Clean reinstall Windows without CD/USB.

⚠ **This tool is designed to use in a internal environment. If you want to use this tool as a "template" or just for testing, you will need to compile it for yourself.**

## ℹ️ Disclaimer
> This application will modify the system. I won't be responsible for any damage you've done yourself trying to use this application.

## ⚙️ Compiling the source code
For compiling, you'll need ```Visual Studio 2022``` and ```.NET 6.0```.
Clone this source and restore the NUGET Packages.

## 📜 Configuation

**Filename**: config.ini
```
[AutoInit]
AdminPassword = 

; Note that this user will be deleted (if not set otherwise) after the Administration switching phase.
DefaultUsername = User
RemoveDefaultUser = true

BackgroundMusic = true

[Applications]
PackageID_Firefox = Mozilla.Firefox
PackageID_AcrobatReader = Adobe.Acrobat.Reader.64-bit
InstallFirefox = true
InstallAcrobatReader = true

EnableSMB1 = true
EnableNET35 = true

RemoteMaintenance = true
RemoteMaintenanceURL = https://example.com
RemoteMaintenanceFileName = Remote Maintenance.exe

; Install other apps by inserting the Package ID of the application.
; You can find the Package ID by typing 'winget search <Name>' or by searching on https://winget.run/
; Use ',' for more than one application
OtherApps =  

[Settings]
DisableTelemetry = true
CheckActivation = true

; System Protection
; Set to 0% to disable system protection
SystemProtection = 20%
DisableAutoRebootAfterBSOD = true

; Options:
;   None = None
;   Complete = Complete memory dump
;   Kernel = Kernel memory dump
;   Small = Small memory dump (64 KB)
;   Auto = Automatic memory dump
SPMaxMemoryDump = Small

; Power Settings
SetMaxPerformance = true
DisableFastBoot = true
```
You can also use ```AutoInit.exe --new-config``` for creating a new empty configuration.

## 📖 Credits
 - **goblinfactory**: Simple console library - [Konsole](https://github.com/goblinfactory/konsole)
 - **NAudio**: C# Audio and MIDI library - [NAudio](https://github.com/naudio/NAudio)
 - **PrivacyIsSexy**: Bloatware removal script - [Privacy.Sexy](https://privacy.sexy/)

## 🧾 License
AutoInit is licensed under [MIT License](https://github.com/valnoxy/AutoInit/blob/main/LICENSE). So you are allowed to use freely and modify the application. I will not be responsible for any outcome. Proceed with any action at your own risk.

```Copyright (c) 2018 - 2022 valnoxy. By Jonas G. <jonas@exploitox.de> ```
