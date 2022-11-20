using Microsoft.EntityFrameworkCore;
using WebAPITemplate.API.Data;
using WebAPITemplate.API.Exceptions;
using WebAPITemplate.Shared.Models.Entities;

namespace WebAPITemplate.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public UserRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// Get all users from DB
    /// </summary>
    /// <returns>All users</returns>
    public async Task<IEnumerable<User>> GetAll()
    {
        await using AppDbContext context = await _contextFactory.CreateDbContextAsync();
        return await context.Users.ToListAsync();
    }

    /// <summary>
    /// Get user by user's ID
    /// </summary>
    /// <param name="userId">User's id</param>
    /// <returns>User</returns>
    /// <exception cref="NotFoundException">User not found.</exception>
    /// <exception cref="InvalidOperationException">More than one user was found.</exception>
    public async Task<User> GetById(Guid userId)
    {
        await using AppDbContext context = await _contextFactory.CreateDbContextAsync();
        User? user = await context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        return user ?? throw new NotFoundException($"User {userId} not found.");
    }

    /// <summary>
    /// Get user by user's name
    /// </summary>
    /// <param name="username">User's name</param>
    /// <returns>User</returns>
    /// <exception cref="ArgumentNullException">User's name was null</exception>
    /// <exception cref="NotFoundException">User not found.</exception>
    /// <exception cref="InvalidOperationException">More than one user was found.</exception>
    public async Task<User> GetByUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException(nameof(username));
        
        await using AppDbContext context = await _contextFactory.CreateDbContextAsync();
        User? user =  await context.Users.SingleOrDefaultAsync(u => u.Username == username);
        return user ?? throw new NotFoundException($"User {username} not found.");
    }

    /// <summary>
    /// Add user to DB
    /// </summary>
    /// <param name="user">New user</param>
    /// <returns>Created user</returns>
    /// <exception cref="ArgumentNullException">User was null.</exception>
    public async Task<User> Add(User user)
    {
        if(user is null)
            throw new ArgumentNullException(nameof(user));
        
        await Validate(user);
        
        // hash password
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        await using AppDbContext context = await _contextFactory.CreateDbContextAsync();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>Updated user</returns>
    /// <exception cref="ArgumentNullException">User was null</exception>
    public async Task<User> Update(User user)
    {
        if(user is null)
            throw new ArgumentNullException(nameof(user));
        
        await Validate(user);
        
        await using AppDbContext context = await _contextFactory.CreateDbContextAsync();
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }
    
    /// <summary>
    /// Delete user from DB
    /// </summary>
    /// <param name="userId">User's id</param>
    /// <returns>True - if user is deleted, False - otherwise</returns>
    /// <exception cref="NotFoundException">User not found.</exception>
    /// <exception cref="InvalidOperationException">More than one user was found.</exception>
    public async Task<bool> Delete(Guid userId)
    {
        await using AppDbContext context = await _contextFactory.CreateDbContextAsync();
        User user = await GetById(userId);
        context.Users.Remove(user);
        return await context.SaveChangesAsync() > 0;
    }

    private async Task Validate(User user)
    {
        await using AppDbContext context = await _contextFactory.CreateDbContextAsync();
        if (await context.Users.AnyAsync(x => x.Username == user.Username && x.Id != user.Id))
            throw new BadRequestException("Username '" + user.Username + "' is already taken");

        if (await context.Users.AnyAsync(x => x.Email == user.Email && x.Id != user.Id))
            throw new BadRequestException("Email '" + user.Email + "' is already taken");
    }
}