if(isObject($class::dictionary)) {
	$class::dictionary.delete();
}

$class::dictionaries = new scriptGroup() {
	class = "dictionaries";
};

function dictionaries::create(%this,%markup) {
	%dict = new scriptGroup() {
		class = dictionary;
		parent = dictionaries;
	};
	%markup = strreplace(%markup," ","_sP_");
	%markup = strreplace(%markup,","," ");
	for(%i=0;%i<getWordCount(%markup); %i++) {
		%_w = getWord(%markup,%i);
		%_w = ltrim(%_w);
		%_w = rtrim(%_w);
		
		%nw = strreplace(%_w,"="," ");
		%word_array = $class::arrays.create(strReplace(getWord(%nw,0),"_sP_"," "), strReplace(getWord(%nw,1),"_sP_"," "));
		// %word_array.display();
		%dict.add(%word_array);
		//%array.add(%word_obj);
	}
	
	%this.add(%dict);
	return %dict;
}

function dictionary::index(%this,%value) {
	for(%i=0;%i<%this.getCount();%i++) {
		%o = %this.getObject(%i);
		if(%o.index(0) $= %value) {
				return %o.index(1);
		}
	}
	return false;
}

function try(%expr) {
	%apples = false;
	eval("" @ %expr @ "%apples=true;");
	if(%apples) {
		return true;
	} else return false;
}