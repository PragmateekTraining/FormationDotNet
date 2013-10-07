#include <string>
#include <fstream>
#include "NativeLogger.h"

namespace native
{
	Logger::Logger(std::string path)
	{
		set_path(path);
	}

	void Logger::set_path(std::string path)
	{
		this->path = path;
	}

	std::string Logger::get_path()
	{
		return path;
	}

	void Logger::log(std::string message)
	{
		std::ofstream log_file(path, std::ios_base::app | std::ios_base::out);

		log_file << message << std::endl;
	}
}