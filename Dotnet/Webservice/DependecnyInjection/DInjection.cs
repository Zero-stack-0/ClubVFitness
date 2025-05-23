using Data.Repository;
using Data.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.Helper;
using System.Reflection;
using Service.Interface;
using Service;
namespace Webservice.DependecnyInjection
{


    public static class DInjection
    {
        public static void AddDependecyInjection(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<GymVDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddAutoMapper(typeof(Service.Helper.Mapper).Assembly);

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            //Services
            services.AddScoped<IUserService, UserService>();
        }
    }
}