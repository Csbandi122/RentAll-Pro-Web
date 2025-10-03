using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentAll_Pro_Web.Data;
using RentAll_Pro_Web.Data.Models;
using RentAll_Pro_Web.ViewModels;

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
            if (id == null) return NotFound();
            var rental = await _context.Rentals.Include(r => r.Customer).FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null) return NotFound();
            return View(rental);
        }

        // GET: Rentals/CreateRentalTransaction
        public async Task<IActionResult> CreateRentalTransaction()
        {
            var viewModel = new CreateRentalViewModel
            {
                Rental = new Rental
                {
                    RentStart = DateTime.Now,
                    RentalDays = 1,
                    TicketNr = "RNT" + DateTime.Now.ToString("yyyyMMddHHmmss")
                },
                CustomerList = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name"),
                AvailableDevices = await _context.Devices.Where(d => d.Available).ToListAsync()
            };
            return View(viewModel);
        }

        // POST: Rentals/CreateRentalTransaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRentalTransaction(CreateRentalViewModel viewModel)
        {
            // === VALIDÁCIÓS LOGIKA JAVÍTÁSA ===
            // Mindig távolítsuk el azokat az elemeket, amik nincsenek az űrlapon.
            ModelState.Remove("Rental.Customer");
            ModelState.Remove("CustomerList");
            ModelState.Remove("AvailableDevices");

            // Feltételes validáció:
            if (viewModel.IsNewCustomer)
            {
                ModelState.Remove("Rental.CustomerId"); // Ha új az ügyfél, a legördülő nem kötelező.
            }
            else
            {
                // Ha meglévő ügyfelet választunk, az új ügyfél mezői nem számítanak.
                ModelState.Remove("NewCustomer.Name");
                ModelState.Remove("NewCustomer.Email");
                ModelState.Remove("NewCustomer.Zipcode");
                ModelState.Remove("NewCustomer.City");
                ModelState.Remove("NewCustomer.Address");
                ModelState.Remove("NewCustomer.IdNumber");
            }

            if (viewModel.SelectedDeviceIds == null || !viewModel.SelectedDeviceIds.Any())
            {
                ModelState.AddModelError(nameof(viewModel.SelectedDeviceIds), "Legalább egy eszközt ki kell választani a bérléshez!");
            }

            // === MENTÉSI LOGIKA ===
            if (ModelState.IsValid)
            {
                if (viewModel.IsNewCustomer)
                {
                    _context.Customers.Add(viewModel.NewCustomer);
                    await _context.SaveChangesAsync();
                    viewModel.Rental.CustomerId = viewModel.NewCustomer.Id;
                }

                _context.Rentals.Add(viewModel.Rental);
                await _context.SaveChangesAsync();

                foreach (var deviceId in viewModel.SelectedDeviceIds)
                {
                    _context.RentalDevices.Add(new RentalDevice { RentalId = viewModel.Rental.Id, DeviceId = deviceId });
                    var device = await _context.Devices.FindAsync(deviceId);
                    if (device != null)
                    {
                        device.RentCount++;
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Ha a validáció sikertelen, töltsük újra az adatokat és jelenítsük meg a hibákat.
            viewModel.CustomerList = new SelectList(await _context.Customers.ToListAsync(), "Id", "Name", viewModel.Rental.CustomerId);
            viewModel.AvailableDevices = await _context.Devices.Where(d => d.Available).ToListAsync();
            return View(viewModel);
        }

        // A többi, régi (scaffoldolt) metódust itt hagyhatjuk, vagy akár ki is törölhetjük, ha már nincs rájuk szükség.
        // ... (Edit, Delete, stb.) ...

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }
    }
}