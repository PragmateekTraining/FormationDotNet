using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttributesSamples
{
    public class CSVSerializerSample : ISample
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        class CSVEntityAttribute : Attribute
        {
            public bool GenerateHeaders { get; set; }
            public bool GenerateId { get; set; }
            public string IdColumnName { get; set; }

            public CSVEntityAttribute(bool generateHeaders = false, bool generateId = false, string idColumnName = null)
            {
                GenerateHeaders = generateHeaders;
                GenerateId = generateId;
                IdColumnName = idColumnName;
            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        class CSVColumnAttribute : Attribute
        {
            public string Name { get; set; }
            public int Index { get; set; }
            public string Format { get; set; }

            public CSVColumnAttribute(string name = null, int index = -1, string format = null)
            {
                Name = name;
                Index = index;
                Format = format;
            }
        }

        [CSVEntity(GenerateHeaders = true, GenerateId = true, IdColumnName = "id")]
        class StockQuote
        {
            [CSVColumn("ticker", Index = 1)]
            public string Ticker { get; set; }
            [CSVColumn("date", Index = 0, Format = "yyyy-MM-dd")]
            public DateTime Date { get; set; }
            [CSVColumn("open")]
            public decimal Open { get; set; }
            [CSVColumn("high")]
            public decimal High { get; set; }
            [CSVColumn("low")]
            public decimal Low { get; set; }
            [CSVColumn("close")]
            public decimal Close { get; set; }
            [CSVColumn("adjusted-close")]
            public decimal AdjustedClose { get; set; }
            [CSVColumn("volume", Index = 2)]
            public decimal Volume { get; set; }

            public StockQuote(DateTime date, string ticker, decimal open, decimal high, decimal low, decimal close, decimal adjustedClose, decimal volume)
            {
                Date = date;
                Ticker = ticker;
                Open = open;
                High = high;
                Low = low;
                Close = close;
                AdjustedClose = adjustedClose;
                Volume = volume;
            }
        }

        class CSVSerializer
        {
            class PropertyAttributePair
            {
                public int Index { get; set; }
                public PropertyInfo Property { get; set; }
                public CSVColumnAttribute Attribute { get; set; }
            }

            public string ColumnSeparator { get; set; }
            public string RowSeparator { get; set; }

            public CSVSerializer(string columnSeparator = "\t", string rowSeparator = "\n")
            {
                ColumnSeparator = columnSeparator;
                RowSeparator = rowSeparator;
            }

            public string Serialize<T>(IEnumerable<T> data)
            {
                StringBuilder builder = new StringBuilder();

                Type type = typeof(T);
                CSVEntityAttribute entityAttribute = type.GetCustomAttribute<CSVEntityAttribute>();
                int index = 1000000;
                IList<PropertyAttributePair> properties = type.GetProperties()
                                                              .Select(pi => new PropertyAttributePair
                                                                        {
                                                                            Index = index++,
                                                                            Property = pi,
                                                                            Attribute = pi.GetCustomAttribute<CSVColumnAttribute>()
                                                                        })
                                                              .OrderBy(pair => pair.Attribute.Index != -1 ? pair.Attribute.Index : pair.Index)
                                                              .ToList();
                if (entityAttribute.GenerateId)
                {
                    properties.Insert(0, null);
                }

                if (entityAttribute.GenerateHeaders)
                {
                    foreach (PropertyAttributePair pair in properties)
                    {
                        if (pair == null)
                        {
                            builder.Append(entityAttribute.IdColumnName + ColumnSeparator);
                            continue;
                        }

                        builder.Append((pair.Attribute.Name ?? pair.Property.Name) + ColumnSeparator);
                    }

                    builder.Remove(builder.Length - ColumnSeparator.Length, ColumnSeparator.Length);
                    builder.Append(RowSeparator);
                }

                int id = 0;
                foreach (T entity in data)
                {
                    foreach (PropertyAttributePair pair in properties)
                    {
                        if (pair == null)
                        {
                            builder.Append(id++ + ColumnSeparator);
                            continue;
                        }

                        object value = pair.Property.GetValue(entity);

                        string str = "";

                        if (value != null)
                        {
                            if (pair.Attribute.Format != null)
                            {
                                MethodInfo toString = pair.Property.PropertyType.GetMethod("ToString", new[] { typeof(string) });

                                if (toString == null)
                                {
                                    throw new Exception(string.Format("No proper 'ToString' method found on type '{0}'!", pair.Property.PropertyType));
                                }

                                str = toString.Invoke(value, new[] { pair.Attribute.Format }) as string;
                            }
                            else
                            {
                                str = value.ToString();
                            }
                        }


                        builder.Append(str + ColumnSeparator);
                    }

                    builder.Remove(builder.Length - ColumnSeparator.Length, ColumnSeparator.Length);
                    builder.Append(RowSeparator);
                }

                builder.Remove(builder.Length - RowSeparator.Length, RowSeparator.Length);

                return builder.ToString();
            }
        }

        public void Run()
        {
            StockQuote[] quotes = {
                                    new StockQuote(new DateTime(2013, 11, 14), "GOOG", 1033.92m, 1039.75m, 1030.35m, 1035.23m, 1035.23m, 1166700m),
                                    new StockQuote(new DateTime(2013, 11, 13), "GOOG", 1006.75m, 1032.85m, 1006.50m, 1032.47m, 1032.47m, 1579400m),
                                    new StockQuote(new DateTime(2013, 11, 12), "GOOG", 1007.70m, 1017.56m, 1005.00m, 1011.78m, 1011.78m, 1218100m),
                                    new StockQuote(new DateTime(2013, 11, 11), "GOOG", 1009.51m, 1015.93m, 1008.00m, 1010.59m, 1010.59m, 1112600m),
                                    new StockQuote(new DateTime(2013, 11, 08), "GOOG", 1008.75m, 1018.50m, 1008.50m, 1016.03m, 1016.03m, 1290800m),
                                    new StockQuote(new DateTime(2013, 11, 07), "GOOG", 1022.61m, 1023.93m, 1007.64m, 1007.95m, 1007.95m, 1679600m)
                                  };

            CSVSerializer serializer = new CSVSerializer();

            string csv = serializer.Serialize(quotes);

            Console.WriteLine(csv);
        }
    }
}
