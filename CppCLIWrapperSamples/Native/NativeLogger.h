#include <string>

namespace native
{
	class
#ifndef __cplusplus_cli
	__declspec(dllexport)
#endif
	Logger
	{
		private: std::string path;
		public: void set_path(std::string message);
		public: std::string get_path();

		public: Logger(std::string path);

		public: void log(std::string message);
	};
}