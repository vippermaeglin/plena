#include <windows.h>
#include <stdio.h>
#include <fcntl.h>
#include <io.h>
#include <iostream>
#include <fstream>

#if !defined(DEBUG_CONSOLE)
#define DEBUG_CONSOLE


#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

using namespace std;
void InitializeDebugConsole(){	
    //Create a console for this application    
	AllocConsole();    
	//Redirect unbuffered STDOUT to the console	
	HANDLE ConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    
	int SystemOutput = _open_osfhandle(intptr_t(ConsoleOutput), _O_TEXT);    
	FILE *COutputHandle = _fdopen(SystemOutput, "w" );    
	*stdout = *COutputHandle;    
	setvbuf(stdout, NULL, _IONBF, 0);    
	
	//Redirect unbuffered STDERR to the console	
	HANDLE ConsoleError = GetStdHandle(STD_ERROR_HANDLE);    
	int SystemError = _open_osfhandle(intptr_t(ConsoleError), _O_TEXT);    
	FILE *CErrorHandle = _fdopen(SystemError, "w" );    
	*stderr = *CErrorHandle;    
	setvbuf(stderr, NULL, _IONBF, 0);    
	
	//Redirect unbuffered STDIN to the console	
	HANDLE ConsoleInput = GetStdHandle(STD_INPUT_HANDLE);    
	int SystemInput = _open_osfhandle(intptr_t(ConsoleInput), _O_TEXT);    
	FILE *CInputHandle = _fdopen(SystemInput, "r" );    
	*stdin = *CInputHandle;    
	setvbuf(stdin, NULL, _IONBF, 0);        
	
	//make cout, wcout, cin, wcin, wcerr, cerr, wclog and clog point to console as well    
	ios::sync_with_stdio(true);
}
	
void ShutdownDebugConsole(void){	
	//Write "Press any key to exit"	
	HANDLE ConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);	
	DWORD CharsWritten;	
	WriteConsole(ConsoleOutput, "\nPress any key to exit", 22, &CharsWritten, 0);	
	//Disable line-based input mode so we can get a single character	
	HANDLE ConsoleInput = GetStdHandle(STD_INPUT_HANDLE);	
	SetConsoleMode(ConsoleInput, 0);	
	//Read a single character	
	TCHAR InputBuffer;	
	DWORD CharsRead;	
	ReadConsole(ConsoleInput, &InputBuffer, 1, &CharsRead, 0);	
}

#endif