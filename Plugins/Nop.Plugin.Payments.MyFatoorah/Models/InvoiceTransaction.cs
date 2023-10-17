// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Models.InvoiceTransaction
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using System;

namespace Nop.Plugin.Payments.MyFatoorah.Models
{
  public class InvoiceTransaction
  {
    public DateTime TransactionDate { get; set; }

    public string PaymentGateway { get; set; }

    public string ReferenceId { get; set; }

    public string TrackId { get; set; }

    public string TransactionId { get; set; }

    public string PaymentId { get; set; }

    public string AuthorizationId { get; set; }

    public string TransactionStatus { get; set; }

    public Decimal TransationValue { get; set; }

    public string CustomerServiceCharge { get; set; }

    public Decimal DueValue { get; set; }

    public string PaidCurrency { get; set; }

    public Decimal PaidCurrencyValue { get; set; }

    public string Currency { get; set; }

    public string Error { get; set; }

    public string CardNumber { get; set; }
  }
}
