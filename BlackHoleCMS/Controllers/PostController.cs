using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlackHoleCMS.Data;
using BlackHoleCMS.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;


namespace BlackHoleCMS.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly BlackHoleCmsContext _context;
        private readonly ILogger _logger;

        public PostController(BlackHoleCmsContext context, ILogger<PostController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            return View(await _context.Post.ToListAsync());
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Topics"] = await _context.Topic.ToListAsync();
            ViewData["Statuses"] = await _context.Status.ToListAsync();
            ViewData["Photos"] = await _context.Photo.ToListAsync(); 
            
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,PostedAt,Photo,PhotoId,Topic,Status")] HttpPost h)
        {

            if (ModelState.IsValid)
            {
                var addedPhoto = new Photo();
                if (h.Photo != null)
                {
                    var photo = await UploadImage(h.Photo);
                    addedPhoto = photo;
                }

                Post post = new Post(h.Title, h.Content, h.PostedAt, h.PhotoId ?? addedPhoto.Id, h.Topic, h.Status);
                
                _context.Add(post);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p = await _context.Post.FindAsync(id);
            
            
            if (p == null)
            {
                return NotFound();
            }

            HttpPost httpPost = new(p.Id, p.Title, p.Content, p.PostedAt, p.Photo, p.Topic, p.Status);
            
            ViewData["Topics"] = await _context.Topic.ToListAsync();
            ViewData["Statuses"] = await _context.Status.ToListAsync();
            ViewData["Photos"] = await _context.Photo.ToListAsync();
            
            return View(httpPost);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Photo,PhotoId,PostedAt,Topic,Status")] HttpPost h)
        {
            if (id != h.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var addedPhoto = new Photo();
                    int? i = h.PhotoId; 
                    if (h.Photo != null)
                    {
                        var photo = await UploadImage(h.Photo);
                        addedPhoto = photo;
                        i = null;
                    }
                    
                    
                    Post post = new Post(h.Id, h.Title, h.Content, h.PostedAt, i ?? addedPhoto.Id , h.Topic, h.Status);
                    
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(h.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(h);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public Photo GetImage(int? id)
        {
            Photo photo = _context.Photo.FirstOrDefault(ph => ph.Id == id);
            return photo;
        }

        public List<Photo> GetImages()
        {
            List<Photo> photos = _context.Photo.AsEnumerable().ToList();
            return photos;
        }

        public async Task<Photo> UploadImage(IFormFile? uploadedPhoto)
        {
            var fileStream = new byte[uploadedPhoto.Length];
            uploadedPhoto.OpenReadStream().BeginRead(fileStream, 0, (int) uploadedPhoto.Length, null, null);
            
            Photo photo = new Photo(uploadedPhoto.FileName,fileStream);
                
            _context.Photo.Add(photo);
            await _context.SaveChangesAsync();

            return photo;
        }
        
        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
