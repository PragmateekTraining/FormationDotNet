using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using SamplesAPI;
using System.Globalization;
using System.ServiceModel.Web;
using System.ServiceModel.Description;

namespace WCFSamples
{
    class FullWebServiceSample
    {
        enum Level
        {
            _ = 0,
            INFO = 1,
            WARNING = 2,
            ERROR = 3
        }

        [DataContract]
        struct Log
        {
            [DataMember]
            public long ID { get; set; }
            [DataMember]
            public string Timestamp { get; set; }
            [DataMember]
            public string Level { get; set; }
            [DataMember]
            public string Message { get; set; }
        }

        [ServiceContract(Namespace = "http://pragmateek.com/", Name = "LogSink")]
        interface ILogSink
        {
            [OperationContract]
            void Log(Level level, string message);
        }

        [ServiceContract(Namespace = "http://pragmateek.com/", Name = "LogSource")]
        interface ILogSource
        {
            [OperationContract]
            [WebGet(UriTemplate = "", ResponseFormat = WebMessageFormat.Json)]
            Log[] GetAllLogs();

            [OperationContract]
            [WebGet(UriTemplate = "from/{firstLogId}", ResponseFormat = WebMessageFormat.Json)]
            Log[] GetLogsFrom(string firstLogId);
        }

        class LogService : ILogSink, ILogSource, IDisposable
        {
            IDbConnection connection;

            public LogService()
            {
                connection = new SQLiteConnection("Data Source=logs.db;Version=3;");
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Logs(id INTEGER PRIMARY KEY ASC AUTOINCREMENT, timestamp TEXT, level INTEGER, message TEXT)";
                    command.ExecuteNonQuery();
                }
            }

            public void Log(Level level, string message)
            {
                DateTime timestamp = DateTime.Now;

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Logs (timestamp, level, message) VALUES (@timestamp, @level, @message)";
                    command.AddParameter("@timestamp", timestamp.ToString("o"));
                    command.AddParameter("@level", level);
                    command.AddParameter("@message", message);

                    command.ExecuteNonQuery();
                }
            }

            IList<Log> ReadLogs(IDataReader reader)
            {
                IList<Log> logs = new List<Log>();

                while (reader.Read())
                {
                    Log log = new Log
                    {
                        ID = Convert.ToInt64(reader["id"]),
                        Timestamp = reader["timestamp"] as string,
                        Level = ((Level)(long)reader["level"]).ToString(),
                        Message = reader["message"] as string
                    };

                    logs.Add(log);
                }

                return logs;
            }

            public Log[] GetAllLogs()
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Logs";

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        return ReadLogs(reader).ToArray();
                    }
                }
            }

            public Log[] GetLogsFrom(string firstLogId)
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Logs WHERE id >= @firstLogId";
                    command.AddParameter("@firstLogId", firstLogId);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        return ReadLogs(reader).ToArray();
                    }
                }
            }

            public void Dispose()
            {
                connection.Dispose();
                GC.SuppressFinalize(this);
            }

            ~LogService()
            {
                Dispose();
            }
        }

        public void Run()
        {
            ServiceHost host = new ServiceHost(typeof(LogService), new Uri("http://localhost:8000/"));
            host.AddServiceEndpoint(typeof(ILogSink), new WSHttpBinding(), "LogSink");
            host.AddServiceEndpoint(typeof(ILogSource), new WebHttpBinding(WebHttpSecurityMode.None), "logs")
                .EndpointBehaviors
                .Add(new WebHttpBehavior());

            host.Open();

            Console.WriteLine("Press enter to close...");

            ILogSink proxy = ChannelFactory<ILogSink>.CreateChannel(new WSHttpBinding(), new EndpointAddress("http://localhost:8000/LogSink"));

            IDictionary<char, Level> mapping = new Dictionary<char, Level> { { '.', Level.INFO }, { '?', Level.WARNING }, { '!', Level.ERROR } };

            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                char level = input[0];
                string message = input.Substring(1);

                proxy.Log(mapping[level], message);
            }

            host.Close();
        }
    }
}
