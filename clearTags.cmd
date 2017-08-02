FOR /f %%a in ('git tag -l') DO git tag -d %%a 
git fetch
FOR /f %%a in ('git tag -l') DO git push --delete origin %%a 
FOR /f %%a in ('git tag -l') DO git tag -d %%a 
pause
