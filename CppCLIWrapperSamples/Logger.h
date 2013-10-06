#include <string>

__declspec(dllexport) class Logger
{
	private: std::string path;
	public: void set_path(std::string message);
	public: std::string get_path();

	public: void log(std::string message);
};
