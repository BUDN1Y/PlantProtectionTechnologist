using PlantProtectionTechnologist.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PlantProtectionTechnologist.ApiClient
{
    class ApiProduct : ApiBase
    {
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

    }
}
