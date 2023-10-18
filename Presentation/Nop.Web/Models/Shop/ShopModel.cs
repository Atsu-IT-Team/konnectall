using Nop.Web.Framework.Models;
using Nop.Web.Models.Shop;
using System.Collections.Generic;

namespace Nop.Web.Models.Catalog
{
    public partial record ShopModel : BaseNopEntityModel
    {
        public ShopModel()
        {
            Categories = new List<CategoryModel>();
            BannerCategories = new List<BannerCategoryModel>();
        }

        public IList<CategoryModel> Categories { get; set; }
        public IList<BannerCategoryModel> BannerCategories { get; set; }
    }
}