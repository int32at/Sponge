gci ..\src\ -recurse -include *.dll -exclude *Tests* | Copy-Item -destination ..\bin\
write-host copied all dlls to bin

$rel = resolve-path ..\bin\release
$dlls = resolve-path ..\bin\Sponge*.dll
..\src\packages\ilmerge.2.13.0307\ilmerge /wildcards /out:$rel\Sponge.dll $dlls