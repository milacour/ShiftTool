using System;
using ShiftTool.Shared.DTOs;
using ShiftTool.Client.Services;

namespace ShiftTool.Client.Helpers
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserDTO UserData { get; set; }
    }

}

