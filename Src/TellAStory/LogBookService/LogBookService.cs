using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LogBookService.StupidStorageService;

namespace LogBookService
{
    public class LogBookService : ILogBookService
    {
        #region ILogBookService Members
        StorageServiceClient proxy = new StorageServiceClient();

        public void CreateLog(string id)
        {
            proxy.WriteTextResource(LogID2ResourceID(id), string.Empty);
        }

        public void WriteLogLine(string id, string line)
        {
            proxy.AppendTextResource(
                LogID2ResourceID(id), 
                string.Format("{0} : {1}{2}", 
                DateTime.Now.ToString(),
                line, 
                Environment.NewLine));
        }

        public void ClearLog(string id)
        {
            proxy.WriteTextResource(LogID2ResourceID(id), string.Empty);
        }

        public void DeleteLog(string id)
        {
            proxy.DeleteResource(LogID2ResourceID(id));
        }

        #endregion

        #region Private Members

        string LogID2ResourceID(string logID)
        {
            return string.Format("Log_{0}", logID);
        }

        string ResourceID2LogID(string logID)
        {
            return logID.Substring(4); // Skip "Log_"
        }

        #endregion

    }
}
