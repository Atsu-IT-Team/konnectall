using System;
using FluentValidation;
using Konnectall.Plugin.Payment.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
namespace Konnectall.Plugin.Payment.Validators
{
    public class IyzicoPublicInfoModelValidator : BaseNopValidator<PaymentInfoModel>
    {
        public IyzicoPublicInfoModelValidator(ILocalizationService localizationService)
        {
            //set validation rules
            RuleFor(x => x.CardholderName)
                   .NotEmpty()
                   .WithMessageAwait(localizationService.GetResourceAsync("Nop.Plugin.Payments.Iyzico.PublicInfo.Fields.CardholderName.Required"));
            RuleFor(x => x.CardNumber)
                    .NotEmpty()
                    .WithMessageAwait(localizationService.GetResourceAsync("Nop.Plugin.Payments.Iyzico.PublicInfo.Fields.CardNumber.Required"))
                    .CreditCard()
                    .WithMessageAwait(localizationService.GetResourceAsync("Nop.Plugin.Payments.Iyzico.PublicInfo.Fields.CardNumber.Error"));
            RuleFor(x => x.CardCode)
                    .NotEmpty()
                    .WithMessageAwait(localizationService.GetResourceAsync("Nop.Plugin.Payments.Iyzico.PublicInfo.Fields.CardCode.Required"))
                    .MinimumLength(3)
                     .WithMessageAwait(localizationService.GetResourceAsync("Nop.Plugin.Payments.Iyzico.PublicInfo.Fields.CardCode.MinimumLength"))
                    .Matches(@"^[0-9]{3,4}$")
                    .WithMessageAwait(localizationService.GetResourceAsync("Nop.Plugin.Payments.Iyzico.PublicInfo.Fields.CardCode.Required"));
            RuleFor(x => x.ExpireMonth)
                    .NotEmpty()
                    .WithMessageAwait(localizationService.GetResourceAsync("Nop.Plugin.Payments.Iyzico.PublicInfo.Fields.ExpireMonth.Required"));
            RuleFor(x => x.ExpireYear)
                    .NotEmpty()
                    .WithMessageAwait(localizationService.GetResourceAsync("Nop.Plugin.Payments.Iyzico.PublicInfo.Fields.ExpireYear.Required"));
            RuleFor(x => x.ExpireMonth).Must((x, context) =>
            {
                //not specified yet
                if (string.IsNullOrEmpty(x.ExpireYear) || string.IsNullOrEmpty(x.ExpireMonth))
                    return true;

                //the cards remain valid until the last calendar day of that month
                //If, for example, an expiration date reads 06/15, this means it can be used until midnight on June 30, 2015
                var enteredDate = new DateTime(int.Parse(x.ExpireYear), int.Parse(x.ExpireMonth), 1).AddMonths(1);

                if (enteredDate < DateTime.Now)
                    return false;

                return true;
            }).WithMessageAwait(localizationService.GetResourceAsync("Payment.ExpirationDate.Expired"));

            //TODO....
            //RuleFor(x => x.Installment).Matches(@"^[0-9]*$").WithMessageAwait(localizationService.GetResourceAsync("Plugins.Payments.Iyzico.Installment.Wrong"));
        }
    }
}
