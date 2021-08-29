using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroceryStoreAPI.Model
{

    public class CustomerList
    {
        [JsonPropertyName("customers")]
        public List<Customer> Customer { get; set; }

    }



    public class Customer
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }

}
