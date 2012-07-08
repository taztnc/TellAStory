using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FacebookAPIService
{
    [ServiceContract]
    public interface IFacebookAPIService
    {
        [OperationContract]
        FacebookFriend[] GetFriends(String accessToken);
    }

    [DataContract]
    public class FacebookFriend
    {
        [DataMember]
        public String Name;

        [DataMember]
        public String Id;
    }
}
