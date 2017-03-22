#ifndef SERVICES_DLL_EXPORT_H
#define SERVICES_DLL_EXPORT_H

#ifdef _WIN32
#ifdef SERVICES_EXPORTS
#    define MG_SERVICES_API __declspec(dllexport)
#else
#    define MG_SERVICES_API __declspec(dllimport)
#endif
#else
#define MG_SERVICES_API
#endif

#endif