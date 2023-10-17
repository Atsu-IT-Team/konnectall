// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Infrastructure.PluginNopStartup
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Payments.MyFatoorah.Services;
using Nop.Web.Framework.Infrastructure.Extensions;
using System;
namespace Nop.Plugin.Payments.MyFatoorah.Infrastructure
{
    public class PluginNopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            Web.Framework.Infrastructure.Extensions.HttpClientBuilderExtensions.WithProxy(services.AddHttpClient<MyFatoorahHttpClient>());
            services.Configure<RazorViewEngineOptions>((Action<RazorViewEngineOptions>)(options => options.ViewLocationExpanders.Add((IViewLocationExpander)new ViewLocationExpander())));
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => 11;
    }
}
