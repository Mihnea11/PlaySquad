using Server.Models;
using System.Data;
namespace Server.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IDbConnection connection) : base(connection) { }

        public async Task<int> AddUserAsync(User user)
        {
            var query = "INSERT INTO \"Users\" (name, email, password, age, city) VALUES (@Name, @Email, @Password, @Age, @City)";
            return await AddAsync(query, user);
        }
    }

}
