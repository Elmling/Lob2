// exec("base/lob2/classes/menusml/menusml.cs"); exec("base/lob2/classes/string.cs");exec("base/lob2/classes/arrays.cs");

$menuMarkupPath = "base/lob2/classes/menusML/";
$menuMarkupPathFileCreation = "base/lob2/classes/menus/";

function parse_files() {
    setmodpaths(getmodpaths());
    %p = $menuMarkupPath;
    for(%i = findFirstFile(%p @ "*.txt"); %i !$= ""; %i=findNextFile(%p @ "*.txt")) {
        %fs = %i;
        if(%fs !$= "") {
            %arr = parse_logic(%fs);
            parse_writeToFile(%arr, strReplace(filename(%fs),".txt",""));
        }
    }
}

function parse_logic(%file) {
    %fs = %file;
    %f = new fileObject();
    %f.openForRead(%fs);
    %l = "";
    %lc = -1;
    %arr = $class::arrays.create("");
    %arr.meta("file", %file);
    %otherData = $class::arrays.create("");
    %title = "";
    while(!%f.isEOF()) {
        %l[%lc++] = %f.readline();
        %l = %l[%lc];
        $class::string.set(%l);

        if($class::string.startswith("-t")) {
            %split = $class::string.split(">");
            %title = %split.index(0);
            %title = trim(strReplace(%title,"-t",""));
            
            %v = %split.index(1);
            
            // echo("title: " @ %title @ " value: " @ %v);
            
            %arr.push("$menu::" @ %title @ " = $class::menuSystem.newMenuObject(\"" @ %title @ "\",\"" @ %v @ "\");");
            if(%arr.meta("headers1") $= "") {
                %arr.meta("headers1", "if(isObject($menu::" @ %title @ ")) $menu::" @ %title @ ".delete();");
                %arr.meta("headers2", "$menu::" @ %title @ ".setName(\"" @ %title @ "\"); $menu::" @ %title @ ".class = \"Menu\";");
            } else {
                %arr.meta_data["headers1"] = %arr.meta_data["headers1"] SPC "if(isObject($menu::" @ %title @ ")) $menu::" @ %title @ ".delete();";
                %arr.meta_data["headers2"] = %arr.meta_data["headers2"] SPC "$menu::" @ %title @ ".setName(\"" @ %title @ "\"); $menu::" @ %title @ ".class = \"Menu\";";
            }
            
        } else if($class::string.startswith("-e")) {
            %split = $class::string.split(">");
            %entry = %split.index(0);
            %entry = trim(strReplace(%entry,"-e",""));
            %v = %split.index(1);
            echo("entry value: " @ %entry); 
            if($class::string.set(%v).contains("mnu-")) {
                %v = strReplace(%v, "mnu-","%m=$class::menuSystem.getmenu(\\\"");
                %v = strReplace(%v,"-mnu","\\\");%m.showmenu(#CLIENT,0);");
            } else {
                %v = strReplace(%v,"\"","\\\"");
            }
            %arr.push("$menu::" @ %title @ ".addMenuItem(\"" @ %entry @ "\",\"" @ %v @ "\");");
        } else {
            %otherData.push(%l);
        }
    }
    // echo("msg: " @ %arr.toString());
    %f.close();
    %f.delete();
    %arr.meta("otherData", %otherData);
    return %arr;
}

function parse_writeToFile(%arr, %name) {
    %otherData = %arr.meta("otherData");
    %f = new fileObject();
    %f.openForWrite($menuMarkupPathFileCreation @ "menu_" @ %name @ ".cs");
    %f.writeLine(%arr.meta("headers1"));
    %first = false;
    %titlecount = 0;
    for(%i=0;%i<%arr.getCount();%i++) {
        %o = %arr.getObject(%i);
        %f.writeLine(%o.value);
        //if(%first == false){
        //    %f.writeLine(%arr.meta("headers2"));
        //    %first = true;
        //}
    }
    %f.writeLine(%arr.meta("headers2"));
    %first = false;
    for(%i=0;%i<%otherData.getCount(); %i++) {
        if(!%first) {
            %first = true;
            %f.writeLine(" ");
        }
        %o = %otherData.getObject(%i);
        %f.writeLine(%o.value);
    }
    %f.close();
    %f.delete();
    exec($menuMarkupPathFileCreation @ %name @ ".cs");
}