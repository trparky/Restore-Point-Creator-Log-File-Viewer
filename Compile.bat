MSBuild.exe "Restore Point Creator Exported Log Viewer.sln" /noconsolelogger /t:Rebuild /p:Configuration=Debug
MSBuild.exe "Restore Point Creator Exported Log Viewer.sln" /noconsolelogger /t:Rebuild /p:Configuration=Release
cscript "C:\Users\Tom\OneDrive\My Visual Studio Projects\MessageBox.vbs" message "Compile complete." "Compile Progress"