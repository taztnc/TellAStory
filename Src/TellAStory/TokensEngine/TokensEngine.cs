using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TokensEngine.LogBookService;

namespace TokensEngine
{
    class TokensEngine : ITokensEngine
    {
        private Dictionary<String, String> SessionTokenToFacebookTokenDictionary = 
            new Dictionary<String, String>();
        private Object syncObj = new Object();
        private LogBookServiceClient logBook = new LogBookServiceClient();
        private static String LOG_ID = "TokensEngine";

        public TokensEngine()
        {
            logBook.CreateLog(LOG_ID);
        }

        public String CreateUniquSessionToken()
        { 
            String ret = Guid.NewGuid().ToString();
            logBook.WriteLogLine(LOG_ID, String.Format(
                "a new Session token has been created : {0}", ret));
            return ret;
        }

        public String ResolveFacebookToken(String sessionToken)
        {
            String ret = null;
            lock (syncObj)
            {
                ret = SessionTokenToFacebookTokenDictionary[sessionToken];
            }

            logBook.WriteLogLine(LOG_ID, String.Format(
                "Token was resolved. Session token: {0} , resolved facebook token{1}", sessionToken, ret));
            return ret;
            
        }

        public void BindSessionTokenToFacebookToken(String sessionToken, String facebookToken)
        {
            lock (syncObj)
            {
                SessionTokenToFacebookTokenDictionary.Add(sessionToken, facebookToken);
            }
            logBook.WriteLogLine(LOG_ID, String.Format(
                "Tokens were binded. Session token: {0} ,  facebook token : {1}", sessionToken, facebookToken));
        }
    }
}
