using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TokensEngine
{
    public static class TokenEngineAccessor
    {
        private static readonly ITokensEngine te = new TokensEngine();
        public static ITokensEngine Get()
        {
            return te;
        }
    }
}
