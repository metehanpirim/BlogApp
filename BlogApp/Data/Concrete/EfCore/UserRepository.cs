using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogContext _blogContext;

        public UserRepository(BlogContext blogContext){
            _blogContext = blogContext;
        }
        public IQueryable<User> Users => _blogContext.Users;

        public void CreateUser(User user)
        {
            _blogContext.Users.Add(user);
            _blogContext.SaveChanges();
        }
    }
}