// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Models.SendPaymentRequest
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using System;
using System.Collections.Generic;

namespace Nop.Plugin.Payments.MyFatoorah.Models
{
  public class SendPaymentRequest
  {
    public string CustomerName { get; set; }

    public string NotificationOption { get; set; }

    public string MobileCountryCode { get; set; }

    public string CustomerMobile { get; set; }

    public string CustomerEmail { get; set; }

    public Decimal InvoiceValue { get; set; }

    public string DisplayCurrencyIso { get; set; }

    public string CallBackUrl { get; set; }

    public string ErrorUrl { get; set; }

    public string Language { get; set; }

    public string CustomerReference { get; set; }

    public string CustomerCivilId { get; set; }

    public string UserDefinedField { get; set; }

    public Customeraddress CustomerAddress { get; set; }

    public DateTime ExpiryDate { get; set; }

    public int SupplierCode { get; set; }

    public IList<Invoiceitem> InvoiceItems { get; set; }
  }
}
