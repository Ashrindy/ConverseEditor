echo off

if exist ".\Release" (
	rmdir ".\Release" /s /q
)

mkdir .\Release
cd .\Release

mkdir .\linux-x64
mkdir .\win-x64

cd ..

cd "ConverseEditor"
dotnet publish -p:PublishProfile=linux-x64
dotnet publish -p:PublishProfile=win-x64

cd ..

cd .\Release

for /r %%F in (*.pdb) do (
	del %%F
)

ren .\linux-x64\libglfw.so.3 libglfw.so

robocopy ..\resources\ .\linux-x64\ /E
robocopy ..\resources\ .\win-x64\ /E

pushd linux-x64
7z a -mx=9 ..\ConverseEditor-linux-x64.7z *
popd

pushd win-x64
7z a -mx=9 ..\ConverseEditor-win-x64.7z *
popd

cd ..
