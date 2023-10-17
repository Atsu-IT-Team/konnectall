// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Components.PaymentMyFatoorahViewComponent
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.MyFatoorah.Components
{
  [ViewComponent(Name = "PaymentMyFatoorah")]
  public class PaymentMyFatoorahViewComponent : NopViewComponent
  {
    public IViewComponentResult Invoke() => (IViewComponentResult) this.View("~/Plugins/Payments.MyFatoorah/Views/PaymentInfo.cshtml");
  }
}
