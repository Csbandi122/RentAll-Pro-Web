using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentAll_Pro_Web.Data;
using RentAll_Pro_Web.Data.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace RentAll_Pro_Web.Controllers
{
    public class DevicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DevicesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Devices.Include(d => d.DeviceType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.DeviceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            ViewData["DeviceTypeId"] = new SelectList(_context.DeviceTypes, "Id", "TypeName");
            return View();
        }

        // POST: Devices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DeviceName,DeviceTypeId,Serial,Price,RentPrice,Available,Picture,RentCount,Notes")] Device device, IFormFile? pictureFile)
        {
            ModelState.Remove("DeviceType"); // <-- EZ A JAVÍTÁS

            if (ModelState.IsValid)
            {
                if (pictureFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(pictureFile.FileName);
                    string path = Path.Combine(wwwRootPath, "images", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await pictureFile.CopyToAsync(fileStream);
                    }
                    device.Picture = "/images/" + fileName; // Itt mentjük az elérési utat
                }

                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeviceTypeId"] = new SelectList(_context.DeviceTypes, "Id", "TypeName", device.DeviceTypeId);
            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            ViewData["DeviceTypeId"] = new SelectList(_context.DeviceTypes, "Id", "TypeName", device.DeviceTypeId);
            return View(device);
        }

        // POST: Devices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DeviceName,DeviceTypeId,Serial,Price,RentPrice,Available,Picture,RentCount,Notes")] Device device, IFormFile? pictureFile)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            // Mivel a kép útvonalát nem küldjük vissza a formról, ki kell olvasnunk az adatbázisból,
            // hogy ne vesszen el, ha nem töltünk fel új képet.
            var deviceFromDb = await _context.Devices.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (deviceFromDb != null)
            {
                device.Picture = deviceFromDb.Picture;
            }


            if (ModelState.IsValid)
            {
                if (pictureFile != null)
                {
                    // Töröljük a régi képet, ha van
                    if (!string.IsNullOrEmpty(device.Picture))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, device.Picture.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Mentsük az új képet
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(pictureFile.FileName);
                    string path = Path.Combine(wwwRootPath, "images", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await pictureFile.CopyToAsync(fileStream);
                    }
                    device.Picture = "/images/" + fileName;
                }

                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
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
            ViewData["DeviceTypeId"] = new SelectList(_context.DeviceTypes, "Id", "TypeName", device.DeviceTypeId);
            return View(device);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.DeviceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                // Töröljük a képet is a szerverről
                if (!string.IsNullOrEmpty(device.Picture))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, device.Picture.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Devices.Remove(device);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}