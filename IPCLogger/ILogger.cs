using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace IPCLogger
{
    [ServiceContract]
    public interface ILogger
    {
        [OperationContract]
        void Log(string message);
    }
}
