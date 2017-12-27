//***** class description start *****
//		author: Elm
//
//		description - ExcelReader Class
//		Supports parsing Excel [.csv ONLY!] files
//		for data manipulation/extraction
//
//***** class description end ***** 

//***** ExcelReader class *****

//new scriptgroup
if(!isObject($class::excelReader))
{
	$class::excelReader = new scriptGroup(excelReader);
}

//name: read
//description: reads a .csv file and extracts data
function excelReader::read(%this,%path)
{
	if(!discoverfile(%path))
	{
		error("excel reader invalid path.");
		return false;
	}
	
	%this.lineCount = 0;
	%this.clear();
	
	%ignoreFirst = true;
	
	%temp = fileCopy(%path,strReplace(%path,".csv",".txt"));
	
	%f = new fileObject();
	%f.openForRead(strReplace(%path,".csv",".txt"));
	
	while(!%f.isEOF())
	{
		%line = %f.readLine();		
		
		%excelData = new scriptObject()
		{
			file = %path;
			class = excelData;
			value = %line;
			parent = %this;
		};
		
		%excelData.splitData();
		
		if(%ignoreFirst)
		{
			if(isObject(%this.header))
				%this.header.delete();
			%this.header = %excelData;
			%ignoreFirst = false;
			continue;
		}
		
		%this.add(%excelData);
	}
	
	%f.close();
	%f.delete();
	fileDelete(strReplace(%path,".csv",".txt"));
}

//name: getValueByValue
//description: used to get a value by another value.
//example: your .csv file looks like:
//firstname,lastname,weight
//Jhon,doe,150
//Abby,jhonson,110
//-
//excelReader.getValueByValue["weight","jhon"] <-- use parenthesis
//would return 150
//excelReader.getValueByValue["lastname","abby"] <-- use parenthesis
//would return jhonson
function excelReader::getValueByValue(%this,%value,%byValue)
{
	%finalValue = false;
	%index = false;
	
	%headString = strReplace(%this.header.value,",","	");
	%wordCount = getWordCount(%headString);
	for(%j=0;%j<%wordCount;%j++)
	{
		
		%word = getField(%headString,%j);
		if(%word $= %value)
		{
			%index = %j;
			break;
		}
	}
	
	if(%index == false)
	{
		error("excel reader no index.");
		return false;
	}
	
	%c = %this.getCount();
	
	for(%i=0;%i<%c;%i++)
	{		
		%excelData = %this.getObject(%i);
		%count = %excelData.splitCount;
		for(%j=0;%j<%count+1;%j++)
		{
			%d = %excelData.data[%j];
			if(%d $= %byValue)
			{
				%finalValue = %excelData.data[%index];
				return %finalValue;
			}
		}
	}
	
	if(%finalValue $= "0")
	{
		error("excel reader no final Value.");
		return false;
	}
}

//******** ExcelData child class **********

//name: splitData
//description: private function, splits data row.

function excelData::splitData(%this)
{
	%data = %this.value;
	%len = strLen(%data);
	
	%this.splitCount = -1;
	
	for(%i=0;%i<%len;%i++)
	{
		%letter = getSubStr(%data,%i,1);
		
		if(%letter $= ",")
		{
			%this.data[%this.splitCount++] = %build;
			%build = "";
		}
		else
		{
			%build = %build @ %letter;
		}
	}
	
	if(%build !$= "")
		%this.data[%this.splitCount++] = %build;
	
	return true;
}

