using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopBook;
using System.Collections.Generic;
using System.Text;

namespace Store.Data.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services, string connetionString)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(connetionString);

            },
                ServiceLifetime.Transient);
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            return services;
        }

    }
}
