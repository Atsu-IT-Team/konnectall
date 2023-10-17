// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Infrastructure.ViewLocationExpander
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Nop.Plugin.Payments.MyFatoorah.Infrastructure
{
  public class ViewLocationExpander : IViewLocationExpander
  {
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }

    public IEnumerable<string> ExpandViewLocations(
      ViewLocationExpanderContext context,
      IEnumerable<string> viewLocations)
    {
      if (context.AreaName == "Admin")
      {
        string[] first = new string[1];
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
        interpolatedStringHandler.AppendLiteral("/Plugins/Nop.Plugin.Payments.MyFatoorah/Areas/Admin/Views/");
        interpolatedStringHandler.AppendFormatted(context.ControllerName);
        interpolatedStringHandler.AppendLiteral("/");
        interpolatedStringHandler.AppendFormatted(context.ViewName);
        interpolatedStringHandler.AppendLiteral(".cshtml");
        first[0] = interpolatedStringHandler.ToStringAndClear();
        viewLocations = ((IEnumerable<string>) first).Concat<string>(viewLocations);
      }
      else
      {
        string[] first = new string[1];
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
        interpolatedStringHandler.AppendLiteral("/Plugins/Nop.Plugin.Payments.MyFatoorah/Views/");
        interpolatedStringHandler.AppendFormatted(context.ControllerName);
        interpolatedStringHandler.AppendLiteral("/");
        interpolatedStringHandler.AppendFormatted(context.ViewName);
        interpolatedStringHandler.AppendLiteral(".cshtml");
        first[0] = interpolatedStringHandler.ToStringAndClear();
        viewLocations = ((IEnumerable<string>) first).Concat<string>(viewLocations);
      }
      return viewLocations;
    }
  }
}
