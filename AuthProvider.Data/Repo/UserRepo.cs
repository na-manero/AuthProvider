using AuthProvider.Data.Context;
using AuthProvider.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AuthProvider.Data.Repo;

public class UserRepo(DataContext context)
{
    private readonly DataContext _context = context;

    // Create User
    public async Task CreateUser(UserEntity user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    // Read User by ID
    public async Task<UserEntity> GetUserByIdAsync(string userId)
    {
        try
        {
            return await _context.Users.FindAsync(userId) ?? null!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return null!;
    }

    // Read All Users
    public async Task<UserEntity> GetUserByEmailAsync(string email)
    {
        try 
        { 
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email) ?? null!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return null!;
    }

    // Update User
    public async Task UpdateUser(UserEntity user)
    {
        try
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    // Delete User
    public async Task DeleteUser(string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

}
