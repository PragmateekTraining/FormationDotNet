using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exceptions
{
    class RethrowSample
    {
        bool rethrow = false;

        public RethrowSample(bool rethrow)
        {
            this.rethrow = rethrow;
        }

        public void F()
        {
            try
            {
                G();
            }
            catch (Exception e)
            {
                if (rethrow) throw;

                throw e;
            }
        }

        public void G()
        {
            H();
        }

        public void H()
        {
            throw new Exception("Boom!");
        }

        public void Run()
        {
            try
            {
                F();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
