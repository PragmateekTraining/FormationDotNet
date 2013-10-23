%include "attribute.i"
%include "std_string.i"
%include "windows.i"

%module Native

%{
#include "NativeLogger.h"
%}

%include "NativeLogger.h"

%attribute(NativeLogger, std::string, Path, get_path, set_path);