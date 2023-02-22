
if(!isObject($class::arrays)) {
	$class::arrays = new scriptGroup() {
		class = arrays;
	};
}

// can use multiple args as array items, or provide 1 argument with
// using commas as a delimiter of a single string "item1,item2,item3,..etc"
function arrays::create(%this,%a0,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9) {
	%array = false;
	if(%a1 $= "") {
		%csv = %a0;
		%w = strreplace(%csv," ","_sP_");
		%w = strreplace(%w,","," ");
		
		%array = new scriptGroup() {
			class = array;
		};

		for(%i=0;%i<getWordCount(%w); %i++) {
			%_w = getWord(%w,%i);
			%_w = ltrim(%_w);
			%_w = rtrim(%_w);
			%word_obj = new scriptObject() {
				value = strReplace(%_w,"_sP_"," ");
			};
			%array.add(%word_obj);
		}
		%this.add(%array);
	} else {
		%array = new scriptGroup() {
			class = array;
		};
		for(%i=0;%i<10;%i++) {
			if(%a[%i] !$= "") {
				%o = new scriptObject() {
					value = rtrim(ltrim(%a[%i]));
				};
				%array.add(%o);
			}
		}
		%this.add(%array);
	}
	return %array;
}

// array...
function array::meta(%this,%key, %data) {
	if(%data $= "")
		return %this.meta_data[%key];
	%this.meta_data[%key] = %data;
	return %this;
}

function array::display(%this) {
	for(%i=0;%i<%this.getCount();%i++) {
		echo(%this.getObject(%i).value);
	}
}

function array::toString(%this) {
	%b = "";
	for(%i=0;%i<%this.getCount();%i++) {
		if(%i == 0) {
			%b = %this.getObject(%i).value;
		} else {
			%b = %b @ "," @ %this.getObject(%i).value;
		}
	}
	return %b;
}

function array::index(%this,%index,%as_obj) {
	if(%as_obj $= "")
		return ltrim(rtrim(%this.getObject(%index).value));
	else
		return %this.getObject(%index);
}

function array::getRandom(%this,%as_obj) {
	if(%this.getcount() == 0) {
		return false;
	}
	if(%as_obj)
		return %this.getObject(getRandom(0,%this.getCount()-1));
	else
		return ltrim(rtrim(%this.getObject(getRandom(0,%this.getCount()-1)).value));
}

function array::get(%this,%val) {
	for(%i=0;%i<%this.getCount();%i++) {
		%o = %this.getObject(%i);
		if(%o.value $= %val)
			return %o;
	}
	return false;
}

function array::contains(%this,%val) {
	for(%i=0;%i<%this.getCount();%i++) {
		%o = %this.getObject(%i);
		if(%o.value $= %val)
			return true;
	}
	return false;
}

function array::pop_last(%this) {
	%this.remove(%this.getObject(%this.getCount()-1));
	return %this;
}

function array::pop_first(%this) {
	%this.remove(%this.getObject(0));
	return %this;
}

function array::push_first(%this,%obj) {
	if(!isObject(%obj)) {
		%v = %obj;
		%obj = new scriptObject() {
			value =  %v;
		};
	}
	%this.add(%obj);
	%this.bringToFront(%obj);
	return %this;
}

// same as push
function array::push_last(%this,%obj) {
	if(!isObject(%obj)) {
		%v = %obj;
		%obj = new scriptObject() {
			value =  %v;
		};
	}
	%this.add(%obj);
	return %this;
}

// same as push last
function array::push(%this,%obj) {
    if(!isObject(%obj)) {
		%v = %obj;
		%obj = new scriptObject() {
			value =  %v;
		};
	}
	%this.add(%obj);
	return %this;
}
