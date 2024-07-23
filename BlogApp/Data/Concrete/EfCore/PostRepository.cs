using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete
{
    public class PostRepository : IPostRepository
    {
        private BlogContext _blogContext;
        public PostRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        public IQueryable<Post> Posts => _blogContext.Posts;

        public void CratePost(Post post)
        {
            _blogContext.Posts.Add(post);
            _blogContext.SaveChanges();
        }
    }
}