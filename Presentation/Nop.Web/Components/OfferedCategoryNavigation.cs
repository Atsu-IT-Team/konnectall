using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Media;
using Nop.Services.Catalog;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Components
{
    public class OfferedCategoryNavigationViewComponent : NopViewComponent
    {
        #region Fields
        private readonly MediaSettings _mediaSettings;
        private readonly ICategoryService _categoryService;
        private readonly IDiscountService _discountService;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly IUrlRecordService _urlRecordService;
        #endregion

        #region Ctor
        public OfferedCategoryNavigationViewComponent(MediaSettings mediaSettings,
            ICategoryService categoryService,
            IDiscountService discountService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IUrlRecordService urlRecordService) 
        {
            _mediaSettings = mediaSettings;
            _categoryService = categoryService;
            _discountService = discountService;
            _localizationService = localizationService;
            _pictureService = pictureService;
            _urlRecordService = urlRecordService;
        }

        #endregion

        #region Methods
        public async Task<IViewComponentResult> InvokeAsync(int currentCategoryId, int currentProductId)
        {
            var discounts = await _discountService.GetAllDiscountsAsync(DiscountType.AssignedToCategories);

            if (discounts == null || discounts.Count == 0)
                return Content("");

            List<Category> categories = new List<Category>();

            foreach (var item in discounts)
            {
                var category = await _categoryService.GetCategoriesByAppliedDiscountAsync(item.Id);

                if (category != null && category.Count > 0)
                {
                    if (categories == null || categories.Count == 0)
                    {
                        categories.AddRange(category);
                        continue;
                    }
                    else 
                    {
                        var diffCate = categories.Where(x => !category.Contains(x)).ToList();
                        categories.AddRange(diffCate);
                    }
                }
            }

            if (categories == null || categories.Count == 0)
                return Content("");

            List<OfferedCategoryModel> model = new List<OfferedCategoryModel>();
            foreach (var item in categories)
            {
                var picture = await _pictureService.GetPictureByIdAsync(item.PictureId);
                string fullSizeImageUrl, imageUrl;

                (fullSizeImageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture);
                (imageUrl, _) = await _pictureService.GetPictureUrlAsync(picture, _mediaSettings.CategoryThumbPictureSize);

                var cat = new OfferedCategoryModel
                {
                    CategoryId = item.Id,
                    Name = await _localizationService.GetLocalizedAsync(item, x => x.Name),
                    SeName = await _urlRecordService.GetSeNameAsync(item),
                    ImageUrl = imageUrl
                };

                model.Add(cat);
            }
            return View(model);
        }
        #endregion
    }
}
