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
        public double price { get; set; }
        public string Brand { get; set; }
    }

}
