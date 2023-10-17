// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Models.ConfigurationModel
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;


#nullable enable
namespace Nop.Plugin.Payments.MyFatoorah.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public ConfigurationModel()
        {
            this.PaymentModes = (IList<SelectListItem>)new List<SelectListItem>()
      {
        new SelectListItem() { Text = "BOTH", Value = "BOTH" },
        new SelectListItem() { Text = "VISA", Value = "VISA" },
        new SelectListItem() { Text = "KNET", Value = "KNET" }
      };
            this.DisplayCurrenciesIsoAlpha = (IList<SelectListItem>)new List<SelectListItem>()
      {
        new SelectListItem() { Text = "Kuwait (KD)", Value = "KWD" },
        new SelectListItem()
        {
          Text = "Saudi Arabia (SR)",
          Value = "SAR"
        },
        new SelectListItem()
        {
          Text = "Bahrain (BD)",
          Value = "BHD"
        },
        new SelectListItem() { Text = "UAE (AED)", Value = "AED" },
        new SelectListItem() { Text = "Qatar (QR)", Value = "QAR" },
        new SelectListItem() { Text = "Oman (OR)", Value = "OMR" },
        new SelectListItem() { Text = "Jordan (JD)", Value = "JOD" },
        new SelectListItem()
        {
          Text = "United States (US)",
          Value = "USD"
        }
      };
            this.CountryCodes = (IList<SelectListItem>)new List<SelectListItem>()
      {
        new SelectListItem() { Text = "Kuwait", Value = "kw" },
        new SelectListItem() { Text = "Saudi Arabia", Value = "sa" },
        new SelectListItem() { Text = "Bahrain", Value = "bh" },
        new SelectListItem() { Text = "UAE", Value = "ae" },
        new SelectListItem() { Text = "Qatar", Value = "qa" }
      };
            this.PaymentAcknowledgements = (IList<SelectListItem>)new List<SelectListItem>()
      {
        new SelectListItem() { Text = "NILL", Value = "4" },
        new SelectListItem() { Text = "SMS", Value = "1" },
        new SelectListItem() { Text = "Email", Value = "2" },
        new SelectListItem()
        {
          Text = "Both SMS and Email",
          Value = "3"
        }
      };
            this.Countries = (IList<SelectListItem>)new List<SelectListItem>()
      {
        new SelectListItem() { Text = "Kuwait", Value = "KWT" },
        new SelectListItem()
        {
          Text = "Saudi Arabia",
          Value = "SAU"
        },
        new SelectListItem() { Text = "Bahrain", Value = "BHR" },
        new SelectListItem() { Text = "UAE", Value = "ARE" },
        new SelectListItem() { Text = "Qatar", Value = "QAT" },
        new SelectListItem() { Text = "Oman", Value = "OMN" },
        new SelectListItem() { Text = "Jordan", Value = "JOD" },
        new SelectListItem() { Text = "Egypt", Value = "EGY" }
      };
        }

        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }

        public bool UseSandbox_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.APIToken")]
        [Required]
        public
#nullable disable
        string APIToken
        { get; set; }

        public bool APIToken_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.PaymentMode")]
        public string PaymentMode { get; set; }

        public bool PaymentMode_OverrideForStore { get; set; }

        public IList<SelectListItem> PaymentModes { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.ReturnURL")]
        public string ReturnURL { get; set; }

        public bool ReturnURL_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.ErrorURL")]
        public string ErrorURL { get; set; }

        public bool ErrorURL_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.CountryCode")]
        public string CountryCode { get; set; }

        public bool CountryCode_OverrideForStore { get; set; }

        public IList<SelectListItem> CountryCodes { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.DisplayCurrencyIsoAlpha")]
        public string DisplayCurrencyIsoAlpha { get; set; }

        public bool DisplayCurrencyIsoAlpha_OverrideForStore { get; set; }

        public IList<SelectListItem> DisplayCurrenciesIsoAlpha { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.AdditionalFee")]
        public Decimal AdditionalFee { get; set; }

        public bool AdditionalFee_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }

        public bool AdditionalFeePercentage_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage")]
        public bool ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage { get; set; }

        public bool ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.PaymentAcknowledgement")]
        public int? PaymentAcknowledgement { get; set; }

        public IList<SelectListItem> PaymentAcknowledgements { get; set; }

        public bool PaymentAcknowledgement_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.MyFatoorah.Fields.Country")]
        public string Country { get; set; }

        public IList<SelectListItem> Countries { get; set; }

        public bool Country_OverrideForStore { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(nameof(ConfigurationModel));
            stringBuilder.Append(" { ");
            if (base.PrintMembers(stringBuilder))
                stringBuilder.Append(' ');
            stringBuilder.Append('}');
            return stringBuilder.ToString();
        }


        protected override bool PrintMembers(StringBuilder builder)
        {
            if (base.PrintMembers(builder))
                builder.Append(", ");
            builder.Append("ActiveStoreScopeConfiguration = ");
            builder.Append(this.ActiveStoreScopeConfiguration.ToString());
            builder.Append(", UseSandbox = ");
            builder.Append(this.UseSandbox.ToString());
            builder.Append(", UseSandbox_OverrideForStore = ");
            builder.Append(this.UseSandbox_OverrideForStore.ToString());
            builder.Append(", APIToken = ");
            builder.Append((object)this.APIToken);
            builder.Append(", APIToken_OverrideForStore = ");
            builder.Append(this.APIToken_OverrideForStore.ToString());
            builder.Append(", PaymentMode = ");
            builder.Append((object)this.PaymentMode);
            builder.Append(", PaymentMode_OverrideForStore = ");
            builder.Append(this.PaymentMode_OverrideForStore.ToString());
            builder.Append(", PaymentModes = ");
            builder.Append((object)this.PaymentModes);
            builder.Append(", ReturnURL = ");
            builder.Append((object)this.ReturnURL);
            builder.Append(", ReturnURL_OverrideForStore = ");
            builder.Append(this.ReturnURL_OverrideForStore.ToString());
            builder.Append(", ErrorURL = ");
            builder.Append((object)this.ErrorURL);
            builder.Append(", ErrorURL_OverrideForStore = ");
            builder.Append(this.ErrorURL_OverrideForStore.ToString());
            builder.Append(", CountryCode = ");
            builder.Append((object)this.CountryCode);
            builder.Append(", CountryCode_OverrideForStore = ");
            builder.Append(this.CountryCode_OverrideForStore.ToString());
            builder.Append(", CountryCodes = ");
            builder.Append((object)this.CountryCodes);
            builder.Append(", DisplayCurrencyIsoAlpha = ");
            builder.Append((object)this.DisplayCurrencyIsoAlpha);
            builder.Append(", DisplayCurrencyIsoAlpha_OverrideForStore = ");
            builder.Append(this.DisplayCurrencyIsoAlpha_OverrideForStore.ToString());
            builder.Append(", DisplayCurrenciesIsoAlpha = ");
            builder.Append((object)this.DisplayCurrenciesIsoAlpha);
            builder.Append(", AdditionalFee = ");
            builder.Append(this.AdditionalFee.ToString());
            builder.Append(", AdditionalFee_OverrideForStore = ");
            builder.Append(this.AdditionalFee_OverrideForStore.ToString());
            builder.Append(", AdditionalFeePercentage = ");
            builder.Append(this.AdditionalFeePercentage.ToString());
            builder.Append(", AdditionalFeePercentage_OverrideForStore = ");
            builder.Append(this.AdditionalFeePercentage_OverrideForStore.ToString());
            builder.Append(", ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage = ");
            builder.Append(this.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage.ToString());
            builder.Append(", ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore = ");
            builder.Append(this.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore.ToString());
            builder.Append(", PaymentAcknowledgement = ");
            builder.Append(this.PaymentAcknowledgement.ToString());
            builder.Append(", PaymentAcknowledgements = ");
            builder.Append((object)this.PaymentAcknowledgements);
            builder.Append(", PaymentAcknowledgement_OverrideForStore = ");
            builder.Append(this.PaymentAcknowledgement_OverrideForStore.ToString());
            builder.Append(", Country = ");
            builder.Append((object)this.Country);
            builder.Append(", Countries = ");
            builder.Append((object)this.Countries);
            builder.Append(", Country_OverrideForStore = ");
            builder.Append(this.Country_OverrideForStore.ToString());
            return true;
        }


        public override int GetHashCode() => (((((((((((((((((((((((((((((base.GetHashCode() * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.ActiveStoreScopeConfiguration)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.UseSandbox)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.UseSandbox_OverrideForStore)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.APIToken)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.APIToken_OverrideForStore)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.PaymentMode)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.PaymentMode_OverrideForStore)) * -1521134295 + EqualityComparer<IList<SelectListItem>>.Default.GetHashCode(this.PaymentModes)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.ReturnURL)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.ReturnURL_OverrideForStore)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.ErrorURL)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.ErrorURL_OverrideForStore)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.CountryCode)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.CountryCode_OverrideForStore)) * -1521134295 + EqualityComparer<IList<SelectListItem>>.Default.GetHashCode(this.CountryCodes)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.DisplayCurrencyIsoAlpha)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.DisplayCurrencyIsoAlpha_OverrideForStore)) * -1521134295 + EqualityComparer<IList<SelectListItem>>.Default.GetHashCode(this.DisplayCurrenciesIsoAlpha)) * -1521134295 + EqualityComparer<Decimal>.Default.GetHashCode(this.AdditionalFee)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.AdditionalFee_OverrideForStore)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.AdditionalFeePercentage)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.AdditionalFeePercentage_OverrideForStore)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore)) * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(this.PaymentAcknowledgement)) * -1521134295 + EqualityComparer<IList<SelectListItem>>.Default.GetHashCode(this.PaymentAcknowledgements)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.PaymentAcknowledgement_OverrideForStore)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Country)) * -1521134295 + EqualityComparer<IList<SelectListItem>>.Default.GetHashCode(this.Countries)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.Country_OverrideForStore);


        //public override bool Equals(BaseNopModel? other) => ((object) this).Equals((object) other);


        public virtual bool Equals(ConfigurationModel? other)
        {
            if ((object)this == (object)other)
                return true;
            return base.Equals((BaseNopModel)other);
            //&& EqualityComparer<int>.Default.Equals(this, other) && EqualityComparer<bool>.Default.Equals(this.UseSandbox, other.UseSandbox) && EqualityComparer<bool>.Default.Equals(this.UseSandbox_OverrideForStore, other.UseSandbox_OverrideForStore) && EqualityComparer<string>.Default.Equals(this.APIToken, other.APIToken) && EqualityComparer<bool>.Default.Equals(this.APIToken_OverrideForStore, other.APIToken_OverrideForStore) && EqualityComparer<string>.Default.Equals(this.PaymentMode, other.PaymentMode) && EqualityComparer<bool>.Default.Equals(this.PaymentMode_OverrideForStore, other.PaymentMode_OverrideForStore) && EqualityComparer<IList<SelectListItem>>.Default.Equals(this.PaymentModes, other.PaymentModes) && EqualityComparer<string>.Default.Equals(this.ReturnURL, other.ReturnURL) && EqualityComparer<bool>.Default.Equals(this.ReturnURL_OverrideForStore, other.ReturnURL_OverrideForStore) && EqualityComparer<string>.Default.Equals(this.ErrorURL, other.ErrorURL) && EqualityComparer<bool>.Default.Equals(this.ErrorURL_OverrideForStore, other.ErrorURL_OverrideForStore) && EqualityComparer<string>.Default.Equals(this.CountryCode, other.CountryCode) && EqualityComparer<bool>.Default.Equals(this.CountryCode_OverrideForStore, other.CountryCode_OverrideForStore) && EqualityComparer<IList<SelectListItem>>.Default.Equals(this.CountryCodes, other.CountryCodes) && EqualityComparer<string>.Default.Equals(this.DisplayCurrencyIsoAlpha, other.DisplayCurrencyIsoAlpha) && EqualityComparer<bool>.Default.Equals(this.DisplayCurrencyIsoAlpha_OverrideForStore, other.DisplayCurrencyIsoAlpha_OverrideForStore) && EqualityComparer<IList<SelectListItem>>.Default.Equals(this.DisplayCurrenciesIsoAlpha, other.DisplayCurrenciesIsoAlpha) && EqualityComparer<Decimal>.Default.Equals(this.AdditionalFee, other.AdditionalFee) && EqualityComparer<bool>.Default.Equals(this.AdditionalFee_OverrideForStore, other.AdditionalFee_OverrideForStore) && EqualityComparer<bool>.Default.Equals(this.AdditionalFeePercentage, other.AdditionalFeePercentage) && EqualityComparer<bool>.Default.Equals(this.AdditionalFeePercentage_OverrideForStore, other.AdditionalFeePercentage_OverrideForStore) && EqualityComparer<bool>.Default.Equals(this.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage, other.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage) && EqualityComparer<bool>.Default.Equals(this.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore, other.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore) && EqualityComparer<int?>.Default.Equals(this.PaymentAcknowledgement, other.PaymentAcknowledgement) && EqualityComparer<IList<SelectListItem>>.Default.Equals(this.PaymentAcknowledgements, other.PaymentAcknowledgements) && EqualityComparer<bool>.Default.Equals(this.PaymentAcknowledgement_OverrideForStore, other.PaymentAcknowledgement_OverrideForStore) && EqualityComparer<string>.Default.Equals(this.Country, other.Country) && EqualityComparer<IList<SelectListItem>>.Default.Equals(this.Countries, other.Countries) && EqualityComparer<bool>.Default.Equals(this.Country_OverrideForStore, other.Country_OverrideForStore);
        }


        protected ConfigurationModel(ConfigurationModel original)
          : base((BaseNopModel)original)
        {


            this.ActiveStoreScopeConfiguration = original.ActiveStoreScopeConfiguration;


            this.UseSandbox = original.UseSandbox;


            this.UseSandbox_OverrideForStore = original.UseSandbox_OverrideForStore;


            this.APIToken = original.APIToken;


            this.APIToken_OverrideForStore = original.APIToken_OverrideForStore;


            this.PaymentMode = original.PaymentMode;


            this.PaymentMode_OverrideForStore = original.PaymentMode_OverrideForStore;


            this.PaymentModes = original.PaymentModes;


            this.ReturnURL = original.ReturnURL;


            this.ReturnURL_OverrideForStore = original.ReturnURL_OverrideForStore;


            this.ErrorURL = original.ErrorURL;


            this.ErrorURL_OverrideForStore = original.ErrorURL_OverrideForStore;


            this.CountryCode = original.CountryCode;


            this.CountryCode_OverrideForStore = original.CountryCode_OverrideForStore;


            this.CountryCodes = original.CountryCodes;


            this.DisplayCurrencyIsoAlpha = original.DisplayCurrencyIsoAlpha;


            this.DisplayCurrencyIsoAlpha_OverrideForStore = original.DisplayCurrencyIsoAlpha_OverrideForStore;


            this.DisplayCurrenciesIsoAlpha = original.DisplayCurrenciesIsoAlpha;


            this.AdditionalFee = original.AdditionalFee;


            this.AdditionalFee_OverrideForStore = original.AdditionalFee_OverrideForStore;


            this.AdditionalFeePercentage = original.AdditionalFeePercentage;


            this.AdditionalFeePercentage_OverrideForStore = original.AdditionalFeePercentage_OverrideForStore;


            this.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage = original.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage;


            this.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore = original.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore;


            this.PaymentAcknowledgement = original.PaymentAcknowledgement;


            this.PaymentAcknowledgements = original.PaymentAcknowledgements;


            this.PaymentAcknowledgement_OverrideForStore = original.PaymentAcknowledgement_OverrideForStore;


            this.Country = original.Country;


            this.Countries = original.Countries;


            this.Country_OverrideForStore = original.Country_OverrideForStore;
        }
    }
}
