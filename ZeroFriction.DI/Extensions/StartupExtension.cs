using ZeroFriction.Repository.IRepository;
using ZeroFriction.Repository.Repository;
using ZeroFriction.Services.IServices;
using ZeroFriction.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.DI.Extensions
{
    public static class StartupExtension
    {
        //Create extension method for DI registration

        //Services DI invoice
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IInvoiceServices, InvoiceServices>();
            return services;
        }


        //Repository DI invoice
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            return services;
        }
    }
}
