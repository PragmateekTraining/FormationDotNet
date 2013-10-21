using SamplesAPI;
using System;
using System.Linq;
using System.Reflection;

namespace CERSamples
{
    public class EntryPoint
    {
        public static int Call(string sampleName)
        {
            try
            {
                Type sampleType = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(type => type.Name == sampleName);

                if (sampleType == null)
                {
                    Console.Error.WriteLine("No such sample '{0}'!", sampleName);

                    return 1;
                }

                ConstructorInfo defaultConstructor = sampleType.GetConstructor(Type.EmptyTypes);

                if (defaultConstructor == null)
                {
                    Console.Error.WriteLine("No default constructor in sample '{0}'!", sampleType.FullName);

                    return 1;
                }

                ISample sample = defaultConstructor.Invoke(null) as ISample;

                try
                {
                    sample.Run();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Error while running sample '{0}':\n{1}", sampleName, e);

                    return 1;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unexpected error:\n'{0}'", e);

                return 1;
            }

            return 0;
        }
    }
}
