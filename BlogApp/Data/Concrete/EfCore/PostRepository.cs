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

        public void CreatePost(Post post)
        {
            _blogContext.Posts.Add(post);
            _blogContext.SaveChanges();
        }

        public void EditPost(Post post)
        {
            var entity = _blogContext.Posts.FirstOrDefault( p => p.PostId == post.PostId);

            if (entity != null){
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive; 
                _blogContext.SaveChanges();
            }
        }
    }
}