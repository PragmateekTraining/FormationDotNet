#include <string>

class __declspec(dllexport) NativeLogger
{
	private: std::string path;
	public: void set_path(std::string message);
	public: std::string get_path();

	public: NativeLogger(std::string path);

	public: void log(std::string message);
};