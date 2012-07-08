using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FacebookAPIService
{
    public class FacebookAPIService : IFacebookAPIService
    {
        private const string GET_FRIENDS_LIST_URL_FORMAT = "https://graph.facebook.com/me/friends?access_token={0}";


        public FacebookFriend[] GetFriends(string accessToken)
        {
            String urlFormat = GET_FRIENDS_LIST_URL_FORMAT;
            String url = String.Format(urlFormat, accessToken);
            List<FacebookFriend> friends = new List<FacebookFriend>();

            JObject obj = ExecuteFacebookCall(url);
            ExtractFriendsList(obj, friends);

            if (friends.Count == 0)
            {
                throw (new Exception("The user is Galmud"));
            }
            return friends.ToArray();
        }

        private static void ExtractFriendsList(JObject obj, List<FacebookFriend> friends)
        {
            JToken jList = (JToken)obj["data"];
            foreach (JToken token in jList)
            {
                friends.Add(new FacebookFriend()
                {
                    Name = (String)token["name"],
                    Id = (String)token["id"]
                });
            }
        }

        /// <summary>
        /// Utility methis. Directly execute a facebook api call.
        /// </summary>
        /// <param name="url">The facebook api URL to call</param>
        /// <returns>JSON object representing the return value of the call</returns>
        private static JObject ExecuteFacebookCall(string url)
        {
            if (url == null)
            {
                throw (new ArgumentNullException("url"));
            }

            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JObject obj = JObject.Parse(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            return obj;
        }
    }
}
