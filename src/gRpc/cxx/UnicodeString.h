#ifndef UNICODE_STRING_H
#define UNICODE_STRING_H

#include <string>
#include "MessagesDllExport.h"
class MG_PROTO_MESSAGES_API UnicodeString
{
public:
    static void WideCharToMultiByte(const wchar_t* uniChar, std::string& str);
    static void MultiByteToWideChar(const char* uniChar, std::wstring& str);
};

#endif