set path=%CD%;%PATH%;
cd ../bin/nuget
for /r %%x in (*.nupkg) do nuget push "%%x"