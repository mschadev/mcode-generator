## B.0.1.1
- Change from gcc to mingw-w64(gcc is for `linux`)  
- Example script regular expression bug fix
```autohotkey
if (!regexmatch(mcode, " ^ ([0 - 9] +),(" c ":|.*?," c ":)([^,] +)", m))
```
There was a __space__ in the script created.  
This prevents the BentschiMCode function from operating normally.
