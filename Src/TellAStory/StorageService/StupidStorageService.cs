using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Threading;

namespace StorageService
{
    // kobig - should catch exceptions
    // kobig - should throw proper exceptions

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class StupidStorageService : IStorageService
    {
        private static string mBasePath = @"C:\temp\TellAStoryStorage";

        #region synchronization objects

        // Create/Delete sources locks
        private System.Object lockBinary = new System.Object();
        private System.Object lockText = new System.Object();

        // Read/Write (single) source locks
        private class ReadWriteSyncObjectsStruct
        {
            public int readersCounter = 0; // Counts the number of active readers
            public int writersCounter = 0; // Counts the number of active writers

            public Object lockReadersCounter = new Object(); // Used for locking the readers counter
            public Object lockWritersCounter = new Object(); // Used for locking the writers counter

            public Semaphore semaphoreReadersAccess = new Semaphore(1, 1); // Used for synching the readers access to the resource
            public Semaphore semaphoreWritersAccess = new Semaphore(1, 1); // Used for synching the writers access to the resource

            public Object lockWrapReaders = new Object(); // Used for locking part of the readers actions (for priority requirements)
        }

        // Holds all locking objects for all resources
        Dictionary<string, ReadWriteSyncObjectsStruct> ReadWriteSyncObjectsMap = new Dictionary<string, ReadWriteSyncObjectsStruct>();

        #endregion

        #region IStorageService Members

        public byte[] ReadBinaryResource(string id)
        {
            byte[] ans = null;
            DoBeforeReadResource(id);
            ans = ReadBinaryResourceImp(id);
            DoAfterReadResource(id);

            return ans;
        }        

        public string ReadTextResource(string id)
        {
            string ans = null;
            DoBeforeReadResource(id);
            ans = ReadTextResourceImp(id);
            DoAfterReadResource(id);

            return ans;
        }        

        public void WriteBinaryResource(string id, byte[] toWrite)
        {
            DoBeforeWriteResource(id);
            WriteBinaryResourceImp(id, toWrite);
            DoAfterWriteResource(id);
        }
        
        public void WriteTextResource(string id, string toWrite)
        {
            DoBeforeWriteResource(id);
            WriteTextResourceImp(id, toWrite);
            DoAfterWriteResource(id);
        }        

        public void AppendTextResource(string id, string toWrite)
        {
            DoBeforeWriteResource(id);
            AppendTextResourceImp(id, toWrite);
            DoAfterWriteResource(id);
        }

        public void ClearTextResource(string id)
        {
            WriteTextResource(id, string.Empty);
        }

        public void DeleteResource(string id)
        {
            DoBeforeWriteResource(id);
            DeleteResourceImp(id);
            DoAfterWriteResource(id);

            ReadWriteSyncObjectsMap.Remove(id);
        }

        #endregion

        #region Private Read/Write implementation methods

        private string ReadTextResourceImp(string id)
        {
            string filePath = CalcFilePath(id, false);
            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

            string ans = null;
            using (StreamReader rd = new StreamReader(filePath))
            {
                ans = rd.ReadToEnd();
            }

            return ans;
        }        

        private void WriteTextResourceImp(string id, string toWrite)
        {
            string filePath = CalcFilePath(id, false);

            using (StreamWriter wr = new StreamWriter(filePath))
            {
                wr.Write(toWrite);
            }
        }

        private void DeleteResourceImp(string id)
        {
            string filePath = CalcFilePath(id, false);

            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

            File.Delete(filePath);
        }

        private void AppendTextResourceImp(string id, string toWrite)
        {
            string filePath = CalcFilePath(id, false);

            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

            using (StreamWriter wr = File.AppendText(filePath))
            {
                wr.Write(toWrite);
            }
        }

        private byte[] ReadBinaryResourceImp(string id)
        {
            string filePath = CalcFilePath(id, true);
            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

            byte[] ans = null;
            using (FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader rd = new BinaryReader(fStream))
                {
                    long totalBytes = new System.IO.FileInfo(filePath).Length;
                    ans = rd.ReadBytes((Int32)totalBytes);
                }
            }

            return ans;
        }

        private void WriteBinaryResourceImp(string id, byte[] toWrite)
        {
            string filePath = CalcFilePath(id, true);

            using (FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryWriter wr = new BinaryWriter(fStream))
                {
                    wr.Write(toWrite);
                }
            }
        }

        #endregion

        #region Private Sync methods

        // Sync Read/Write methods
        private void DoBeforeReadResource(string id)
        {
            ReadWriteSyncObjectsStruct syncObjectsStruct = ReadWriteSyncObjectsMap[id];
            lock (syncObjectsStruct.lockWrapReaders)
            {
                syncObjectsStruct.semaphoreReadersAccess.WaitOne();

                lock (syncObjectsStruct.lockReadersCounter)
                {
                    syncObjectsStruct.readersCounter++;
                    if (syncObjectsStruct.readersCounter == 1)
                        syncObjectsStruct.semaphoreWritersAccess.WaitOne();
                }

                syncObjectsStruct.semaphoreReadersAccess.Release();
            }
        }

        private void DoAfterReadResource(string id)
        {
            ReadWriteSyncObjectsStruct syncObjectsStruct = ReadWriteSyncObjectsMap[id];
            lock (syncObjectsStruct.lockReadersCounter)
            {
                syncObjectsStruct.readersCounter--;
                if (syncObjectsStruct.readersCounter == 0)
                    syncObjectsStruct.semaphoreWritersAccess.Release();
            }
        }

        private void DoBeforeWriteResource(string id)
        {
            if (!ReadWriteSyncObjectsMap.ContainsKey(id))
            {
                ReadWriteSyncObjectsMap.Add(id, new ReadWriteSyncObjectsStruct());
            }

            ReadWriteSyncObjectsStruct syncObjectsStruct = ReadWriteSyncObjectsMap[id];
            lock (syncObjectsStruct.lockWritersCounter)
            {
                syncObjectsStruct.writersCounter++;
                if (syncObjectsStruct.writersCounter == 1)
                    syncObjectsStruct.semaphoreReadersAccess.WaitOne();
            }

            syncObjectsStruct.semaphoreWritersAccess.WaitOne();
        }

        private void DoAfterWriteResource(string id)
        {
            ReadWriteSyncObjectsStruct syncObjectsStruct = ReadWriteSyncObjectsMap[id];
            syncObjectsStruct.semaphoreWritersAccess.Release();

            lock (syncObjectsStruct.lockWritersCounter)
            {
                syncObjectsStruct.writersCounter--;
                if (syncObjectsStruct.writersCounter == 0)
                    syncObjectsStruct.semaphoreReadersAccess.Release();
            }
        }

        #endregion

        #region Private Util Methods

        public void CreateResource(string id)
        {
            string filePath = CalcFilePath(id);

            FileStream st = File.Create(filePath);
            st.Close();
        }

        private string CalcFilePath(string fileName)
        {
            return Path.Combine(mBasePath, fileName);
        }

        private string CalcFilePath(string id, bool binary)
        {
            string fileName = string.Format("{0}.{1}", id, binary ? "bin" : "txt");
            return CalcFilePath(fileName);
        }

        #endregion
    }
}
