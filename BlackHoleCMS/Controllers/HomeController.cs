using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlackHoleCMS.Data;
using Microsoft.AspNetCore.Mvc;
using BlackHoleCMS.Models;
using BlackHoleCMS.ViewModels;
using Microsoft.Extensions.Logging;

namespace BlackHoleCMS.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlackHoleCmsContext _context;

        public HomeController(ILogger<HomeController> logger, BlackHoleCmsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var data = new ViewPostTopic
            {
                Topics = GetViewTopics(),
                Posts = GetViewPosts()
            };


            return View(data);
        }

        public IActionResult Topic(sbyte? id)
        {
            
            List<ViewPost> posts = GetAllViewPosts(id);
            
            var t = _context.Topic.Single(item => item.Id == id);
            var p = _context.Photo.FirstOrDefault(item => item.Id == t.Photo);
            
            ViewTopic topic = new(t.Id, t.Name, t.Content, p);
            ViewData["Topic"] = topic;
            return View(posts);
        }

        public IActionResult Post(int? id)
        {
            Post? post = _context.Post.FirstOrDefault(item => item.Id == id);

            if (post.Photo != null)
            {
                ViewBag.Photo = _context.Photo.FirstOrDefault(photo => photo.Id == post.Photo);
            }
            else
            {
                ViewBag.Photo = null;
            }
            
            return View(post);
        }

        public List<ViewPost> GetViewPosts(byte? id = 0)
        {
            var dbPosts = _context.Post.AsEnumerable().OrderByDescending(p => p.PostedAt).ToList().Take(6);
            
            if (id != 0)
            {
                dbPosts = _context.Post.Where(item => item.Topic == id && item.Status != 5).AsEnumerable().ToList();
            }
            
            List<ViewPost> posts = new ();

            foreach (var p in dbPosts)
            {
                var photo = _context.Photo.FirstOrDefault(ph => ph.Id == p.Photo);
                posts.Add(new ViewPost(p.Id, p.Title, p.Content, p.PostedAt, photo, p.Topic, p.Status));
            }

            return posts;
        }
        
        public List<ViewPost> GetAllViewPosts(sbyte? id = -1)
        {
            var dbPosts = _context.Post.AsEnumerable().OrderByDescending(p => p.PostedAt).ToList();
            
            if (id != -1)
            {
                dbPosts = _context.Post.Where(item => item.Topic == id && item.Status != 5).AsEnumerable().ToList();
            }
            
            List<ViewPost> posts = new ();

            foreach (var p in dbPosts)
            {
                var photo = _context.Photo.FirstOrDefault(ph => ph.Id == p.Photo);
                posts.Add(new ViewPost(p.Id, p.Title, p.Content, p.PostedAt, photo, p.Topic, p.Status));
            }

            return posts;
        }
        
        public List<ViewTopic> GetViewTopics()
        {
            var dbTopics = _context.Topic.ToList();

            List<ViewTopic> topics = new ();

            foreach (var t in dbTopics)
            {
                var photo = _context.Photo.FirstOrDefault(ph => ph.Id == t.Photo);
                topics.Add(new ViewTopic(t.Id, t.Name, t.Content, photo));
            }

            return topics;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}