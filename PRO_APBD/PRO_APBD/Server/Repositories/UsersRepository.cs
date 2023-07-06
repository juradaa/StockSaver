using Microsoft.EntityFrameworkCore;
using PRO_APBD.Server.Models;

namespace PRO_APBD.Server.Repositories;

public interface IUsersRepository
{
    Task<User?> GetUser(string username);
    Task AddUser(User user);
}
public class UsersRepository : IUsersRepository
{
    private readonly DataContext _context;

    public UsersRepository(DataContext context)
    {
        _context = context;
    }
    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUser(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
    }
}
