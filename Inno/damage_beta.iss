#define ApplicationVersion GetFileVersion('..\GrimDamage\bin\release\GrimDamage.exe')

[Setup]
AppVerName=Grim Damage
AppName=Grim Damage (c) EvilSoft
VersionInfoVersion={#ApplicationVersion}
AppId=gddmg
DefaultDirName={code:DefDirRoot}\Grim Damage
Uninstallable=Yes
OutputDir=..\Installer
SetupIconFile=gd.ico


[Tasks]
Name: desktopicon; Description: "Create a &desktop icon"; GroupDescription: "Icons:"
Name: starticon; Description: "Create a &startmenu icon"; GroupDescription: "Icons:"


[Icons]
Name: "{commonprograms}\Grim Damage"; Filename: "{app}\\GrimDamage.exe"; Tasks: starticon
Name: "{commondesktop}\Grim Damage"; Filename: "{app}\\GrimDamage.exe"; Tasks: desktopicon


[Files]
Source: "..\GrimDamage\bin\Release\*"; Excludes: "*.pdb, *.exe.config"; DestDir: "{app}"; Flags: overwritereadonly recursesubdirs createallsubdirs touch ignoreversion


[Run]
Filename: "{app}\dotNetFx45_Full_setup.exe"; Description: "Install .NET 4.5"; Flags: postinstall unchecked
Filename: "{app}\vcredist_x86.exe"; Parameters: "/install /quiet /norestart"; Description: "Install VC++ Redistributable 2013 (x86)"; Flags: runhidden runascurrentuser
Filename: "{app}\2010sp1_vcredist_x86.exe"; Parameters: "/install /quiet /norestart"; Description: "Install VC++ Redistributable 2010 SP1 (x86)"; Flags: runhidden runascurrentuser
Filename: "{app}\GrimDamage.exe"; Description: "Launch Grim Damage"; Flags: postinstall nowait


[Setup]
UseSetupLdr=yes
DisableProgramGroupPage=yes
DiskSpanning=no
AppVersion={#ApplicationVersion}
PrivilegesRequired=admin
DisableWelcomePage=Yes
AlwaysShowDirOnReadyPage=Yes
DisableDirPage=No
OutputBaseFilename=GrimDamageInstaller
LicenseFile=license.txt

[UninstallDelete]
Type: filesandordirs; Name: {app}

[Languages]
Name: eng; MessagesFile: compiler:Default.isl

[Code]
function IsRegularUser(): Boolean;
begin
Result := not (IsAdminLoggedOn or IsPowerUserLoggedOn);
end;

function DefDirRoot(Param: String): String;
begin
if IsRegularUser then
Result := ExpandConstant('{localappdata}')
else
Result := ExpandConstant('{pf}')
end;

