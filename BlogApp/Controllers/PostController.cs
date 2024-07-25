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

        public IActionResult AddComment(int PostId, string UserName, string Text, string Url){
            var comment = new Comment{
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                User = new User{ UserName = UserName, Image = "anonymous.jpg"}
            };
            _commentRepository.AddComment(comment);
            return RedirectToRoute("post_details", new {slug = Url});

        }

    }
}