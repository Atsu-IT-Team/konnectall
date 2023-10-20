using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework;
using Nop.Web.Models.Catalog;
using System.Linq;
using Nop.Web.Areas.Admin.Models.Catalog;
using System;
using Nop.Web.Areas.Admin.Factories;
using Nop.Core.Caching;
using Nop.Web.Infrastructure.Cache;
using Nop.Core.Domain.Media;
using Nop.Web.Models.Media;
using Nop.Services.Media;
using Nop.Web.Models.Shop;
using Nop.Services.Seo;

namespace Nop.Web.Controllers
{
    public class ShopController : BasePublicController
    {
        #region Fields

        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly ICategoryModelFactory _categoryModelFactory;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlRecordService _urlRecordService;

        #endregion

        #region Ctor

        public ShopController(
            ICatalogModelFactory catalogModelFactory,
            ICategoryService categoryService,
            IGenericAttributeService genericAttributeService,
            IPermissionService permissionService,
            IStoreContext storeContext,
            IWebHelper webHelper,
            IWorkContext workContext,
            ICategoryModelFactory categoryModelFactory,
            IStaticCacheManager staticCacheManager,
            IPictureService pictureService,
            MediaSettings mediaSettings,
            ILocalizationService localizationService,
            IUrlRecordService urlRecordService)
        {
            _catalogModelFactory = catalogModelFactory;
            _categoryService = categoryService;
            _genericAttributeService = genericAttributeService;
            _permissionService = permissionService;
            _storeContext = storeContext;
            _webHelper = webHelper;
            _workContext = workContext;
            _categoryModelFactory = categoryModelFactory;
            _staticCacheManager = staticCacheManager;
            _pictureService = pictureService;
            _mediaSettings = mediaSettings;
            _localizationService = localizationService;
            _urlRecordService = urlRecordService;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesDisplayedOnHomepageAsync();

            var bannerCategory = await _categoryService.GetAllCategoriesDisplayedAsBannerAsync();

            var store = await _storeContext.GetCurrentStoreAsync();

            //'Continue shopping' URL
            await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.LastContinueShoppingPageAttribute,
                _webHelper.GetThisPageUrl(false),
                store.Id);

            //display "edit" (manage) link
            bool manageEdit = await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel)
                && await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories);

            ShopModel model = new ShopModel();
            foreach (var item in bannerCategory)
            {
                var picture = await _pictureService.GetPictureByIdAsync(item.PictureId);
                string fullSizeImageUrl, imageUrl;

                (fullSizeImageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture);
                (imageUrl, _) = await _pictureService.GetPictureUrlAsync(picture, _mediaSettings.CategoryThumbPictureSize);

                var bc = new BannerCategoryModel
                {
                    CategoryId = item.Id,
                    Name = await _localizationService.GetLocalizedAsync(item, x => x.Name),
                    ShortDesc = await _localizationService.GetLocalizedAsync(item, x => x.Description),
                    SeName = await _urlRecordService.GetSeNameAsync(item),
                    ImageUrl = imageUrl
                };

                model.BannerCategories.Add(bc);
            }

            foreach (var cat in categories)
            {
                if (manageEdit)
                {
                    DisplayEditLink(Url.Action("Edit", "Category", new { id = cat.Id, area = AreaNames.Admin }));
                }

                var catModel = await _catalogModelFactory.PrepareCategoryModelAsync(cat, new CatalogProductsCommand());
                bool isSelectedCat = cat.Id == categories.FirstOrDefault()?.Id;

                if (isSelectedCat)
                {
                    catModel.CatalogProductsModel = await _catalogModelFactory.PrepareCategoryProductsModelAsync(cat
                        , new CatalogProductsCommand());
                }
                var currentStore = await _storeContext.GetCurrentStoreAsync();
                var pictureSize = _mediaSettings.CategoryThumbPictureSize;
                var categoryPictureCacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopModelCacheDefaults.CategoryPictureModelKey, cat,
                      pictureSize, true, await _workContext.GetWorkingLanguageAsync(), _webHelper.IsCurrentConnectionSecured(),
                      currentStore);

                catModel.PictureModel = await _staticCacheManager.GetAsync(categoryPictureCacheKey, async () =>
                {
                    var picture = await _pictureService.GetPictureByIdAsync(cat.PictureId);
                    string fullSizeImageUrl, imageUrl;

                    (fullSizeImageUrl, picture) = await _pictureService.GetPictureUrlAsync(picture);
                    (imageUrl, _) = await _pictureService.GetPictureUrlAsync(picture, pictureSize);

                    var pictureModel = new PictureModel
                    {
                        FullSizeImageUrl = fullSizeImageUrl,
                        ImageUrl = imageUrl,
                        Title = string.Format(await _localizationService
                            .GetResourceAsync("Media.Category.ImageLinkTitleFormat"), cat.Name),
                        AlternateText = string.Format(await _localizationService
                            .GetResourceAsync("Media.Category.ImageAlternateTextFormat"), cat.Name)
                    };

                    return pictureModel;
                });

                model.Categories.Add(catModel);
            }

            return View(model);
        }
        #region Products

        [HttpPost]
        public virtual async Task<IActionResult> ProductList(int categoryId)
        {
            CategoryProductSearchModel searchModel = new CategoryProductSearchModel();
            searchModel.CategoryId = categoryId;
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                return await AccessDeniedDataTablesJson();

            //try to get a category with the specified id
            var category = await _categoryService.GetCategoryByIdAsync(searchModel.CategoryId)
                ?? throw new ArgumentException("No category found with the specified id");

            //prepare model
            var model = await _categoryModelFactory.PrepareCategoryProductListModelAsync(searchModel, category);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> LoadDealOfTheDayComponent(int selectedProductId)
        {
            var newComponentHtml = await RenderViewComponentToStringAsync("HomepageDealOfDay", new { selectedProductId = selectedProductId });
            return Json(newComponentHtml);
        }



        [HttpPost]
        public virtual async Task<IActionResult> LoadCategoryProductsComponent(int selectedCategryId)
        {
            var newComponentHtml = await RenderViewComponentToStringAsync("HomepageProducts", new { categoryId = selectedCategryId });
            return Json(newComponentHtml);
        }

        #endregion

    }
}
