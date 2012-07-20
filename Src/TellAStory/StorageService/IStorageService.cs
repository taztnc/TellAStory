using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Collections;
using System.IO;

namespace TellAStoryServiceLib
{
    [ServiceContract]
    public interface IStorageService
    {
        // Read
        [OperationContract]
        byte[] ReadBinaryResource(string id);

        [OperationContract]
        string ReadTextResource(string id);

        // Write
        [OperationContract]
        void WriteBinaryResource(string id, byte[] toWrite);

        [OperationContract]
        void WriteTextResource(string id, string toWrite);

        [OperationContract]
        void AppendTextResource(string id, string toWrite);
                
        // Clear
        [OperationContract]
        void ClearTextResource(string id);

        // Delete
        [OperationContract]
        void DeleteResource(string id);
    }   
}
