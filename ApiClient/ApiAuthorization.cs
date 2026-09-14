using PlantProtectionTechnologist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlantProtectionTechnologist.ApiClient
{
    class ApiAuthorization : ApiBase
    {
        public async Task<DataUser?> GetDataAuthorization(string log, string pass)
        {
            try
            {
                string url = $"authorization?log={Uri.EscapeDataString(log)}&pass={Uri.EscapeDataString(pass)}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string content = await response.Content.ReadAsStringAsync();

                DataUser? result = JsonSerializer.Deserialize<DataUser>(content);

                return result;

            }
            catch (Exception ex)
            {
                return new DataUser();
            }
        }
    }
}
