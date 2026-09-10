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

namespace PlantProtectionTechnologist
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        public ApiClient()
        {

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7223/api/appTechnologi/");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

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

        public async Task<ProductDto[]?> GetDataProduction()
        {
            try
            {
                string url = $"getDataProduction";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ProductDto[]>(content);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if (item.activeTechMapFill == "v")
                        {
                            item.activeTechMapFill = "Не найдено";
                        }

                        if (item.activeRecipeFill == "v0")
                        {
                            item.activeRecipeFill = "Не найдено";
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return null;
            }
        }

        public async Task<bool> AddNewProduct(ConfirmationProduct newProduct)
        {
            try
            {
                string url = $"addNewProduct";

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, newProduct);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Ошибка");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return false;
            }
        }

        public async Task<bool> EditProduct(ConfirmationProduct editProduct)
        {
            try
            {
                string url = $"editProduct";

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, editProduct);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Ошибка");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return false;
            }
        }

        public async Task<bool> ChangetStatusProduct(ConfirmationProduct? editProduct)
        {
            try
            {
                if (editProduct == null)
                {
                    return false;
                }

                string url = $"changetStatusProduct";

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, editProduct);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Ошибка");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                return false;
            }

        }

        public async Task<RecipesData[]?> GetDataRecipes()
        {
            try
            {
                string url = $"getDataRecipes";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка API: {response.StatusCode}");
                    return null;
                }

                var result = JsonSerializer.Deserialize<RecipesData[]>(content);

                if (result == null)
                {
                    MessageBox.Show("Ошибка");
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return null;
            }
        }

        public async Task<RecipeComponets[]?> GetDataRecipeComponets(int id)
        {
            try
            {
                string url = $"getDataRecipeComponets?id={Uri.EscapeDataString(Convert.ToString(id))}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка API: {response.StatusCode} RecipeComponets");
                    return null;
                }

                var result = JsonSerializer.Deserialize<RecipeComponets[]>(content);

                if (result == null)
                {
                    MessageBox.Show("Ошибка");
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return null;
            }
        }

        public async Task<RawMaterialsData[]?> GetDataDbRawMaterials()
        {
            try
            {
                string url = $"getDataRawMaterials";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка API: {response.StatusCode} RawMaterials");
                    return null;
                }

                var result = JsonSerializer.Deserialize<RawMaterialsData[]>(content);

                if (result == null)
                {
                    MessageBox.Show("Ошибка");
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return null;
            }
        }

        public async Task<RecipeStatusHistory[]?> GetRecipesComment(int entityId, string entityType)
        {
            try
            {
                string url = $"getDataRecipesComment?id={Uri.EscapeDataString(entityId.ToString())}&type={Uri.EscapeDataString(entityType)}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка API: {response.StatusCode}");
                    return null;
                }

                var result = JsonSerializer.Deserialize<RecipeStatusHistory[]>(content);

                if (result == null)
                {
                    MessageBox.Show("Ошибка");
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return null;
            }
        }




    }
}
