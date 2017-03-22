#ifndef MESSAGES_DLL_EXPORT_H
#define MESSAGES_DLL_EXPORT_H

#ifdef _WIN32
#ifdef PROTO_MESSAGES_EXPORTS
#    define MG_PROTO_MESSAGES_API __declspec(dllexport)
#else
#    define MG_PROTO_MESSAGES_API __declspec(dllimport)
#endif
#else
#define MG_PROTO_MESSAGES_API
#endif

#endif