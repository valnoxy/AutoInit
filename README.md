<h1 align="center"><br><img src="https://dl.exploitox.de/other/AutoInit.png" alt="WhatsApp Desktop Dark Mode" width=175px></h1>

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
AdminPW=null
RemoteMaintenance=<LinkToExe>
PackageID_Firefox=Mozilla.Firefox
PackageID_AcrobatReader=Adobe.Acrobat.Reader.64-bit
DotNet=dism /Online /Enable-Feature /All /FeatureName:NetFx3 /NoRestart
SMB=dism /Online /Enable-Feature /All /FeatureName:SMB1Protocol /NoRestart
```

## 📖 Credits
 - **goblinfactory**: Simple console library - [Konsole](https://github.com/goblinfactory/konsole)
 - **NAudio**: C# Audio and MIDI library - [NAudio](https://github.com/naudio/NAudio)
 - **PrivacyIsSexy**: Bloatware removal script - [Privacy.Sexy](https://privacy.sexy/)

## 🧾 License
AutoInit is licensed under [MIT License](https://github.com/valnoxy/AutoInit/blob/main/LICENSE). So you are allowed to use freely and modify the application. I will not be responsible for any outcome. Proceed with any action at your own risk.

```Copyright (c) 2018 - 2022 valnoxy. By Jonas G. <jonas@exploitox.de> ```
