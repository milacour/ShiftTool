using ShiftTool.Shared.Models;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Server.Data;
using Dapper;

namespace ShiftTool.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

        public UserRepository(INpgsqlConnectionFactory npgsqlConnectionFactory)
        {
            _npgsqlConnectionFactory = npgsqlConnectionFactory ?? throw new ArgumentNullException(nameof(npgsqlConnectionFactory));
        }

        // Hent alle brugere fra databasen
        public async Task<List<User>> GetUsersAsync()
        {
            const string query = "SELECT * FROM Users";
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            return (await connection.QueryAsync<User>(query)).AsList();
        }

        // Hent den nuværende bruger baseret på email
        public async Task<User> GetCurrentUserAsync(string email)
        {
            const string query = "SELECT * FROM Users WHERE Email = @Email";
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        // Opret en ny bruger i databasen
        public async Task CreateUserAsync(User user)
        {
            const string query = @"
                INSERT INTO Users (Email, Password, FullName, PhoneNumber, IsCoordinator, 
                Experience, Skills, CreatedAt)
                VALUES (@Email, @Password, @FullName, @PhoneNumber, @IsCoordinator, 
                @Experience, @Skills, @CreatedAt)
            ";
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            await connection.ExecuteAsync(query, user);
        }

        // Opdater en eksisterende bruger i databasen baseret på email
        public async Task UpdateUserAsync(User user)
        {
            const string query = @"
                UPDATE Users SET Email = @Email, Password = @Password, FullName = @FullName, 
                PhoneNumber = @PhoneNumber, IsCoordinator = @IsCoordinator, 
                Experience = @Experience, Skills = @Skills WHERE Email = @Email
            ";
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            await connection.ExecuteAsync(query, user);
        }

        // Slet en bruger fra databasen baseret på email
        public async Task DeleteUserAsync(string email)
        {
            const string query = "DELETE FROM Users WHERE Email = @Email";
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            await connection.ExecuteAsync(query, new { Email = email });
        }
    }
}
