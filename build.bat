msbuild KtermMonitor\KtermMonitor.csproj -p:Configuration=Release;Platform=AnyCPU -t:rebuild 
pause

dotnet publish KtermMonitor\KtermMonitor.csproj -p:Configuration=Release;Platform=AnyCPU --output bin/Release/publish
