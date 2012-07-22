using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LogBookService
{
    [ServiceContract]
    public interface ILogBookService
    {
        // Create
        [OperationContract]
        void CreateLog(string id);

        // Write
        [OperationContract]
        void WriteLogLine(string id, string line);

        // Clear
        [OperationContract]
        void ClearLog(string id);

        // Delete
        [OperationContract]
        void DeleteLog(string id);
    }   
}
