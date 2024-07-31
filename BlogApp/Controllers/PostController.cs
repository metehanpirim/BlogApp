using System.IO.Compression;
using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var comment = new Comment{
                Text = Text,
                PublishedOn = DateTime.Now.AddSeconds(1),
                PostId = PostId,
                UserId = int.Parse(userId ?? "")
            };
            _commentRepository.AddComment(comment);

            return Json( new{
                userName = userName,
                text = comment.Text,
                publishedOn = comment.PublishedOn,
                image = User.FindFirstValue(ClaimTypes.UserData)
            });

        }

        [Authorize]
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model){
            if(ModelState.IsValid){

                var post = _postRepository.Posts.FirstOrDefault(p => p.Url == model.Url);

                if(post == null){
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

                    _postRepository.CratePost(new Post{
                        UserId = int.Parse(userId),
                        Title = model.Title,
                        Content = model.Content,
                        Description = model.Description,
                        Url = model.Url,
                        PublishedOn = DateTime.Now.AddSeconds(1),
                        Image = "6.jpg",
                        IsActive = false
                    });

                    return RedirectToAction("Index", "Post");
                }
                else{
                    ModelState.AddModelError("", "A post exists with the given url!");
                    return View(model);
                }
            }
            return View(model);
        }
        
        [Authorize]
        public async Task<IActionResult> List(){

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;

            if(string.IsNullOrEmpty(role) || role != "admin"){
                posts = posts.Where( p => p.UserId == userId);
            }

            return View( await posts.ToListAsync() );
        }

    }
}