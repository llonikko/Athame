#define ProductName "Athame"
#define ProductExec "Athame.exe"
#define ProductVersion GetFileVersion(AddBackslash(SourcePath) + ProductEntry)

[Setup]
AppName={#ProductName}
AppVersion={#ProductVersion}
DefaultDirName={pf}\{#ProductName}
DefaultGroupName={#ProductName}
Compression=lzma2
SolidCompression=yes
LicenseFile={#file AddBackslash(SourcePath) + "..\Athame\Resources\Licenses.rtf"}

[Icons]
Name: "{group}\{#ProductName}"; Filename: "{app}\{#ProductExec}"
