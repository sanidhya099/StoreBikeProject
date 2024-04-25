using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesAndReturnSystem.BLL;
using SalesAndReturnSystem.DAL;

namespace SalesAndReturnSystem
{
    public static  class SalesAndReturnExtension
    {
        public static void AddBackendDependencies(this IServiceCollection services,
           Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<SalesAndReturnContext>(options);


            services.AddTransient<SalesService>((ServiceProvider) =>
            {
                var context = ServiceProvider.GetService<SalesAndReturnContext>();
                return new SalesService(context);
            });
        }
    }
}
