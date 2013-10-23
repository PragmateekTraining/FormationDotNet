#include <string>
#include <fstream>
#include "NativeLogger.h"

NativeLogger::NativeLogger(std::string path)
{
	set_path(path);
}

void NativeLogger::set_path(std::string path)
{
	this->path = path;
}

std::string NativeLogger::get_path()
{
	return path;
}

void NativeLogger::log(std::string message)
{
	std::ofstream log_file(path, std::ios_base::app | std::ios_base::out);

	log_file << message << std::endl;
}