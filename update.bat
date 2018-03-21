@echo off
SET PAINT_NET_PATH="C:\Program Files\paint.net\FileTypes"
SET MAP_DLL_PATH="MapFileType\MapFileType\bin\Debug\MapFileType.dll"
SET CODEC_DLL_PATH="MapFileType\MapFileType\bin\Debug\MapFileCodec.dll"

Echo Paint.NET path is %PAINT_NET_PATH%
Echo -
Echo Copying Map DLL
Echo Map DLL path is %MAP_DLL_PATH%
Copy /Y %MAP_DLL_PATH% %PAINT_NET_PATH%
Echo -
Echo Map Codec DLL path is %CODEC_DLL_PATH%
Copy /Y %CODEC_DLL_PATH% %PAINT_NET_PATH%
Echo -
Echo -

