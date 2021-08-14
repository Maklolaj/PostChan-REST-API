using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostChan.Contracts.V1.Responses
{
    public class AuthSuccesResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
