using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Media;
using Nop.Core.Domain.Discounts;
using Nop.Web.Models.Catalog;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;

namespace Nop.Web.Models.Discounts
{
    public partial record ProductsDealModel : BaseNopEntityModel
    {
        public ProductsDealModel()
        {
            Discount = new Discount();
            Products = new List<ProductOverviewModel>();
            AddToCart = new ProductDetailsModel.AddToCartModel();
            SelectedProduct = new ProductDetailsModel();
        }

        public Discount Discount { get; set; }
        public ProductDetailsModel SelectedProduct { get; set; }
        public IEnumerable<ProductOverviewModel> Products { get; set; }
        public ProductDetailsModel.AddToCartModel AddToCart { get; set; }


    }
}