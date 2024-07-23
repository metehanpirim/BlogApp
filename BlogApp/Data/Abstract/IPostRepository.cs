using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository{

        IQueryable<Post> Posts{ get; }

        void CratePost(Post post);

    }
}
