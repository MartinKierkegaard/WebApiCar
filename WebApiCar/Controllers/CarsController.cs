using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCar.Model;

namespace WebApiCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        static string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CarDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static List<Car> carList = new List<Car>()
        {
            new Car(){Id = 1,Model="x3",Vendor="Tesla", Price=400000},
            new Car(){Id = 2,Model="x2",Vendor="Tesla", Price=600000},
            new Car(){Id = 3,Model="x1",Vendor="Tesla", Price=800000},
            new Car(){Id = 4,Model="x0",Vendor="Tesla", Price=1400000},
        };

        /// <summary>
        /// Method for get all the cars from the static list
        /// </summary>
        /// <returns>List of cars</returns>
        // GET: api/Cars
        [HttpGet]
        public IEnumerable<Car> Get()
        {
            var carList = new List<Car>();

            string selectall = "select id, vendor, model, price from Car";

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                using (SqlCommand selectCommand = new SqlCommand(selectall, databaseConnection))
                {
                    databaseConnection.Open();

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string vendor = reader.GetString(1);
                            string model = reader.GetString(2);
                            int price = reader.GetInt32(3);

                            carList.Add(new Car(id,vendor,model,price));

                        }

                    }
                }


                    }

                return carList;
        }

        //[Route("/byVendor/{vendor}")]
        [HttpGet(("byVendor/{vendor}"), Name ="GetByVendor")]
        public IEnumerable<Car> GetByVendor(string vendor)
        {
            //should be an SQL statement
            return carList.Where(x=> x.Vendor == vendor);
        }

        [HttpGet(("byVendor/{vendor}/price/{price}"), Name = "GetByVendorAndPrice")]
        public IEnumerable<Car> GetByVendorandPrice(string vendor, int price)
        {
            //shold be an sql statements
            return carList.Where(x => x.Vendor == vendor && x.Price == price);
        }

        // GET: api/Cars/5
        [HttpGet("{id}", Name = "Get")]
        public Car Get(int id)
        {
            return carList.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Post a new car to the static list
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Cars
        [HttpPost]
        public void Post([FromBody] Car value)
        {
            Car newcar = new Car() { Id = GetId(), Model = value.Model, Vendor = value.Vendor, Price = value.Price };
            carList.Add(newcar);
        }

        // PUT: api/Cars/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Car value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            carList.Remove(Get(id));
        }

       int GetId()
        {
            int max = carList.Max(x => x.Id);
            return max+1;
        }

    }
}
