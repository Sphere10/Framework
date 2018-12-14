@echo off
SET /P repo=Please enter GitHub Url:
if "%repo%"=="" GOTO Error

git init
git remote add origin %repo%
git fetch origin
git checkout -b master --track origin/master
del xxx.bat
git add --a
git status

GOTO End
:Error
ECHO Bad Url

:End