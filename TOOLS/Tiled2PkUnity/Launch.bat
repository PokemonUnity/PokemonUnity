@echo off
IF [%1]==[] (
    echo You must drag and drop your tilemap's JSON onto this batch script.
    echo Otherwise, execute Tiled2PkUnity in the command prompt with your arguments.
    pause
) ELSE (
    node tiled2pkunity.js %1
    pause
)