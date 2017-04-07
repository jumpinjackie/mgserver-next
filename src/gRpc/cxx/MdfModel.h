#ifndef MDF_MODEL_H
#define MDF_MODEL_H

#include <string>

#define MDFMODEL_NAMESPACE          MdfModel
#define BEGIN_NAMESPACE_MDFMODEL    namespace MdfModel {
#define END_NAMESPACE_MDFMODEL      }

BEGIN_NAMESPACE_MDFMODEL

    // the MdfString is a std::wstring.
    // It might be swapped out  at a later date for a const wchar_t* .
    typedef std::wstring MdfString; // wide string for unicode support

END_NAMESPACE_MDFMODEL

#endif