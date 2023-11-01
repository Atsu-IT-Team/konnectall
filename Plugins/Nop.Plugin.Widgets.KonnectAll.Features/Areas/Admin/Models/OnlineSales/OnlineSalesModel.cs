using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.OnlineSales
{
    /// <summary>
    /// Represents a configuration model
    /// </summary>
    public record OnlineSalesModel : BaseNopEntityModel, ILocalizedModel<OnlineSalesLocalizedModel>
    {
        #region Ctor
        public OnlineSalesModel()
        {
            Locales = new List<OnlineSalesLocalizedModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.Picture")]
        [UIHint("Picture")]
        public int Picture { get; set; }


        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.Published")]
        public bool Published { get; set; }


        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<OnlineSalesLocalizedModel> Locales { get; set; }

        #endregion
    }

    public record OnlineSalesLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.Description")]
        public string Description { get; set; }
    }
}
