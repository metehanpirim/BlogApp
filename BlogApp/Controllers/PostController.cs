using System.IO.Compression;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICommentRepository _commentRepository;

        public PostController(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
        }
        public async Task<IActionResult> Index(string slug)
        {
            var posts = _postRepository.Posts;
            if(!string.IsNullOrEmpty(slug)){
                posts = posts.Where( p => p.Tags.Any( t => t.Url == slug  ));
            }
            return View(await posts.ToListAsync());
        }

        public async Task<IActionResult> Details(string slug){
            return View(await _postRepository
            .Posts
            .Include( p => p.Tags)
            .Include( p => p.Comments)
            .ThenInclude( c => c.User)
            .FirstOrDefaultAsync(p => p.Url == slug));
        }

        public JsonResult AddComment(int PostId, string UserName, string Text){

            var comment = new Comment{
                Text = Text,
                PublishedOn = DateTime.Now.AddSeconds(1),
                PostId = PostId,
                User = new User{ UserName = UserName, Image = "anonymous.jpg"}
            };
            _commentRepository.AddComment(comment);

            return Json( new{
                userName = comment.User.UserName,
                text = comment.Text,
                publishedOn = comment.PublishedOn,
                image = comment.User.Image
            });

        }

    }
}