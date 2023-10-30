using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess;

public class UserEfcDao : IUserDao
{
    private readonly TodoContext context;

    public UserEfcDao(TodoContext context)
    {
        this.context = context;
    }
    public async Task<User?> GetByUsernameAsync(string username)
    { 
        return await context.Users.FirstOrDefaultAsync(u => 
            u.Username.ToLower().Equals(username.ToLower()));
    }

    public async Task<User> CreateAsync(User user)
    {
        EntityEntry<User> newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return newUser.Entity;
    }

    public async Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        IQueryable<User> users = context.Users.AsQueryable(); 
        
        if(searchParameters.UsernameContains != null)
            users = context.Users
                .Where(u => u.Username.ToLower().Contains(searchParameters.UsernameContains!.ToLower()));

        IEnumerable<User> result = await users.ToListAsync();
        return result;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        User? existing = await context.Users.FindAsync(id);
        if (existing == null)
            throw new Exception($"User with id {id} is not found.");
        return existing;
    }
}