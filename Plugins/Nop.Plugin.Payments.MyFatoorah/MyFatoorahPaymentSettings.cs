// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.MyFatoorahPaymentSettings
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using Nop.Core.Configuration;
using System;

namespace Nop.Plugin.Payments.MyFatoorah
{
  public class MyFatoorahPaymentSettings : ISettings
  {
    public bool UseSandbox { get; set; }

    public Decimal AdditionalFee { get; set; }

    public bool AdditionalFeePercentage { get; set; }

    public string DisplayCurrencyIsoAlpha { get; set; }

    public int? SendInvoiceOption { get; set; }

    public int? Language { get; set; }

    public bool ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage { get; set; }

    public string APIToken { get; set; }

    public string Country { get; set; }
  }
}
