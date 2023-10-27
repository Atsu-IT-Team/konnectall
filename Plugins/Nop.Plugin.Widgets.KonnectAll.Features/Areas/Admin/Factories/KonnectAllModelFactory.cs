using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using Nop.Plugin.Widgets.KonnectAll.Features.Services;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Nop.Web.Framework.Factories;
using System.Collections.Generic;
using Nop.Plugin.Widgets.KonnectAll.Features.Models;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Factories
{
    public class KonnectAllModelFactory : IKonnectAllModelFactory
    {
        #region Fields
        private readonly IKonnectAllService _konnectAllService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        #endregion

        #region Ctor
        public KonnectAllModelFactory(IKonnectAllService konnectAllService,
            ILocalizedModelFactory localizedModelFactory,
            ILocalizationService localizationService,
            IPictureService pictureService) 
        {
            _konnectAllService = konnectAllService;
            _localizedModelFactory = localizedModelFactory;
            _localizationService = localizationService;
            _pictureService = pictureService;
        }
        #endregion

        #region Methods

        #region Online Sales Features
        /// <summary>
        /// Prepare public info model
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the banner search model
        /// </returns>
        public async Task<IList<OnlineSalesPublicInfoModel>> PreparePublicInfoModelAsync() 
        {
            var osList = await _konnectAllService.GetAllOnlineSalesAsync();

            if (osList == null)
                return null;

            var model = await osList.SelectAwait(async onlineSales => 
            {
                var os = new OnlineSalesPublicInfoModel
                {
                    Title = await _localizationService.GetLocalizedAsync(onlineSales, x => x.Title),
                    Description = await _localizationService.GetLocalizedAsync(onlineSales, x => x.Description)
                };

                var picture = (await _pictureService.GetPictureByIdAsync(onlineSales.Picture))
                        ?? throw new Exception("Picture cannot be loaded");
                os.PictureUrl = (await _pictureService.GetPictureUrlAsync(picture)).Url;

                return os;
            }).ToListAsync();

            return model;
        }

        /// <summary>
        /// Prepare onlineSales search model
        /// </summary>
        /// <param name="searchModel">OnlineSales search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales search model
        /// </returns>
        public async Task<OnlineSalesSearchModel> PrepareOnlineSalesSearchModelAsync(OnlineSalesSearchModel searchModel) 
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare "published" filter (0 - all; 1 - published only; 2 - unpublished only)
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.OnlineSales.Search.Field.Published.All")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "1",
                Text = await _localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.OnlineSales.Search.Field.Published.ActiveOnly")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "2",
                Text = await _localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.OnlineSales.Search.Field.Published.InActiveOnly")
            });

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged onlineSales list model
        /// </summary>
        /// <param name="searchModel">OnlineSales search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales list model
        /// </returns>
        public async Task<OnlineSalesListModel> PrepareOnlineSalesListModelAsync(OnlineSalesSearchModel searchModel) 
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get online sales
            var onlineSales = await _konnectAllService.GetAllOnlineSalesAsync(title: searchModel.Title,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
                overridePublished: searchModel.PublishedId == 0 ? null : (bool?)(searchModel.PublishedId == 1));

            //prepare grid model
            var model = await new OnlineSalesListModel().PrepareToGridAsync(searchModel, onlineSales, () =>
            {
                return onlineSales.SelectAwait(async os =>
                {
                    //fill in model values from the entity
                    var osm = os.ToModel<OnlineSalesModel>();

                    //fill in additional values (not existing in the entity)
                    var picture = (await _pictureService.GetPictureByIdAsync(os.Picture))
                        ?? throw new Exception("Picture cannot be loaded");

                    return osm;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare onlineSales model
        /// </summary>
        /// <param name="model">OnlineSales model</param>
        /// <param name="onlineSales">OnlineSales</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales model
        /// </returns>
        public async Task<OnlineSalesModel> PrepareOnlineSalesModelAsync(OnlineSalesModel model, OnlineSales onlineSales, bool excludeProperties = false) 
        {
            Func<OnlineSalesLocalizedModel, int, Task> localizedModelConfiguration = null;

            if (onlineSales != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = onlineSales.ToModel<OnlineSalesModel>();
                }

                //define localized model configuration action
                localizedModelConfiguration = async (locale, languageId) =>
                {
                    locale.Title = await _localizationService.GetLocalizedAsync(onlineSales, entity => entity.Title, languageId, false, false);
                    locale.Description = await _localizationService.GetLocalizedAsync(onlineSales, entity => entity.Description, languageId, false, false);
                };
            }

            //set default values for the new model
            if (onlineSales == null)
                model.Published = true;

            //prepare localized models
            if (!excludeProperties)
                model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

            return model;
        }
        #endregion

        #endregion
    }
}
