using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using PlantProtectionTechnologist.Models;
using System.Net.Http.Json;
using PlantProtectionTechnologist.Models.Recipe;
using PlantProtectionTechnologist.Models.Product;

namespace PlantProtectionTechnologist.ApiClient
{
    public class ApiBase
    {
        public readonly HttpClient _httpClient;
        public ApiBase()
        {

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5026/api/appTechnologi/");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
    }
}
