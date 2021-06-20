using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostChan.Domain;
using Microsoft.AspNetCore.Mvc;
using PostChan.Contracts.V1;

namespace PostChan.Controllers.V1
{
    public class PostsController : Controller
    {
        private List<Post> _posts;

        public PostsController()
        {
            _posts = new List<Post>();

            for(int i = 1; i<5; i++)
            {
                _posts.Add(new Post { Id = Guid.NewGuid().ToString() });
            }
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);
        }
    }
}
