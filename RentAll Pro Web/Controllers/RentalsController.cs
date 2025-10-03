using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentAll_Pro_web.Data;
using RentAll_Pro_web.Data.Models;
using RentAll_Pro_web.ViewModels;

namespace RentAll_Pro_Web.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rentals.Include(r => r.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address");
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketNr,CustomerId,RentStart,RentalDays,PaymentMode,Comment,Contract,Invoice,TotalAmount,ReviewEmailSent,ContractEmailSent,InvoiceEmailSent")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", rental.CustomerId);
            return View(rental);
        }


        // --- INNEN KEZDŐDIK AZ ÚJ KÓD ---

        // GET: Rentals/CreateRentalTransaction
        public async Task<IActionResult> CreateRentalTransaction()
        {
            var viewModel = new CreateRentalViewModel
            {
                // Előkészítjük az új bérlést alapértelmezett adatokkal
                Rental = new Rental
                {
                    RentStart = DateTime.Now,
                    RentalDays = 1,
                    TicketNr = "RNT" + DateTime.Now.ToString("yyyyMMddHHmmss")
                },
                // Betöltjük a választható ügyfeleket a legördülő listához
                CustomerList = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name"),
                // Betöltjük az elérhető eszközöket
                AvailableDevices = await _context.Devices.Where(d => d.Available).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Rentals/CreateRentalTransaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRentalTransaction(CreateRentalViewModel viewModel)
        {
            // Ellenőrizzük, hogy legalább egy eszközt kiválasztott-e
            if (viewModel.SelectedDeviceIds == null || !viewModel.SelectedDeviceIds.Any())
            {
                ModelState.AddModelError("SelectedDeviceIds", "Legalább egy eszközt ki kell választani a bérléshez!");
            }

            // Csak a ViewModel egy részét validáljuk, a többit mi töltjük fel
            if (ModelState.IsValid && viewModel.SelectedDeviceIds != null && viewModel.SelectedDeviceIds.Any())
            {
                // 1. Mentsük el az új Rental objektumot
                _context.Rentals.Add(viewModel.Rental);
                await _context.SaveChangesAsync(); // Elmentjük, hogy megkapja az új ID-ját

                // 2. Dolgozzuk fel a kiválasztott eszközöket
                foreach (var deviceId in viewModel.SelectedDeviceIds)
                {
                    // Hozzunk létre egy kapcsolati rekordot
                    var rentalDevice = new RentalDevice
                    {
                        RentalId = viewModel.Rental.Id,
                        DeviceId = deviceId
                    };
                    _context.RentalDevices.Add(rentalDevice);

                    // Frissítsük az eszközt: már nem elérhető és növeljük a bérlési számát
                    var device = await _context.Devices.FindAsync(deviceId);
                    if (device != null)
                    {
                        device.Available = false;
                        device.RentCount++;
                        _context.Devices.Update(device);
                    }
                }

                // 3. Mentsük el az összes változást az adatbázisban
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // Sikeres mentés után irányítsuk át a listára
            }

            // Ha hiba történt, töltsük újra az adatokat és jelenítsük meg újra az oldalt
            viewModel.CustomerList = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name", viewModel.Rental.CustomerId);
            viewModel.AvailableDevices = await _context.Devices.Where(d => d.Available).ToListAsync();
            return View(viewModel);
        }

        // --- EDDIG TART AZ ÚJ KÓD ---


        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address", rental.CustomerId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketNr,CustomerId,RentStart,RentalDays,PaymentMode,Comment,Contract,Invoice,TotalAmount,ReviewEmailSent,ContractEmailSent,InvoiceEmailSent")] Rental rental)
        {
            if (id != rental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address", rental.CustomerId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }
    }
}
