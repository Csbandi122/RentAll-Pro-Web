using Microsoft.AspNetCore.Mvc.Rendering;
using RentAll_Pro_Web.Data.Models;
using System.Collections.Generic;

namespace RentAll_Pro_Web.ViewModels
{
    public class CreateRentalViewModel
    {
        // Ide kerülnek majd a bérlési űrlap adatai
        public Rental Rental { get; set; }

        // Ebből lesz a legördülő lista az ügyfelek kiválasztásához
        public SelectList CustomerList { get; set; }

        // Itt tároljuk a választható, elérhető eszközöket
        public List<Device> AvailableDevices { get; set; }

        // Ebben a listában kapjuk vissza a kiválasztott eszközök azonosítóit
        public List<int> SelectedDeviceIds { get; set; }

        public CreateRentalViewModel()
        {
            // Inicializáljuk a listát, hogy ne legyen null
            SelectedDeviceIds = new List<int>();
        }
    }
}