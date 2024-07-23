using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogContext _blogContext;
        public TagRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public IQueryable<Tag> Tags => _blogContext.Tags;

        public void CreateTag(Tag tag)
        {
            _blogContext.Tags.Add(tag);
            _blogContext.SaveChanges();
        }
    }
}