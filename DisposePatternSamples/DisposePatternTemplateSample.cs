using SamplesAPI;
using System;

namespace DisposePatternSamples
{
    public class DisposePatternTemplateSample : ISample
    {
        class DisposableAndFinalizable : IDisposable
        {
            IDisposable disposableRessource;
            IntPtr nativeRessource;
            bool disposed = false;

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            
            ~DisposableAndFinalizable()
            {
                Dispose(false);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        if (disposableRessource != null) disposableRessource.Dispose();
                    }

                    // Do something with nativeRessource

                    disposed = true;
                }
            }
        }

        class DisposableAndFinalizableSub : DisposableAndFinalizable
        {
            IDisposable disposableRessource;
            IntPtr nativeRessource;
            bool disposed = false;

            protected override void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    try
                    {
                        if (disposing)
                        {
                            if (disposableRessource != null) disposableRessource.Dispose();
                        }

                        // Do something with nativeRessource

                        disposed = true;
                    }
                    finally
                    {
                        base.Dispose(disposing);
                    }
                }
            }
        }

        public void Run()
        {
        }
    }
}
