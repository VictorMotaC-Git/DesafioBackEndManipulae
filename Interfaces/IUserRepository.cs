using System.Threading.Tasks;
using DesafioBackEndManipulae.Models;

public interface IUserRepository
{
    Task<User> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}
