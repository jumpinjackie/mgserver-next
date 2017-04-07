#include "UnicodeString.h"
#include <codecvt>

void UnicodeString::WideCharToMultiByte(const wchar_t* uniChar, std::string& str)
{
    std::wstring_convert<std::codecvt_utf8<wchar_t>, wchar_t> converter;
    str = converter.to_bytes(uniChar);
}

void UnicodeString::MultiByteToWideChar(const char* uniChar, std::wstring& str)
{
    std::wstring_convert<std::codecvt_utf8<wchar_t>, wchar_t> converter;
    str = converter.from_bytes(uniChar);
}