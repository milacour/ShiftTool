using Blazored.LocalStorage;
using ShiftTool.Shared.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShiftTool.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<HttpResponseMessage> RegisterUser(UserDTO userDto)
        {
            return await _httpClient.PostAsJsonAsync("api/user/register", userDto);
        }

        public async Task<HttpResponseMessage> Login(UserDTO userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/login", userDto);
            if (response.IsSuccessStatusCode)
            {
                // Læs brugerdata fra responsens indhold
                var userData = await response.Content.ReadFromJsonAsync<UserDTO>();

                if (userData != null)
                {
                    // Gem brugerdata i lokal lagring
                    await _localStorage.SetItemAsync("user", userData);
                }
            }
            return response;
        }


        public async Task<HttpResponseMessage> Logout()
        {
            return await _httpClient.PostAsync("api/user/logout", null);
        }


        public async Task<HttpResponseMessage> CreateUserAsync(UserDTO userDto)
        {
            return await _httpClient.PostAsJsonAsync("api/user/create-user", userDto);
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDTO>>("api/user/get-users");
        }

        public async Task<UserDTO> GetCurrentUserAsync(string email)
        {
            return await _httpClient.GetFromJsonAsync<UserDTO>($"api/user/get-user/{email}");
        }

        public async Task<HttpResponseMessage> UpdateUserAsync(UserDTO userDto)
        {
            return await _httpClient.PutAsJsonAsync("api/user/update-user", userDto);
        }

        public async Task<HttpResponseMessage> DeleteUser(string email)
        {
            return await _httpClient.DeleteAsync($"api/user/delete-user/{email}");
        }
    }
}