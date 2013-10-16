using SamplesAPI;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace COMInteropSamples
{
    public class NativeCOMSample : ISample
    {
        [ComImport]
        [Guid("F0CE8DD2-AE05-435B-AEB5-038EA4A1191B")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        interface IComLogger
        {
            void Log(string message);
        }

        [ComImport]
        [Guid("8A49257F-D5E9-47DB-B87D-E09EBAF199AE")]
        class ComLogger
        {
        }

        private string message = null;

        public NativeCOMSample(string message)
        {
            this.message = message;
        }

        private void Build()
        {
            Tools.RunAndWait("build.bat");
        }

        private void Register()
        {
            string regFileTemplate = File.ReadAllText("NativeComponent/ComLogger.reg.template");
            string COMDLLPath = Path.Combine(Directory.GetCurrentDirectory(), @"NativeComponent\ComLogger.dll").Replace(@"\", @"\\");
            string TLBPath = Path.Combine(Directory.GetCurrentDirectory(), @"NativeComponent\ComLogger.tlb").Replace(@"\", @"\\");
            string regFile = regFileTemplate.Replace("{{dll_path}}", COMDLLPath)
                                            .Replace("{{tlb_path}}", TLBPath);

            File.WriteAllText(@"NativeComponent\ComLogger.reg", regFile);

            Tools.RunAndWait("reg", @"import NativeComponent\ComLogger.reg");
        }

        private void Invoke()
        {
            ComLogger comLogger = new ComLogger();
            IComLogger logger = comLogger as IComLogger;
            logger.Log(message);
        }

        private void RunExe()
        {
            Tools.RunAndWait("Test.exe", '"' + message + '"');
        }

        public void Run()
        {
            // Type type = Type.GetTypeFromProgID("Pragmateek.ComLogger");
            // Activator.CreateInstance(type);
            // dynamic logger = Activator.CreateInstance(type);
            //Marshal.
            // dynamic logger = new Co mLogger();
            Build();
            Register();
            Invoke();
            RunExe();
            // logger.Test("123333333333333333333", 123, 456.789);
        }
    }
}
