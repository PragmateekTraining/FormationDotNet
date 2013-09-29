using System;

class ConfigurationReader
{
    static void Main()
    {
        Console.WriteLine("ConstString : " + ConfigurationData.ConstString);
        Console.WriteLine("StaticReadonlyString : " + ConfigurationData.StaticReadonlyString);
    }
}