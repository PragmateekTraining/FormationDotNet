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
        const int n = 10;
        const int nWorkers = 8;

        class Data
        {
            public long ID { get; set; }
            public long Version { get; set; }
            public long Value { get; set; }
        }

        void Setup()
        {
            using (IDbConnection connection = NewConnection())
            {
                using (IDbCommand setup = connection.CreateCommand())
                {
                    setup.CommandText = "DROP TABLE IF EXISTS Data;" +
                                        "CREATE TABLE Data(id INTEGER PRIMARY KEY, version INTEGER, value INTEGER);" +
                                        "INSERT INTO Data VALUES(0, 0, 0);";
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
                        Version = (long)reader["version"],
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

        bool Write(IDbConnection connection, Data data, long lastKnownVersion = 0)
        {
            bool hasBeenUpdated;

            using (IDbTransaction transaction = connection.BeginTransaction())
            {
                using (IDbCommand write = connection.CreateCommand())
                {
                    write.CommandText = "UPDATE Data SET version=@version,value=@value WHERE id=0 AND version=@lastKnownVersion";
                    write.AddParameter("@version", data.Version);
                    write.AddParameter("@value", data.Value);
                    write.AddParameter("@lastKnownVersion", lastKnownVersion);
                    hasBeenUpdated = write.ExecuteNonQuery() == 1;
                }

                transaction.Commit();
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
                            using (IDbTransaction transaction = connection.BeginTransaction())
                            {
                                using (IDbCommand @lock = connection.CreateCommand())    
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
            bool hasBeenUpdated = false;

            Data data;
            using (IDbConnection connection = NewConnection())
            {
                for (int i = 0; i < n; ++i)
                {
                    Console.WriteLine(i);

                    long currentVersion;
                    long currentValue;
                    do
                    {
                        data = Read(connection);

                        currentVersion = data.Version;
                        currentValue = data.Value;

                        ++data.Value;
                        ++data.Version;

                        hasBeenUpdated = Write(connection, data, currentVersion);
                    }
                    while (!hasBeenUpdated);

                    // Console.WriteLine("[{0}] '{1}' -> '{2}' / '{3}' -> '{4}'", Thread.CurrentThread.ManagedThreadId, initialFlag, me, initialValue, data.Value);
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
