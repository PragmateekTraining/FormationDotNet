using SamplesAPI;
using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading;

namespace ThreadingSamples
{
    public class OptimisticConcurrencySample : ISample
    {
        const string connectionString = "Data Source=data.db;Version=3;";
        const int n = 20;
        const int nWorkers = 8;

        class Data
        {
            public long ID { get; set; }
            public string Flag { get; set; }
            public long Value { get; set; }
        }

        void Setup()
        {
            using (IDbConnection connection = NewConnection())
            {
                using (IDbCommand setup = connection.CreateCommand())
                {
                    setup.CommandText = "DROP TABLE IF EXISTS Data;" +
                                        "CREATE TABLE Data(id INTEGER PRIMARY KEY, flag TEXT, value INTEGER);" +
                                        "INSERT INTO Data VALUES(0, NULL, 0);";
                    setup.ExecuteNonQuery();
                }
            }
        }

        IDbConnection NewConnection()
        {
            IDbConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }

        Data Read(IDbConnection connection)
        {
            Data data = null;

            using (IDbCommand read = connection.CreateCommand())
            {
                read.CommandText = "SELECT * FROM Data WHERE id=0";

                using (IDataReader reader = read.ExecuteReader())
                {
                    reader.Read();
                    data = new Data
                    {
                        ID = (long)reader["id"],
                        Flag = reader["flag"] as string,
                        Value = (long)reader["value"]
                    };
                }
            }

            return data;
        }

        Data Read()
        {
            return Read(NewConnection());
        }

        bool Write(IDbConnection connection, Data data, string whereFlag = null)
        {
            bool hasBeenUpdated;

            using (IDbCommand write = connection.CreateCommand())
            {
                write.CommandText = "UPDATE Data SET flag=@flag,value=@value WHERE id=0 AND (flag=@whereFlag OR flag is NULL)";
                write.AddParameter("@flag", data.Flag);
                write.AddParameter("@value", data.Value);
                write.AddParameter("@whereFlag", whereFlag);
                hasBeenUpdated = write.ExecuteNonQuery() == 1;
            }

            return hasBeenUpdated;
        }

        void NaiveCount()
        {
            Data data;
            using (IDbConnection connection = NewConnection())
            {
                for (int i = 0; i < n; ++i)
                {
                    Console.WriteLine(i);

                    data = Read(connection);
                    ++data.Value;

                    if (!Write(connection, data))
                    {
                        throw new Exception("Naive implementation shoud have written something!");
                    }
                }
            }
        }

        void PessimisticCount()
        {
            Data data;
            for (int i = 0; i < n; ++i)
            {
                Console.WriteLine(i);

                using (IDbConnection connection = NewConnection())
                {
                    while (true)
                    {
                        try
                        {
                            using (IDbCommand @lock = connection.CreateCommand())
                            {
                                using (IDbTransaction transaction = connection.BeginTransaction())
                                {
                                    @lock.CommandText = "PRAGMA locking_mode=EXCLUSIVE;";
                                    @lock.ExecuteNonQuery();

                                    transaction.Commit();
                                }
                            }
                        }
                        catch (SQLiteException)
                        {
                            continue;
                        }
                        break;
                    }

                    Console.WriteLine("Thread {0} has exclusive access.", Thread.CurrentThread.ManagedThreadId);

                    data = Read(connection);
                    ++data.Value;

                    Write(connection, data);
                }
            }
        }

        void OptimisticCount()
        {
            string me = Guid.NewGuid().ToString();

            bool hasBeenUpdated = false;

            Data data;
            using (IDbConnection connection = NewConnection())
            {
                for (int i = 0; i < n; ++i)
                {
                    Console.WriteLine(i);

                    do
                    {
                        data = Read(connection);

                        string initialFlag = data.Flag;

                        ++data.Value;
                        data.Flag = me;

                        hasBeenUpdated = Write(connection, data, initialFlag);
                    }
                    while (!hasBeenUpdated);
                }
            }
        }

        void LaunchAndWait(Action action)
        {
            Thread[] threads = new Thread[nWorkers];

            for (int i = 0; i < nWorkers; ++i)
            {
                threads[i] = new Thread(action.Invoke) { IsBackground = true };
            }

            foreach (Thread thread in threads) thread.Start();
            foreach (Thread thread in threads) thread.Join();
        }

        public void Run()
        {
            Setup();

            Stopwatch stopwatch = Stopwatch.StartNew();
            LaunchAndWait(NaiveCount);
            stopwatch.Stop();
            TimeSpan naiveTime = stopwatch.Elapsed;
            Data naiveData = Read();

            Setup();

            stopwatch.Restart();
            LaunchAndWait(PessimisticCount);
            stopwatch.Stop();
            TimeSpan pessimisticTime = stopwatch.Elapsed;
            Data pessimisticData = Read();

            Setup();

            stopwatch.Restart();
            LaunchAndWait(OptimisticCount);
            stopwatch.Stop();
            TimeSpan optimisticTime = stopwatch.Elapsed;
            Data optimisticData = Read();

            Console.WriteLine("Naive: {0} in {1}.", naiveData.Value, naiveTime);
            Console.WriteLine("Pessimistic: {0} in {1}.", pessimisticData.Value, pessimisticTime);
            Console.WriteLine("Optimistic: {0} in {1}.", optimisticData.Value, optimisticTime);
        }
    }
}
