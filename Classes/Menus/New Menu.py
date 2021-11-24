from datetime import date

menu_name = input("Enter the name of the menu: ")

men_str = "$menu::" + menu_name
men_str_space_fix = ""

menu_string = "if(isObject(" + men_str + "))"
menu_string += "\n\t" + men_str + ".delete();\n\n"

#$menu::Arrow_Base_On_Table = $class::menuSystem.newMenuObject("Arrow Base On Table","Place An Arrow Base Type");

menu_string +=  men_str + " = $class::menuSystem.newMenuObject(\"" + menu_name + "\",\"A menu entry\");\n\n"

menu_string += men_str + ".setName(\"" + menu_name + "Menu\");"
menu_string += "\n" + men_str + ".class = \"Menu\";"

menu_string += "\n\n//MENU INPUT " + men_str + ".addMenuItem(\"Menu Item\",\"" + men_str + ".showInputMenu(#CLIENT);\");"
menu_string += "\n\n//MENU REGULAR " + men_str + ".addMenuItem(\"Menu Item\",\"\");"

menu_string += "\n\n\npackage class_menu_" + menu_name + "\n{\n\tfunction _a(){}\n};\nactivatePackage(class_menu_" + menu_name +");"

menu_head = "//MENU " + menu_name + " Generated On " + str(date.today()) + "\n\n"

print(menu_head + menu_string)

file_name = "menu_" + menu_name + '.cs'
f = open(file_name, 'w+')
f.write(menu_head + menu_string)
f.close()

while 1:
    pass
