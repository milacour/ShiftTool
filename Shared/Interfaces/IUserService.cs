using ShiftTool.Shared.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public interface IUserService
{
    Task<HttpResponseMessage> RegisterUser(UserDTO userDto);
    Task<HttpResponseMessage> Login(UserDTO userDto);
    Task<HttpResponseMessage> Logout();
    Task<HttpResponseMessage> CreateUserAsync(UserDTO userDto);
    Task<List<UserDTO>> GetUsers();
    Task<UserDTO> GetCurrentUserAsync(string email);
    Task<HttpResponseMessage> UpdateUserAsync(UserDTO userDto);
    Task<HttpResponseMessage> DeleteUser(string email);
}
