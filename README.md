# InstantZip
Zip everything, without wasting our time

## Drag and drop
Drag the file you want to zip and drop it on InstantZip.exe

## Shell
Write ./InstantZip.exe with the files you want to zip in a shell
Like: `"./InstantZip.exe" ..\Release\net6.0\publish\*\ -d ./Out/ -c s`

Usage:
 - InstantZip files... [-d destination] [-c compression-level]

Arguments:
- -d destination directory/filename
- -c compression level
   -  n - No compression should be performed on the file.
   -  f  - The compression operation should complete as quickly as possible.
   -  o - The compression operation should be optimally compressed.
   -  s - The compression operation should create output as small as possible.

Made under 5 hours
