using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace ReflectionSamples
{
    public class EmitSample : ISample
    {
        object F(object o)
        {
            A a = o as A;
            return a.N;
        }

        public class A
        {
            public int N { get; set; }
            public double D { get; set; }
            public string S { get; set; }
        }

        static class PropertyInfoBasedPropertiesDumper
        {
            private static IDictionary<Type, IList<PropertyInfo>> cache = new Dictionary<Type, IList<PropertyInfo>>();

            public static string Dump(object o)
            {
                if (o == null)
                {
                    return "null";
                }

                StringBuilder dump = new StringBuilder();

                Type type = o.GetType();

                if (!cache.ContainsKey(type))
                {
                    cache.Add(type, type.GetProperties());
                }

                dump.Append("{\n");

                foreach (PropertyInfo pi in cache[type])
                {
                    dump.AppendFormat("\t{0}:{1}\n", pi.Name, pi.GetValue(o, null));
                }

                dump.Append("}");

                return dump.ToString();
            }
        }

        static class MethodInfoBasedPropertiesDumper
        {
            private static IDictionary<Type, IDictionary<string, MethodInfo>> cache = new Dictionary<Type, IDictionary<string, MethodInfo>>();

            public static string Dump(object o)
            {
                if (o == null)
                {
                    return "null";
                }

                StringBuilder dump = new StringBuilder();

                Type type = o.GetType();

                if (!cache.ContainsKey(type))
                {
                    IDictionary<string, MethodInfo> getters = type.GetProperties().Select(pi => new KeyValuePair<string, MethodInfo>(pi.Name, pi.GetGetMethod()))
                                                                                  .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    cache.Add(type, getters);
                }

                dump.Append("{\n");

                foreach (KeyValuePair<string, MethodInfo> pair in cache[type])
                {
                    dump.AppendFormat("\t{0}:{1}\n", pair.Key, pair.Value.Invoke(o, null));
                }

                dump.Append("}");

                return dump.ToString();
            }
        }

        static class EmitBasedPropertiesDumper
        {
            private delegate object MyDelegate(object o);

            private static IDictionary<Type, IDictionary<string, MyDelegate>> cache = new Dictionary<Type, IDictionary<string, MyDelegate>>();

            public static string Dump(object o)
            {
                if (o == null)
                {
                    return "null";
                }

                StringBuilder dump = new StringBuilder();

                Type type = o.GetType();

                if (!cache.ContainsKey(type))
                {
                    IDictionary<string, MyDelegate> getters = new Dictionary<string, MyDelegate>();

                    foreach (PropertyInfo property in type.GetProperties())
                    {
                        DynamicMethod getter = new DynamicMethod("get_" + property.Name, typeof(object), new[] { typeof(object) });

                        ILGenerator il = getter.GetILGenerator();
                        //il.DeclareLocal(typeof(object));
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Isinst, type);
                        il.Emit(OpCodes.Callvirt, type.GetMethod("get_" + property.Name));
                        if (property.PropertyType.IsValueType)
                        {
                            il.Emit(OpCodes.Box, property.PropertyType);
                        }
                        il.Emit(OpCodes.Ret);

                        getters[property.Name] = (MyDelegate)getter.CreateDelegate(typeof(MyDelegate));
                    }

                    cache.Add(type, getters);
                }

                dump.Append("{\n");

                foreach (KeyValuePair<string, MyDelegate> namedGetter in cache[type])
                {
                    dump.AppendFormat("\t{0}:{1}\n", namedGetter.Key, namedGetter.Value(o));
                }

                dump.Append("}");

                return dump.ToString();
            }
        }

        public void Run()
        {
            A a = new A { N = 123, D = 456.789, S = "123" };

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            const int N = 1000000;

            for (int i = 0; i < N; ++i)
            {
                PropertyInfoBasedPropertiesDumper.Dump(a);
                // MethodInfoBasedPropertiesDumper.Dump(a);
                // EmitBasedPropertiesDumper.Dump(a);
            }

            stopwatch.Stop();

            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();

            for (int i = 0; i < N; ++i)
            {
                // PropertyInfoBasedPropertiesDumper.Dump(a);
                MethodInfoBasedPropertiesDumper.Dump(a);
                // EmitBasedPropertiesDumper.Dump(a);
            }

            stopwatch.Stop();

            TimeSpan t2 = stopwatch.Elapsed;

            stopwatch.Restart();

            for (int i = 0; i < N; ++i)
            {
                // ReflectionBasedPropertiesDumper.Dump(a);
                EmitBasedPropertiesDumper.Dump(a);
            }

            stopwatch.Stop();

            TimeSpan t3 = stopwatch.Elapsed;

            double ratioT1T3 = 1.0 * t1.Ticks / t3.Ticks;
            double ratioT2T3 = 1.0 * t2.Ticks / t3.Ticks;

            Console.WriteLine("With Emit: {0}", t3);
            Console.WriteLine("With MethodInfo: {0} (x{1:N2})", t2, ratioT2T3);
            Console.WriteLine("With PropertyInfo: {0} (x{1:N2})", t1, ratioT1T3);
        }
    }
}
