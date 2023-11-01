using FluentValidation;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.ApplicationRequest;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Validators
{
    public class ApplicationRequestValidator : BaseNopValidator<ApplicationRequestModel>
    {
        public ApplicationRequestValidator(ILocalizationService localizationService) 
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.FirstName.Required"));
            RuleFor(x => x.LastName).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.LastName.Required"));
            RuleFor(x => x.Email).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.Email.Required"));
            RuleFor(x => x.Phone).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.Phone.Required"));
        }
    }
}
