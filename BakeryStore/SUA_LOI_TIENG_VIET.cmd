@echo off
chcp 65001 >nul
cd /d "%~dp0"
if not exist "BakeryStore.csproj" (
  echo KHONG TIM THAY BakeryStore.csproj o thu muc nay.
  echo Hay chep 2 file SUA_LOI_TIENG_VIET.cmd va FixUTF8.ps1
  echo vao THU MUC BakeryStore co file BakeryStore.csproj, sau do chay lai.
  pause
  exit /b 1
)
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0FixUTF8.ps1"
echo.
pause
