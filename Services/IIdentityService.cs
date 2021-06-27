﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostChan.Domain;

namespace PostChan.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
    }
}
