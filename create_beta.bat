copy inno\vcredist_x64.exe GrimDamage\bin\Release\
copy inno\vcredist_x86.exe GrimDamage\bin\Release\
copy inno\dotNetFx45_Full_setup.exe GrimDamage\bin\Release\
copy inno\2010sp1_vcredist_x86.exe GrimDamage\bin\Release\

copy /y grimdamage\bin\release\GrimDamage.exe Installer\GDDamage.exe

Inno\iscc Inno\damage_beta.iss