using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostChan.Domain
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public bool Succes { get; set; }

        public IEnumerable<string> Errors{ get; set; }

    }
}
