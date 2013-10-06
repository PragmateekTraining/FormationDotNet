using System;
using System.Globalization;
using System.Threading;

namespace SamplesAPI
{
    public class Culture : IDisposable
    {
        private readonly CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

        public Culture(string cultureName)
            : this(CultureInfo.GetCultureInfo(cultureName))
        {
        }

        public Culture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = this.originalCulture;
        }
    }
}
