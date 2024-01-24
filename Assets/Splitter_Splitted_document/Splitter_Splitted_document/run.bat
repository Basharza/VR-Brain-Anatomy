@echo off
setlocal enabledelayedexpansion

for %%f in (MR-head_*.tiff) do (
    set "name=%%~nf"
    set "newname=!name:MR-head_=MR!"
    ren "%%f" "!newname!%%~xf"
)

endlocal
