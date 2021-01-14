REM Recursively deletes all files located in all bin & obj folders in executed directory, then deletes bin & obj folders.
for /d /r . %%d in (bin,obj) do @if exist "%%d" DEL /s/q "%%d"
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s/q "%%d"