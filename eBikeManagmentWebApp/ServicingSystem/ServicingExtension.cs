using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServicingSystem.BLL;
using ServicingSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicingSystem
{
    public static  class ServicingExtension
    {
       
            public static void ServicingDependencies(this IServiceCollection services,
               Action<DbContextOptionsBuilder> options)
            {
                services.AddDbContext<ServicingDataContext>(options);


                services.AddTransient<ServicingService>((ServiceProvider) =>
                {
                    var context = ServiceProvider.GetService<ServicingDataContext>();
                    return new ServicingService(context);
                });
            }
        
    }
}
