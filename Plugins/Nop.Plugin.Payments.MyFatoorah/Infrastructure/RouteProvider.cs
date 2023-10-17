// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.RouteProvider
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.MyFatoorah
{
  public class RouteProvider : IRouteProvider
  {
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
      endpointRouteBuilder.MapControllerRoute("Plugin.Payments.MyFatoorah.PDTHandler", "Plugins/PaymentMyFatoorah/PDTHandler", (object) new
      {
        controller = "PaymentMyFatoorah",
        action = "PDTHandler"
      });
      endpointRouteBuilder.MapControllerRoute("Plugin.Payments.MyFatoorah.CancelOrder", "Plugins/PaymentMyFatoorah/CancelOrder", (object) new
      {
        controller = "PaymentMyFatoorah",
        action = "CancelOrder"
      });
    }

    public int Priority => -1;
  }
}
