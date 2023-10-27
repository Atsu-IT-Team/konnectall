using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Models
{
    public record OnlineSalesPublicInfoModel : BaseNopModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }
    }
}
