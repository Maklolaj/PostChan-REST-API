using PostChan.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostChan.Services
{
    public interface IPostService
    {
        List<Post> GetPosts();

        Post GetPostById(Guid postId);
    }
}
