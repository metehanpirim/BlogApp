using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class CommentRepository : ICommentRepository
    {
        private BlogContext _blogContext;

        public CommentRepository(BlogContext blogContext){
            _blogContext = blogContext;
        }
        public IQueryable<Comment> Comments => _blogContext.Comments;

        public void AddComment(Comment comment)
        {
            _blogContext.Comments.Add(comment);
            _blogContext.SaveChanges();
        }
    }
}