using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace TellAStoryServiceLib
{
    // kobig - should handle thread safety in all functions
    // kobig - should catch exceptions
    // kobig - should throw normal exceptions

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class StupidStorageService : IStorageService
    {
        private static string mBasePath = @"C:\temp\TellAStoryStorage";

        #region IStorageService Members        

        public int CreateBinaryResource()
        {
            return CreateResource(true);
        }

        public int CreateTextResource()
        {
            return CreateResource(false);
        }

        public byte[] ReadBinaryResource(int id)
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

        public string ReadTextResource(int id)
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

        public void WriteBinaryResource(int id, byte[] toWrite)
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

        public void WriteTextResource(int id, string toWrite)
        {
            string filePath = CalcFilePath(id, false);
            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

            using (StreamWriter wr = new StreamWriter(filePath))
            {
                wr.Write(toWrite);
            }
        }

        public void AppendTextResource(int id, string toWrite)
        {
            string filePath = CalcFilePath(id, false);
            if (!File.Exists(filePath))
                throw new Exception("Wrong ID");

            using (StreamWriter wr = File.AppendText(filePath))
            {                
                wr.Write(toWrite);
            }
        }

        public void ClearTextResource(int id)
        {
            WriteTextResource(id, string.Empty);
        }

        public void DeleteBinaryResource(int id)
        {
            // kobig - Cannot implemet properly with current method of having a file with the last id. Need a better method, like holding a linked list of all ids
            throw new NotImplementedException();
        }

        public void DeleteTextResource(int id)
        {
            // kobig - Cannot implemet properly with current method of having a file with the last id. Need a better method, like holding a linked list of all ids
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

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
