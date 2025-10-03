using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentAll_Pro_Web.Data;
using RentAll_Pro_Web.Data.Models;
using RentAll_Pro_Web.ViewModels;

namespace RentAll_Pro_web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        // GET: Services/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateServiceViewModel
            {
                // Betöltjük az összes eszközt a listába
                AllDevices = await _context.Devices.ToListAsync(),
                // Előre kitöltünk néhány mezőt
                Service = new Service
                {
                    ServiceDate = DateTime.Now,
                    TicketNr = "SRV" + DateTime.Now.ToString("yyyyMMddHHmmss")
                }
            };
            return View(viewModel);
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceViewModel viewModel)
        {
            if (viewModel.SelectedDeviceIds == null || !viewModel.SelectedDeviceIds.Any())
            {
                ModelState.AddModelError(nameof(viewModel.SelectedDeviceIds), "Legalább egy eszközt ki kell választani a szervizeléshez!");
            }

            if (ModelState.IsValid)
            {
                // 1. Mentsük el a Service bejegyzést
                _context.Services.Add(viewModel.Service);
                await _context.SaveChangesAsync(); // Elmentjük, hogy kapjon egy ID-t

                // 2. Rendeljük hozzá a kiválasztott eszközöket
                foreach (var deviceId in viewModel.SelectedDeviceIds)
                {
                    var serviceDevice = new ServiceDevice
                    {
                        ServiceId = viewModel.Service.Id,
                        DeviceId = deviceId
                    };
                    _context.ServiceDevices.Add(serviceDevice);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Hiba esetén töltsük újra az eszközlistát
            viewModel.AllDevices = await _context.Devices.ToListAsync();
            return View(viewModel);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketNr,ServiceType,Description,Technician,ServiceDate,CostAmount")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
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
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
