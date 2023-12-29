using ShiftTool.Shared.DTOs;

namespace ShiftTool.Client.Services
{

    public class UserState
    {
        public UserDTO CurrentUser { get; private set; }

        public bool IsLoggedIn => CurrentUser != null;

        public event Action OnUserStateChanged;

        public void Login(UserDTO UserData)
        {
            CurrentUser = UserData;
            OnUserStateChanged?.Invoke();
        }

        public void UpdateUser(UserDTO updatedUser)
        {
            if (CurrentUser != null && CurrentUser.Email == updatedUser.Email)
            {
                CurrentUser = updatedUser;
                OnUserStateChanged?.Invoke();
            }
        }


        public void Logout()
        {
            CurrentUser = null;
            OnUserStateChanged?.Invoke();
        }
    }
}