@echo off

start dotnet build --project ".\CommissionManagerApp\CommissionManagerAPP.csproj"

start dotnet build --project ".\CommissionManager.Gui\CommissionManager.GUI.csproj"


start dotnet run --project ".\CommissionManagerApp\CommissionManagerAPP.csproj"

start dotnet run --project ".\CommissionManager.Gui\CommissionManager.GUI.csproj"
