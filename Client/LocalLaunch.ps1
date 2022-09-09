Remove-Item './bin/Debug' -Recurse
dotnet restore
dotnet build
dotnet publish -c Debug /p:UseAppHost=false
cd bin/Debug/net6.0/publish
dotnet Client.dll