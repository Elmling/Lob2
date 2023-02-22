in menusml.cs:
	$menumarkuppath is where it will look for .txt files of
	menus.
	
	$menuMarkupPathFileCreation is where it will create the
	new menu.cs file created from the markup


Steps:
1. execute this file
2. call the function parse_files(); which will look for and create
	any menus you've created using the markup language

example:
-t test > this is a test menu
-e toast > mnu-test2-mnu

-t test2 > does this work?
-e yes > talk("xD");

we created a menu titled test, with the heading text on the menu as
"this is a test menu", with a single entry, that calls our second menu.
our second menu is titled test2, and it has a single entry that
calls the function talk("xD"), you can still use #CLIENT to reference 
the client.

then we use -e to add entries users can select

to call another menu, use the syntax: mnu-mymenuname-mnu

