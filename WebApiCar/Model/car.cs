using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCar.Model
{

    /// <summary>
    /// model klasse for car 
    /// som bruges i web api'en
    /// </summary>
    public class Car
    {
        public int Id { get; set; }
        public string Vendor { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
    }
}
