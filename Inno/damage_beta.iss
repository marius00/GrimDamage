#define ApplicationVersion GetFileVersion('..\GrimDamage\bin\release\GrimDamage.exe')

[Setup]
AppVerName=Grim Dawn Damage Details
AppName=Grim Dawn Damage Details (c) EvilSoft
VersionInfoVersion={#ApplicationVersion}
AppId=gddmg
DefaultDirName={code:DefDirRoot}\Grim Dawn Damage Details
Uninstallable=Yes
OutputDir=..\Installer
SetupIconFile=gd.ico


[Tasks]
Name: desktopicon; Description: "Create a &desktop icon"; GroupDescription: "Icons:"
Name: starticon; Description: "Create a &startmenu icon"; GroupDescription: "Icons:"


[Icons]
Name: "{commonprograms}\Grim Dawn Damage Details"; Filename: "{app}\\GrimDamage.exe"; Tasks: starticon
Name: "{commondesktop}\Grim Dawn Damage Details"; Filename: "{app}\\GrimDamage.exe"; Tasks: desktopicon


[Files]
Source: "..\GrimDamage\bin\Release\*"; Excludes: "*.pdb, *.exe.config"; DestDir: "{app}"; Flags: overwritereadonly recursesubdirs createallsubdirs touch ignoreversion


[Run]
Filename: "{app}\dotNetFx45_Full_setup.exe"; Description: "Install .NET 4.5"; Flags: postinstall unchecked
Filename: "{app}\vcredist_x86.exe"; Parameters: "/install /quiet /norestart"; Description: "Install VC++ Redistributable 2013 (x86)"; Flags: runhidden runascurrentuser
Filename: "{app}\2010sp1_vcredist_x86.exe"; Parameters: "/install /quiet /norestart"; Description: "Install VC++ Redistributable 2010 SP1 (x86)"; Flags: runhidden runascurrentuser
Filename: "{app}\GrimDamage.exe"; Description: "Launch Grim Dawn Damage Details"; Flags: postinstall nowait


[Setup]
UseSetupLdr=yes
DisableProgramGroupPage=yes
DiskSpanning=no
AppVersion={#ApplicationVersion}
PrivilegesRequired=admin
DisableWelcomePage=Yes
AlwaysShowDirOnReadyPage=Yes
DisableDirPage=No
OutputBaseFilename=GDDamageDetailsBeta
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

