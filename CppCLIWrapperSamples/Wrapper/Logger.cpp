//#include "NativeLogger.h"
#include <msclr/marshal_cppstd.h>

namespace native
{
	class Logger
	{
		private: std::string path;
		public: void set_path(std::string message);
		public: std::string get_path();

		public: Logger(std::string path);

		public: void log(std::string message);
	};
}

namespace Managed
{
    public ref class Logger
    {
		private: native::Logger* nativeLogger;

		public: Logger(System::String^ path)
		{
			Path = path;
			nativeLogger = new native::Logger(msclr::interop::marshal_as<std::string>(path));
		}

		public: property System::String^ Path;

		public: void Log(System::String^ message)
		{
			nativeLogger->log(msclr::interop::marshal_as<std::string>(message));
		}
    };
}