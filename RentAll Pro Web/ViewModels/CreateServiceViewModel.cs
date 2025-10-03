using Microsoft.AspNetCore.Mvc.Rendering;
using RentAll_Pro_Web.Data.Models;
using System.Collections.Generic;

namespace RentAll_Pro_Web.ViewModels
{
    public class CreateServiceViewModel
    {
        // Az űrlap adatai
        public Service Service { get; set; }

        // Itt tároljuk az összes eszközt a kiválasztáshoz
        public List<Device> AllDevices { get; set; }

        // Ebben kapjuk vissza a kiválasztott eszközök azonosítóit
        public List<int> SelectedDeviceIds { get; set; }

        public CreateServiceViewModel()
        {
            SelectedDeviceIds = new List<int>();
            Service = new Service();
        }
    }
}