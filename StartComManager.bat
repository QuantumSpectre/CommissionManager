@echo off
cd /d "%~dp0PublishedProjects\CommissionManager.GUI"
start "" "CommissionManager.GUI.exe"

cd /d "%~dp0PublishedProjects\CommissionManager.API"
start "" "CommissionManagerAPP.exe"