while(isobject($class::string))
	$class::string.delete();

$class::string = new scriptGroup()
{
	class = string;
};

function string::set(%this,%str)
{
	%len = strLen(%str);

	if(%len == 0)return false;
	
	%this.clear();
	%this.string = %str;
	
	for(%i=0;%i<%len;%i++)
	{
		%letter = getSubStr(%str,%i,1);
		%letterO = new scriptObject()
		{
			value = %letter;
		};
		%this.add(%letterO);
	}
    return %this;
}

function string::cap_first(%this)
{
	%this.getObject(0).value = strUpr(%this.getObject(0).value);
	%this.update();
}

function string::cap_every_word(%this)
{
	%c = %this.getcount();
	%build = "";
	%break = false;
	
	for(%i=0;%i<%c;%i++)
	{
		%letter = %this.getObject(%i);
		if(%build $= "")
		{
			%this.getObject(%i).value = strUpr(%this.getObject(%i).value);
			%build = 1;
			continue;
		}
		
		if(%break)
		{
			%this.getObject(%i).value = strUpr(%this.getObject(%i).value);
			%break = false;
		}
		
		if(%letter.value $= " ")
		{
			%break = 1;
		}
	}
	
	%this.update();
}

function string::contains(%this,%cont)
{
	if(stripos(%this.string,%cont) >= 0) return true;
	
	return false;
}

function string::startsWith(%this,%phrase,%trim) {
    if(%trim $= "" || %trim == 1)
        %phrase = trim(%phrase);
    if(strLwr(getSubStr(%this.string,0,strLen(%phrase))) $= strLwr(%phrase)) {
        return true;
    }
    return false;
}

function string::splice(%this,%indexa,%indexb)
{
	%len = strLen(%this.string);
	
	if(%indexb $= "")%indexb = %len;
	
	if(%indexb > %this.getCount())%indexb = %this.getcount();
	
	%c = %indexb;
	%build = "";
	
	for(%i=0;%i<%indexa;%i++)
	{
		%obj = %this.getObject(%i);
		%obj.remove = true;
	}	
	
	for(%i=%len-1;%i>=%len-%indexb;%i--)
	{
		%obj = %this.getObject(%i);
		%obj.remove = true;
	}
	
	%total = -1;
	for(%i=0;%i<%this.getcount();%i++)
	{
		%o = %this.getObject(%i);
		if(%o.remove){
			%o.remove = "";
			continue;
		}

		%build = %build @ %o.value;
	}

	return %build;
}

function string::split(%this,%del) {
    %arr = $class::arrays.create("");
    %bb = "";
    %awaiting = false;
    for(%i=0;%i<%this.getcount();%i++)
	{
        %v = %this.getObject(%i).value;
        if(%v $= %del) {
            %arr.push(%bb);
            %bb = "";
            %awaiting = false;
            continue;
        }
        %awaiting = true;
        %bb = %bb @ %v;
    }
    if(%awaiting) {
        %arr.push(%bb);
    }
    return %arr;
}

function string::update(%this)
{
	%c = %this.getcount();
	%build = "";
	
	for(%i=0;%i<%c;%i++)
	{
		%build = %build @ %this.getObject(%i).value;
	}
	
	%this.string = %build;
}
