using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

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

        public void EditPost(Post post, int[] tagIds)
        {
            var entity = _blogContext.Posts.Include(p => p.Tags).FirstOrDefault( p => p.PostId == post.PostId);

            if (entity != null){
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive; 

                entity.Tags = [.. _blogContext.Tags.Where( tag => tagIds.Contains(tag.TagId) )];

                _blogContext.SaveChanges();
            }


        }
    }
}