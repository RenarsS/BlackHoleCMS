using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlackHoleCMS.Data;
using BlackHoleCMS.HttpModels;
using BlackHoleCMS.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;

namespace BlackHoleCMS.Controllers
{
    [Authorize]
    public class TopicController : Controller
    {
        private readonly BlackHoleCmsContext _context;
        private readonly ILogger _logger;

        public TopicController(BlackHoleCmsContext context, ILogger<TopicController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Topic
        public async Task<IActionResult> Index()
        {
            return View(await _context.Topic.ToListAsync());
        }
        
        // GET: Topic/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }


        public async Task<IActionResult> Create()
        {
            ViewData["Photos"] = await _context.Photo.ToListAsync(); 
            return View();
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Content, Photo, PhotoId")] HttpTopic t)
        {
            await _context.Database.OpenConnectionAsync();
            
            if (ModelState.IsValid)
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT lookup.topic ON;");
                
                var tpc = await _context.Topic.OrderByDescending(model => model.Id).FirstOrDefaultAsync();
                if (tpc.Id != null)
                {
                    t.Id = (byte) (tpc.Id + 1);
                }
                else
                {
                    t.Id = 0;
                }
                

                var addedPhoto = new Photo();
                if (t.Photo != null)
                {
                    var photo = await UploadImage(t.Photo);
                    addedPhoto = photo;
                }
                
                Topic topic = new Topic(((byte) (tpc.Id + 1)), t.Name, t.Content, t.PhotoId ?? addedPhoto.Id);

                await _context.AddAsync(topic);
                await _context.SaveChangesAsync();
                
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT lookup.topic OFF;");
                await _context.Database.CloseConnectionAsync();
                
                return RedirectToAction(nameof(Index));
            }
            await _context.Database.CloseConnectionAsync();
            
            return View();
        }


        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var t = await _context.Topic.FindAsync(id);
            if (t == null)
            {
                return NotFound();
            }

            HttpTopic httpTopic = new(t.Id, t.Name, t.Content, t.Photo);
            ViewData["Photos"] = await _context.Photo.ToListAsync();
            
            return View(httpTopic);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("Id, Name, Content, Photo, PhotoId")] HttpTopic h)
        {
            if (id != h.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogWarning(h.ToJson());
                    var addedPhoto = new Photo();
                    if (h.Photo != null)
                    {
                        var photo = await UploadImage(h.Photo);
                        addedPhoto = photo;
                    }
                
                    Topic topic = new Topic(h.Id, h.Name, h.Content, h.PhotoId ?? addedPhoto.Id);
                    
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(h.Id))
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


        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            var topic = await _context.Topic.FindAsync(id);
            _context.Topic.Remove(topic);
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
        
        private bool TopicExists(byte id)
        {
            return _context.Topic.Any(e => e.Id == id);
        }
    }
}
