using Microsoft.AspNetCore.Mvc.Rendering;
using RentAll_Pro_Web.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentAll_Pro_Web.ViewModels
{
    public class CreateRentalViewModel
    {
        public Rental Rental { get; set; }
        public SelectList CustomerList { get; set; }
        public List<Device> AvailableDevices { get; set; }
        public List<int> SelectedDeviceIds { get; set; }

        // --- ÚJ RÉSZEK ---
        // Ebbe a propertybe kötjük a checkboxot, ami jelzi, ha új ügyfelet veszünk fel
        public bool IsNewCustomer { get; set; }

        // Itt tároljuk az új ügyfél adatait, ha megadja őket
        public Customer NewCustomer { get; set; }
        // --- ÚJ RÉSZEK VÉGE ---

        public CreateRentalViewModel()
        {
            SelectedDeviceIds = new List<int>();
            NewCustomer = new Customer();
        }
    }
}