using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interface;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly GymVDbContext context;
        public UserRepository(GymVDbContext context)
        {
            this.context = context;
        }

        public Task<Users?> Create(Users user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return Task.FromResult(user);
        }

        public async Task<Users?> GetByEmail(string email)
        {
            return await context.Users.Include(it => it.Role).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Users?> GetByEmailOrPhoneAndPassword(string emailOrPhone, string password)
        {
            return await context.Users.Include(it => it.Role).FirstOrDefaultAsync(x => (x.Email == emailOrPhone || x.PhoneNumber == emailOrPhone) && x.Password == password);
        }
    }
}