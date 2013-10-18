using Microsoft.Win32.SafeHandles;
using SamplesAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SafeHandleSamples
{
    public class HandlesLeakSample : ISample
    {
        const string filePath = "test.txt";

        [Flags]
        enum OpenFileStyle : uint
        {
            OF_CANCEL = 0x00000800,  // Ignored. For a dialog box with a Cancel button, use OF_PROMPT.
            OF_CREATE = 0x00001000,  // Creates a new file. If file exists, it is truncated to zero (0) length.
            OF_DELETE = 0x00000200,  // Deletes a file.
            OF_EXIST = 0x00004000,  // Opens a file and then closes it. Used to test that a file exists
            OF_PARSE = 0x00000100,  // Fills the OFSTRUCT structure, but does not do anything else.
            OF_PROMPT = 0x00002000,  // Displays a dialog box if a requested file does not exist 
            OF_READ = 0x00000000,  // Opens a file for reading only.
            OF_READWRITE = 0x00000002,  // Opens a file with read/write permissions.
            OF_REOPEN = 0x00008000,  // Opens a file by using information in the reopen buffer.

            // For MS-DOS–based file systems, opens a file with compatibility mode, allows any process on a 
            // specified computer to open the file any number of times.
            // Other efforts to open a file with other sharing modes fail. This flag is mapped to the 
            // FILE_SHARE_READ|FILE_SHARE_WRITE flags of the CreateFile function.
            OF_SHARE_COMPAT = 0x00000000,

            // Opens a file without denying read or write access to other processes.
            // On MS-DOS-based file systems, if the file has been opened in compatibility mode
            // by any other process, the function fails.
            // This flag is mapped to the FILE_SHARE_READ|FILE_SHARE_WRITE flags of the CreateFile function.
            OF_SHARE_DENY_NONE = 0x00000040,

            // Opens a file and denies read access to other processes.
            // On MS-DOS-based file systems, if the file has been opened in compatibility mode,
            // or for read access by any other process, the function fails.
            // This flag is mapped to the FILE_SHARE_WRITE flag of the CreateFile function.
            OF_SHARE_DENY_READ = 0x00000030,

            // Opens a file and denies write access to other processes.
            // On MS-DOS-based file systems, if a file has been opened in compatibility mode,
            // or for write access by any other process, the function fails.
            // This flag is mapped to the FILE_SHARE_READ flag of the CreateFile function.
            OF_SHARE_DENY_WRITE = 0x00000020,

            // Opens a file with exclusive mode, and denies both read/write access to other processes.
            // If a file has been opened in any other mode for read/write access, even by the current process,
            // the function fails.
            OF_SHARE_EXCLUSIVE = 0x00000010,

            // Verifies that the date and time of a file are the same as when it was opened previously.
            // This is useful as an extra check for read-only files.
            OF_VERIFY = 0x00000400,

            // Opens a file for write access only.
            OF_WRITE = 0x00000001
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OFSTRUCT
        {
            public byte cBytes;
            public byte fFixedDisc;
            public UInt16 nErrCode;
            public UInt16 Reserved1;
            public UInt16 Reserved2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szPathName;
        }

        /*[DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(int handle);*/

        public class RemoteTester : MarshalByRefObject
        {
            private UnsafeHandleWrapper unsafeHandleWrapper;
            private SaferHandleWrapper saferHandleWrapper;
            private SaferSlowHandleWrapper saferSlowHandleWrapper;
            private BestHandleWrapper bestHandleWrapper;

            public void Unsafe()
            {
                unsafeHandleWrapper = new UnsafeHandleWrapper("unsafe.txt");
            }

            public void Safer()
            {
                saferHandleWrapper = new SaferHandleWrapper("safer.txt");
            }

            public void SaferSlow()
            {
                saferSlowHandleWrapper = new SaferSlowHandleWrapper("safer_slow.txt");
            }

            public void Safe()
            {
                bestHandleWrapper = new BestHandleWrapper("safe.txt");
            }
        }

        class UnsafeHandleWrapper
        {
            [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
            static extern int OpenFile([MarshalAs(UnmanagedType.LPStr)]string lpFileName, out OFSTRUCT lpReOpenBuff, OpenFileStyle uStyle);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool CloseHandle(int handle);

            private readonly int handle;

            public UnsafeHandleWrapper(string path)
            {
                if (!File.Exists(path))
                {
                    using (File.Create(path)) { }
                }

                OFSTRUCT of;

                handle = OpenFile(path, out of, OpenFileStyle.OF_READ | OpenFileStyle.OF_SHARE_EXCLUSIVE);
            }

            public void Release()
            {
                CloseHandle(handle);
            }
        }

        class SaferHandleWrapper : IDisposable
        {
            [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
            static extern int OpenFile([MarshalAs(UnmanagedType.LPStr)]string lpFileName, out OFSTRUCT lpReOpenBuff, OpenFileStyle uStyle);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool CloseHandle(int handle);

            private readonly int handle = -1;

            public SaferHandleWrapper(string path)
            {
                if (!File.Exists(path))
                {
                    using (File.Create(path)) { }
                }

                OFSTRUCT of;

                handle = OpenFile(path, out of, OpenFileStyle.OF_READ | OpenFileStyle.OF_SHARE_EXCLUSIVE);
            }

            public void Release()
            {
                if (handle != -1)
                {
                    CloseHandle(handle);
                }
            }

            public void Dispose()
            {
                Release();
                GC.SuppressFinalize(this);
            }

            ~SaferHandleWrapper()
            {
                Release();
            }
        }

        class SaferSlowHandleWrapper : IDisposable
        {
            [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
            static extern int OpenFile([MarshalAs(UnmanagedType.LPStr)]string lpFileName, out OFSTRUCT lpReOpenBuff, OpenFileStyle uStyle);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool CloseHandle(int handle);

            private int handle = -1;
            private int Handle
            {
                get
                {
                    return handle;
                }
                set
                {
                    Thread.Sleep(2000);
                    handle = value;
                }
            }

            public SaferSlowHandleWrapper(string path)
            {
                if (!File.Exists(path))
                {
                    using (File.Create(path)) { }
                }

                OFSTRUCT of;

                Handle = OpenFile(path, out of, OpenFileStyle.OF_READ | OpenFileStyle.OF_SHARE_EXCLUSIVE);
            }

            public void Release()
            {
                if (handle != -1)
                {
                    CloseHandle(handle);
                }
            }

            public void Dispose()
            {
                Release();
                GC.SuppressFinalize(this);
            }

            ~SaferSlowHandleWrapper()
            {
                Release();
            }
        }

        class BestHandleWrapper : IDisposable
        {
            [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
            static extern SafeFileHandle OpenFile([MarshalAs(UnmanagedType.LPStr)]string lpFileName, out OFSTRUCT lpReOpenBuff, OpenFileStyle uStyle);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool CloseHandle(int handle);

            private SafeFileHandle handle = null;
            private SafeFileHandle Handle
            {
                get
                {
                    return handle;
                }
                set
                {
                    Thread.Sleep(2000);
                    handle = value;
                }
            }

            public BestHandleWrapper(string path)
            {
                if (!File.Exists(path))
                {
                    using (File.Create(path)) { }
                }

                OFSTRUCT of;

                Handle = OpenFile(path, out of, OpenFileStyle.OF_READ | OpenFileStyle.OF_SHARE_EXCLUSIVE);
            }

            public void Release()
            {
                if (handle != null)
                {
                    handle.Dispose();
                }
            }

            public void Dispose()
            {
                Release();
                GC.SuppressFinalize(this);
            }

            ~BestHandleWrapper()
            {
                Release();
            }
        }

        /*private static void Collect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }*/

        /*[DllImport("kernel32.dll", EntryPoint = "OpenFile", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        static extern int UnsafeOpenFile([MarshalAs(UnmanagedType.LPStr)]string lpFileName, out OFSTRUCT lpReOpenBuff, OpenFileStyle uStyle);

        private void Unsafe()
        {
            if (!File.Exists("unsafe.txt"))
            {
                using (File.Create("unsafe.txt")) { }
            }

            OFSTRUCT of;

            int handle = UnsafeOpenFile("unsafe.txt", out of, OpenFileStyle.OF_READ | OpenFileStyle.OF_SHARE_EXCLUSIVE);

            Thread.Sleep(2000);

            CloseHandle(handle);
        }

        private void Safer()
        {
            if (!File.Exists("safer.txt"))
            {
                using (File.Create("safer.txt")) { }
            }

            OFSTRUCT of;

            int handle = -1;

            try
            {
                handle = UnsafeOpenFile("safer.txt", out of, OpenFileStyle.OF_READ | OpenFileStyle.OF_SHARE_EXCLUSIVE);

                Thread.Sleep(2000);
            }
            finally
            {
                CloseHandle(handle);
            }
        }*/

        private void RunInAppDomain(Action<RemoteTester> f)
        {
            AppDomain tmp = AppDomain.CreateDomain("tmp");
            RemoteTester tester = tmp.CreateInstanceAndUnwrap("SafeHandleSamples", "SafeHandleSamples.HandlesLeakSample+RemoteTester") as RemoteTester;
            new Thread(() => f(tester)) { IsBackground = true }.Start();
            Thread.Sleep(100);
            AppDomain.Unload(tmp);
        }

        private void TryOpen(string path)
        {
            try
            {
                File.ReadAllText(path);
                Console.WriteLine("Can open file '{0}'!", path);
            }
            catch
            {
                Console.Error.WriteLine("Cannot open file '{0}'!", path);
            }
        }

        public void Run()
        {
            // string text = File.ReadAllText(filePath);


            // UnsafeHandleWrapper unsafeHandleWrapper = new UnsafeHandleWrapper("test1.txt");
            // unsafeHandleWrapper.Release();

            // unsafeHandleWrapper = null;

            // Collect();

            // BetterHandleWrapper betterHandleWrapper = new BetterHandleWrapper("test2.txt");

            // betterHandleWrapper = null;

            // Collect();

            /*Thread unsafeThread = new Thread(Unsafe) { IsBackground = true };
            Thread saferThread = new Thread(Safer) { IsBackground = true };

            unsafeThread.Start();
            saferThread.Start();

            Thread.Sleep(100);

            unsafeThread.Abort();
            saferThread.Abort();

            try
            {
                File.ReadAllText("unsafe.txt");
                Console.WriteLine("Can read file 'unsafe.txt'");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Cannot open file 'unsafe.txt'!");
            }

            try
            {
                File.ReadAllText("safer.txt");
                Console.WriteLine("Can read file 'safer.txt'");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Cannot open file 'safer.txt'!");
            }*/

            /*AppDomain tmp = AppDomain.CreateDomain("tmp");
            RemoteTester tester = tmp.CreateInstanceAndUnwrap("SafeHandleSamples", "SafeHandleSamples.HandlesLeakSample+RemoteTester") as RemoteTester;
            new Thread(() => tester.Unsafe()) { IsBackground = true }.Start();
            Thread.Sleep(1000);
            AppDomain.Unload(tmp);

            tmp = AppDomain.CreateDomain("tmp");
            tester = tmp.CreateInstanceAndUnwrap("SafeHandleSamples", "SafeHandleSamples.HandlesLeakSample+RemoteTester") as RemoteTester;
            new Thread(() => tester.Safer()) { IsBackground = true }.Start();
            Thread.Sleep(1000);
            AppDomain.Unload(tmp);

            tmp = AppDomain.CreateDomain("tmp");
            tester = tmp.CreateInstanceAndUnwrap("SafeHandleSamples", "SafeHandleSamples.HandlesLeakSample+RemoteTester") as RemoteTester;
            new Thread(() => tester.SaferSlow()) { IsBackground = true }.Start();
            Thread.Sleep(1000);
            AppDomain.Unload(tmp);

            tmp = AppDomain.CreateDomain("tmp");
            tester = tmp.CreateInstanceAndUnwrap("SafeHandleSamples", "SafeHandleSamples.HandlesLeakSample+RemoteTester") as RemoteTester;
            new Thread(() => tester.Safe()) { IsBackground = true }.Start();
            Thread.Sleep(1000);
            AppDomain.Unload(tmp);*/

            RunInAppDomain(tester => tester.Unsafe());
            RunInAppDomain(tester => tester.Safer());
            RunInAppDomain(tester => tester.SaferSlow());
            RunInAppDomain(tester => tester.Safe());

            TryOpen("unsafe.txt");
            TryOpen("safer.txt");
            TryOpen("safer_slow.txt");
            TryOpen("safe.txt");
        }
    }
}
