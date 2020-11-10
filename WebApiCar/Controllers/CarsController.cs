using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        /// <summary>
        /// using the local database called MyCarDatabase 
        /// </summary>
        static string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyCarDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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

                            carList.Add(new Car(id, vendor, model, price));

                        }
                    }
                }
            }

            return carList;
        }

        //[Route("/byVendor/{vendor}")]
        [HttpGet(("byVendor/{vendor}"), Name = "GetByVendor")]
        public IEnumerable<Car> GetByVendor(string vendor)
        {
            string sql = $"select id, vendor, model, price from Car Where vendor = '{vendor}'";

            return GetCarsFromDB(sql);
        }


        [HttpGet(("byVendor/{vendor}/price/{price}"), Name = "GetByVendorAndPrice")]
        public IEnumerable<Car> GetByVendorandPrice(string vendor, int price)
        {
            string sql = $"select id, vendor, model, price from Car Where vendor = '{vendor}' and price = {price}";
            return GetCarsFromDB(sql);
        }

        // GET: api/Cars/5
        [HttpGet("{id}", Name = "Get")]
        public Car Get(int id)
        {

            string sql = $"select id, vendor, model, price from Car Where id = {id}";

            //only one row is returned from the DB, so I use FirstOrdefault() at the list
            return GetCarsFromDB(sql).FirstOrDefault();
        }

        /// <summary>
        /// Post a new car to the static list
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Cars
        [HttpPost]
        public void Post([FromBody] Car value)
        {
            string insertCarSql = "insert into car (id,vendor, model, price) values (@id, @vendor, @model, @price)";
            using (SqlConnection databaseconnection = new SqlConnection(conn))
            {
                databaseconnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertCarSql, databaseconnection))
                {
                    insertCommand.Parameters.AddWithValue("@id", value.Id);
                    insertCommand.Parameters.AddWithValue("@vendor", value.Vendor);
                    insertCommand.Parameters.AddWithValue("@model", value.Model);
                    insertCommand.Parameters.AddWithValue("@price", value.Price);
                    int rowaffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine($"rows affected: {rowaffected}");
                }
            }
        }


        // PUT: api/Cars/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Car value)
        {
            string updateCarSql = "update car set vendor = @vendor, model = @model , price = @price where id = @id";
            using (SqlConnection databaseconnection = new SqlConnection(conn))
            {
                databaseconnection.Open();
                using (SqlCommand updateCommand = new SqlCommand(updateCarSql, databaseconnection))
                {
                    updateCommand.Parameters.AddWithValue("@id", value.Id);
                    updateCommand.Parameters.AddWithValue("@vendor", value.Vendor);
                    updateCommand.Parameters.AddWithValue("@model", value.Model);
                    updateCommand.Parameters.AddWithValue("@price", value.Price);
                    int rowaffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine($"rows affected: {rowaffected}");
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            int rowaffected = 0;
            string deleteCarSql = "delete from car where id = @id";
            using (SqlConnection databaseconnection = new SqlConnection(conn))
            {
                databaseconnection.Open();
                using (SqlCommand deleteCommand = new SqlCommand(deleteCarSql, databaseconnection))
                {
                    deleteCommand.Parameters.AddWithValue("@id", id);
                    rowaffected = deleteCommand.ExecuteNonQuery();

                    Console.WriteLine($"rows affected: {rowaffected}");
                }
            }
        }

        //int GetId()
        //{
        //    int max = carList.Max(x => x.Id);
        //    return max + 1;
        //}



        /// <summary>
        /// generic Sql method to use for http gets(select) from the car table
        /// </summary>
        /// <param name="sql">the sql that will be executed to teh Database</param>
        /// <returns>a list of car objects</returns>
        private List<Car> GetCarsFromDB(string sql)
        {
            
            var carList = new List<Car>();

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, databaseConnection))
                {
                    databaseConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string vendor = reader.GetString(1);
                            string model = reader.GetString(2);
                            int price = reader.GetInt32(3);

                            carList.Add(new Car(id, vendor, model, price));

                        }

                    }
                }
            }

            return carList;
        }


    }
}
