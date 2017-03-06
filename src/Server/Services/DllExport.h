#ifdef _WIN32
#ifdef SERVICES_EXPORTS
#    define MG_SERVICES_API __declspec(dllexport)
#else
#    define MG_SERVICES_API __declspec(dllimport)
#endif
#else
#define MG_SERVICES_API
#endif