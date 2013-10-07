#include <string>

#ifdef BUILDING_NATIVE_DLL
#define DLLAPI __declspec(dllexport)
#else
#define DLLAPI __declspec(dllimport)
#endif

namespace native
{
	class DLLAPI Logger
	{
		private: std::string path;
		public: void set_path(std::string message);
		public: std::string get_path();

		public: Logger(std::string path);

		public: void log(std::string message);
	};
}