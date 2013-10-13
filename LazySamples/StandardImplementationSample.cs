using System;

namespace LazySamples
{
    class StandardImplementationSample
    {
        private Lazy<object> lazyObject = new Lazy<object>(() => new object());

        public object Object
        {
            get
            {
                return lazyObject.Value;
            }
        }
    }
}
