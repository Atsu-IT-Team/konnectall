using Nop.Services.Orders;
using Nop.Core.Domain.Orders;
using Iyzipay.Model;
using Nop.Core;
using Iyzipay.Request;
using Iyzipay;
using Nop.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Konnectall.Plugin.Payment.Services;
using Konnectall.Plugin.Payment.Factories;

namespace Konnectall.Plugin.Payment.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void Configure(IApplicationBuilder application)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIyzicoPaymentService, IyzicoPaymentService>();
            services.AddScoped<IIyzicoPaymentModelFactory, IyzicoPaymentModelFactory>();
        }
        public int Order => 0;
    }
}