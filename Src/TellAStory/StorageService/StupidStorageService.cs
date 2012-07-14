using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Threading;

namespace TellAStoryServiceLib
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
        Dictionary<int, ReadWriteSyncObjectsStruct> ReadWriteSyncObjectsMap = new Dictionary<int, ReadWriteSyncObjectsStruct>();

        #endregion

        #region IStorageService Members

        public int CreateBinaryResource()
        {
            int ans = 0;
            lock (lockBinary)
            {
                ans = CreateResource(true);
            }

            ReadWriteSyncObjectsMap.Add(ans, new ReadWriteSyncObjectsStruct());

            return ans;
        }

        public int CreateTextResource()
        {
            int ans = 0;
            lock (lockText)
            {
                ans = CreateResource(false);
            }

            ReadWriteSyncObjectsMap.Add(ans, new ReadWriteSyncObjectsStruct());

            return ans;
        }

        public byte[] ReadBinaryResource(int id)
        {
            byte[] ans = null;
            DoBeforeReadResource(id);
            ans = ReadBinaryResourceImp(id);
            DoAfterReadResource(id);

            return ans;
        }        

        public string ReadTextResource(int id)
        {
            string ans = null;
            DoBeforeReadResource(id);
            ans = ReadTextResourceImp(id);
            DoAfterReadResource(id);

            return ans;
        }        

        public void WriteBinaryResource(int id, byte[] toWrite)
        {
            DoBeforeWriteResource(id);
            WriteBinaryResourceImp(id, toWrite);
            DoAfterWriteResource(id);
        }
        
        public void WriteTextResource(int id, string toWrite)
        {
            DoBeforeWriteResource(id);
            WriteTextResourceImp(id, toWrite);
            DoAfterWriteResource(id);
        }        

        public void AppendTextResource(int id, string toWrite)
        {
            DoBeforeWriteResource(id);
            AppendTextResourceImp(id, toWrite);
            DoAfterWriteResource(id);
        }
        
        public void ClearTextResource(int id)
        {
            WriteTextResource(id, string.Empty);
        }

        public void DeleteBinaryResource(int id)
        {
            lock (lockBinary)
            {
                // kobig - Cannot implemet properly with current method of having a file with the last id. Need a better method, like holding a linked list of all ids
                // kobig - The implementation has to include sync as a writer
                // kobig - What happens if somone wants to read and waits for me while someone else deletes the resource ???? We should throw a proper exception



                throw new NotImplementedException();
            }

            ReadWriteSyncObjectsMap.Remove(id);
        }

        public void DeleteTextResource(int id)
        {
            lock (lockText)
            {
                // kobig - Cannot implemet properly with current method of having a file with the last id. Need a better method, like holding a linked list of all ids
                // kobig - The implementation has to include sync as a writer
                // kobig - What happens if somone wants to read and waits for me while someone else deletes the resource ???? We should throw a proper exception


                throw new NotImplementedException();
            }

            // kobig - think if the access to this object should be synched - it is a general question related to services
            ReadWriteSyncObjectsMap.Remove(id);
        }

        #endregion

        #region Private Read/Write implementation methods

        private string ReadTextResourceImp(int id)
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

        private void WriteTextResourceImp(int id, string toWrite)
        {
            string filePath = CalcFilePath(id, false);
            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

            using (StreamWriter wr = new StreamWriter(filePath))
            {
                wr.Write(toWrite);
            }
        }

        private void AppendTextResourceImp(int id, string toWrite)
        {
            string filePath = CalcFilePath(id, false);
            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

            using (StreamWriter wr = File.AppendText(filePath))
            {
                wr.Write(toWrite);
            }
        }

        private byte[] ReadBinaryResourceImp(int id)
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

        private void WriteBinaryResourceImp(int id, byte[] toWrite)
        {
            string filePath = CalcFilePath(id, true);
            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

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
        private void DoBeforeReadResource(int id)
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

        private void DoAfterReadResource(int id)
        {
            ReadWriteSyncObjectsStruct syncObjectsStruct = ReadWriteSyncObjectsMap[id];
            lock (syncObjectsStruct.lockReadersCounter)
            {
                syncObjectsStruct.readersCounter--;
                if (syncObjectsStruct.readersCounter == 0)
                    syncObjectsStruct.semaphoreWritersAccess.Release();
            }
        }

        private void DoBeforeWriteResource(int id)
        {
            ReadWriteSyncObjectsStruct syncObjectsStruct = ReadWriteSyncObjectsMap[id];
            lock (syncObjectsStruct.lockWritersCounter)
            {
                syncObjectsStruct.writersCounter++;
                if (syncObjectsStruct.writersCounter == 1)
                    syncObjectsStruct.semaphoreReadersAccess.WaitOne();
            }

            syncObjectsStruct.semaphoreWritersAccess.WaitOne();
        }

        private void DoAfterWriteResource(int id)
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

        public int CreateResource(bool binary)
        {
            int id = GenerateID(binary);
            string filePath = CalcFilePath(id, binary);

            FileStream st = File.Create(filePath);
            st.Close();

            return id;
        }

        private string CalcFilePath(string fileName)
        {
            return Path.Combine(mBasePath, fileName);
        }

        private string CalcFilePath(int id, bool binary)
        {
            string fileName = string.Format("{0}.{1}", id, binary ? "bin" : "txt");
            return CalcFilePath(fileName);
        }

        private int GenerateID(bool binary)
        {
            // kobig - Should write this function with another method           
            string filePath = CalcFilePath(binary ? "binary_ids" : "text_ids");
            string lastIdString = "";
            int ans = 1;

            if (!File.Exists(filePath))
            {
                string dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                using (StreamWriter wr = new StreamWriter(filePath))
                {
                    wr.Write("1");
                }

                return ans;
            }

            using (StreamReader rd = new StreamReader(filePath))
            {
                lastIdString = rd.ReadLine();
            }

            ans = int.Parse(lastIdString) + 1;

            File.Delete(filePath);

            using (StreamWriter wr = new StreamWriter(filePath))
            {
                wr.Write(ans.ToString());
            }

            return ans;
        }        

        #endregion
        
    }
}
