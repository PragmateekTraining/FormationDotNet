﻿namespace ProtobufSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            new LogServerSample("localhost", 1234).Run();
        }
    }
}
