Write-Host " >>> Deleting bin (1/5)" -ForegroundColor Green
Remove-Item './bin/Debug' -Recurse

Write-Host " >>> Restoring Client (2/5)" -ForegroundColor Green
dotnet restore

Write-Host " >>> Building Client (3/5)" -ForegroundColor Green
dotnet build

Write-Host " >>> Publish Client (4/5)" -ForegroundColor Green
dotnet publish -c Debug /p:UseAppHost=false

Write-Host " >>> Running Client (5/5)" -ForegroundColor Green
cd bin/Debug/net6.0/publish
dotnet Client.dll