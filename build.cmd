cd librato4net

msbuild librato4net.csproj /t:Build /p:Configuration="Release 4.0"
msbuild librato4net.csproj /t:Build;Package /p:Configuration="Release 4.5"

cd ..