set path=%CD%;%PATH%;
cd ../bin/nuget
del *.nupkg

for /r %%x in (*.nuspec) do nuget pack "%%x" -b ../
for /r %%x in (*.nupkg) do nuget push "%%x"