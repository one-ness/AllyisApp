#Use this to delete tags from within Source Trees Terminal Window currenlty 
#Delete local tags.
FOR /f %a in ('git tag -l') DO git tag -d %a 
#Fetch remote tags.
git fetch
#Delete remote tags.
FOR /f %a in ('git tag -l') DO git push --delete origin %a 
#Delete local tasg.
FOR /f %a in ('git tag -l') DO git tag -d %a 

