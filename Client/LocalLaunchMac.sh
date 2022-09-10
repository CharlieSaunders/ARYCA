#!/bin/bash
rm -r bin
dotnet build
dotnet publish -c Debug /p:UseAppHost=false
cd bin/Debug/net6.0/publish
dotnet Client.dll