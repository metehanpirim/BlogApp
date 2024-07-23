using System.IO.Compression;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
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

    }
}