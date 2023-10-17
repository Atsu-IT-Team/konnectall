using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public partial record ShopModel : BaseNopEntityModel
    {
        public ShopModel()
        {
            Categories = new List<CategoryModel>();

        }

        public IList<CategoryModel> Categories { get; set; }

  
    }
}