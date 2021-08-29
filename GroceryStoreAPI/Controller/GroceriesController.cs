using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using GroceryStoreAPI.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controller
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class GroceriesController : ControllerBase
    {
        private readonly string jsonFile = Path.Combine(Directory.GetCurrentDirectory(), $"{"database.json"}");
        private string jsonString;
        private CustomerList getCustomerListFromJsonFile()
        {
            using (var reader = new StreamReader(jsonFile))
            {
                jsonString = reader.ReadToEnd();
            }
            return JsonSerializer.Deserialize<CustomerList>(jsonString);
        }

        // GET: api/<GroceriesController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string jsonString;
                using (var reader = new StreamReader(jsonFile))
                {
                    jsonString = reader.ReadToEnd();
                }

                CustomerList customers =  JsonSerializer.Deserialize<CustomerList>(jsonString);
                return Ok(customers);
            } 
            catch (Exception ex)
            {
                return Problem("ERROR", ex.Message);
            }
        }

        // GET api/<GroceriesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                CustomerList customerList = getCustomerListFromJsonFile();

                var myCustomer = customerList.Customer.Where(e => e.Id == id);
                if (myCustomer.Count() > 0)
                {
                    return Ok(myCustomer);
                } else
                {
                    return BadRequest("ERROR: Customer does not exist");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("ERROR: " + ex.Message);
            }
        }

        // POST api/<GroceriesController>/John%20Doe
        [HttpPost("{name}")]
        public IActionResult Post(string name)
        {
            try
            {
                CustomerList customerList = getCustomerListFromJsonFile();

                Customer newCustomer = new Customer();
                newCustomer.Id = customerList.Customer.Max(e => e.Id) + 1;  //TODO: Write Code to ensure IDs are never the same if two requests sent simultaneously
                newCustomer.Name = name;
                customerList.Customer.Add(newCustomer);

                jsonString = JsonSerializer.Serialize(customerList);
                System.IO.File.WriteAllText(jsonFile, jsonString);
              
                return Ok(newCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest("ERROR: " + ex.Message);
            }
        }

        // PUT api/<GroceriesController>/5/Jane%20Doe
        [HttpPut("{id}/{name}")]
        public IActionResult Put(int id, string name) //finds ID and replaces with (new) name
        {
            try
            {
                CustomerList customerList = getCustomerListFromJsonFile();

                Customer myCustomer = (Customer)customerList.Customer.Where(e => e.Id == id).FirstOrDefault();
                if (myCustomer != null)
                {
                    myCustomer.Name = name;
                } else
                {
                    return BadRequest("ERROR: User with ID " + id + " does not exist");
                }

                jsonString = JsonSerializer.Serialize(customerList);
                System.IO.File.WriteAllText(jsonFile, jsonString);

                return Ok(myCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest("ERROR: " + ex.Message);
            }
        }

        // DELETE api/<GroceriesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                CustomerList customerList = getCustomerListFromJsonFile();
                Customer myCustomer = (Customer)customerList.Customer.Where(e => e.Id == id).FirstOrDefault();
                if (myCustomer != null)
                {
                    customerList.Customer.Remove(myCustomer);
                }
                else
                {
                    return BadRequest("ERROR: User with ID " + id + " does not exist");
                }

                jsonString = JsonSerializer.Serialize(customerList);
                System.IO.File.WriteAllText(jsonFile, jsonString);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("ERROR: " + ex.Message);
            }
        }   
    }
}
