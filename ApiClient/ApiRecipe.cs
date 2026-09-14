using MaterialDesignThemes.Wpf;
using PlantProtectionTechnologist.Models.Recipe;
using PlantProtectionTechnologist.Scipts;
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
    class ApiRecipe : ApiBase
    {
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

        public async Task<bool> CreateRecipe(CreateRecipe createRecipe)
        {
            try
            {
                string url = $"createRecipe";

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, createRecipe);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка API: {response.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return false;
            }
        }

        public async Task<bool> EditRecipe(int id, CreateRecipe createRecipe)
        {
            try
            {
                string url = $"editRecipe?id={Uri.EscapeDataString(id.ToString())}";

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, createRecipe);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка API: {response.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return false;
            }
        }

        public async Task<bool> EditStatusRecipe(int recipeId, string statusId)
        {
            try
            {
                string url = $"editStatusRecipe?id={Uri.EscapeDataString(recipeId.ToString())}&statusId={Uri.EscapeDataString(statusId.ToString())}&userId={Uri.EscapeDataString(ButtonManager.instance.user.id.ToString())}";

                HttpResponseMessage response = await _httpClient.PutAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка API: {response.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return false;
            }
        }
    }
}
