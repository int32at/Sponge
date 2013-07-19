New-Item -ItemType Directory -Force -Path ..\lib\tmp
write-host "created tmp dir"

gci ..\src\ -recurse -include *.dll -exclude ('*Tests.dll', 'Sponge.dll')   | Copy-Item -destination ..\lib\tmp\ -force
write-host "copied dlls to tmp dir"

$rel = resolve-path ..\lib\
$dlls = resolve-path ..\lib\tmp\Sponge*.dll
..\src\packages\ilmerge.2.13.0307\ilmerge /wildcards /out:$rel\Sponge.dll $dlls
write-host "merge successfull"

Remove-Item -Recurse -Force ..\lib\tmp
write-host "deleted tmp dir"