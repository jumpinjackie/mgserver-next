#ifndef MDF_MODEL_H
#define MDF_MODEL_H

namespace MdfModel
{
    // the MdfString is a std::wstring.
    // It might be swapped out  at a later date for a const wchar_t* .
    typedef std::wstring MdfString; // wide string for unicode support
}

#endif