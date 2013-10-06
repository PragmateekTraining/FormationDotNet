using System.Reflection;
using System.Text.RegularExpressions;

namespace PrecompiledRegexGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            RegexCompilationInfo[] regex = { new RegexCompilationInfo("abc", RegexOptions.None, "ABCPattern", "Tools.PrecompiledRegex", true) };

            AssemblyName assemblyName = new AssemblyName { Name = "Tools.PrecompiledRegex" };

            Regex.CompileToAssembly(regex, assemblyName);  
        }
    }
}
