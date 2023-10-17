// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Models.PaymentData
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using System;
using System.Collections.Generic;

namespace Nop.Plugin.Payments.MyFatoorah.Models
{
  public class PaymentData
  {
    public long InvoiceId { get; set; }

    public string InvoiceStatus { get; set; }

    public string InvoiceReference { get; set; }

    public string CustomerReference { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ExpiryDate { get; set; }

    public Decimal InvoiceValue { get; set; }

    public string Comments { get; set; }

    public string CustomerName { get; set; }

    public string CustomerMobile { get; set; }

    public string CustomerEmail { get; set; }

    public string UserDefinedField { get; set; }

    public string InvoiceDisplayValue { get; set; }

    public IList<Invoiceitem> InvoiceItems { get; set; }

    public IList<InvoiceTransaction> InvoiceTransactions { get; set; }
  }
}
