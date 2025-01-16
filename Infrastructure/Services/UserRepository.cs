using System.Linq.Expressions;
using Application.Abstraction;
using Application.Extensions;
using Application.Repositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    internal class UserRepository : IUserRepository
    {
        private readonly IRetailingOrderingSystemDbContext context;

        public UserRepository(IRetailingOrderingSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            user.Password = user.Password.GetHash();
            await this.context.Users.AddAsync(user);
            int result = await this.context.SaveChangesAsync();

            if (result > 0) return user;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            User user = await this.context.Users.FindAsync(id);

            if (user is not null)
            {
                this.context.Users.Remove(user);
            }

            int result = await this.context.SaveChangesAsync();

            if (result > 0) return true;

            return false;
        }

        public async Task<IQueryable<User>> GetAllAsync(Expression<Func<User, bool>> expression)
        {
            return this.context.Users.Where(expression)
                .Include(x => x.Orders);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await this.context.Users
                .Where(x => x.Id == id)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await GetByIdAsync(user.Id);

            if (existingUser is not null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;

                if (Enum.IsDefined(typeof(Role), user.Role))
                {
                    existingUser.Role = user.Role;
                }
                else
                {
                    throw new ArgumentException("Invalid role provided.");
                }

                await this.context.SaveChangesAsync();
                return existingUser;
            }

            return null;
        }


        public async Task UpdatePasswordAsync(User user)
        {
            var existingUser = await GetByIdAsync(user.Id);

            if (existingUser is not null)
            {
                existingUser.Password = user.Password;
                await this.context.SaveChangesAsync();
            }
        }
    }
}

