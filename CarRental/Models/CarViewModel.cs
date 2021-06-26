using Microsoft.AspNetCore.Http;
using Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class CarViewModel
    {
        public string Brand_Search { get; set; }
        public string FromDate_Search { get; set; }
        public string ToDate_Search { get; set; }
        public List<Car> Cars { get; set; }
    }
    public class CarViewEtity
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoFile { get; set; }
        public double price { get; set; }
        public string Brand { get; set; }
    }
    public class Order
    {
        [Required]
        public string CarID { get; set; }
        [Required]
        public string Start_Date { get; set; }
        [Required]
        public string End_Date { get; set; }
        [Required]
        public string Pick_Location { get; set; }
        [Required]
        public string Drop_Location { get; set; }
        [Required]
        [Phone]
        public string Contact_No { get; set; }
        [Required]
        public string Contact_Person { get; set; }
    }

    public class OrderSearch
    {
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public string CarID { get; set; }
    }

}
