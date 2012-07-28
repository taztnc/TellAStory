using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TokensEngine
{
    public interface ITokensEngine
    {
        String CreateUniquSessionToken();
        String ResolveFacebookToken(String sessionToken);
        void BindSessionTokenToFacebookToken(String sessionToken, String facebookToken);
    }
}
