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
        // Create
        [OperationContract]
        int CreateBinaryResource();

        [OperationContract]
        int CreateTextResource();

        // Read
        [OperationContract]
        byte[] ReadBinaryResource(int id);

        [OperationContract]
        string ReadTextResource(int id);

        // Write
        [OperationContract]
        void WriteBinaryResource(int id, byte[] toWrite);

        [OperationContract]
        void WriteTextResource(int id, string toWrite);

        [OperationContract]
        void AppendTextResource(int id, string toWrite);
                
        // Clear
        [OperationContract]
        void ClearTextResource(int id);

        // Delete
        [OperationContract]
        void DeleteBinaryResource(int id);

        [OperationContract]
        void DeleteTextResource(int id);
    }   
}
