// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Models.SendPaymentResponse
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Payments.MyFatoorah.Models
{
  public class SendPaymentResponse
  {
    public bool IsSuccess { get; set; }

    public string Message { get; set; }

    public Validationerror[] ValidationErrors { get; set; }

    public Data Data { get; set; }

    public string MessageSummary
    {
      get
      {
        if (this.Message == "Unauthorized")
          this.Message += "-Invalid Access Code 103 !. Please contact MyFatoorah customer support to Activate your API Account.";
        return this.ValidationErrors == null ? this.Message : this.Message + " - " + string.Join(",", ((IEnumerable<Validationerror>) this.ValidationErrors).Select<Validationerror, string>((Func<Validationerror, string>) (x => x.Error)));
      }
    }
  }
}
