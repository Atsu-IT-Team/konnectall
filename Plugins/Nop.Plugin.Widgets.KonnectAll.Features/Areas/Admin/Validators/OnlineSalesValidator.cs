using FluentValidation;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.OnlineSales;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Validators
{
    public class OnlineSalesValidator : BaseNopValidator<OnlineSalesModel>
    {
        public OnlineSalesValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.OnlineSales.Title.Required"));
        }
    }
}
