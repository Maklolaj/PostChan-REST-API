﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostChan.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Base = "api";

        public const string Version = "v1";

        public const  string Root = Base + "/" + Version;

        public static class Posts
        {
            public const string GetAll = Base + "/" + "posts";

            public const string Create = Base + "/" + "posts";

            public const string Get = Base + "/" + "posts/{postId}";
        }
    }



}
