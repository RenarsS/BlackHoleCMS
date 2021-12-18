using BlackHoleCMS.Data;
using BlackHoleCMS.HttpModels;
using BlackHoleCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace BlackHoleCMS.Controllers;

[Authorize]
public class PhotoController : Controller
{
    private readonly BlackHoleCmsContext _context;
    private readonly ILogger _logger;

    public PhotoController(BlackHoleCmsContext context, ILogger<PhotoController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    // GET
    public IActionResult Index()
    {
        
        return View(_context.Photo.ToList());
    }
    
    public IActionResult Upload()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(HttpPhoto ph)
    {
        var fileStream = new byte[ph.File.Length];
        ph.File.OpenReadStream().BeginRead(fileStream, 0, (int) ph.File.Length, null, null);
            
        Photo photo = new Photo(ph.File.FileName,fileStream);
                
        _context.Photo.Add(photo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        Photo photo = await _context.Photo.FirstOrDefaultAsync(ph => ph.Id == id);
         _context.Photo.Remove(photo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}