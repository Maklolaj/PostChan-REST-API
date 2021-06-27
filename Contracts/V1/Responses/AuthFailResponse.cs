using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostChan.Contracts.V1.Responses
{
    public class AuthFailResponse
    {
        public IEnumerable<string> Errors{ get; set; }
    }
}
