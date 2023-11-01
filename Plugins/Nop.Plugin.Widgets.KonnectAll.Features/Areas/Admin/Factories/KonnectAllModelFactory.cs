using Microsoft.AspNetCore.Mvc.Rendering;
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
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.OnlineSales;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.ApplicationRequest;
using Nop.Core.Infrastructure;
using Nop.Core.Domain.Customers;
using System.IO;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Factories
{
    public class KonnectAllModelFactory : IKonnectAllModelFactory
    {
        #region Fields
        private readonly IKonnectAllService _konnectAllService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly INopFileProvider _fileProvider;
        private readonly IPictureService _pictureService;
        #endregion

        #region Ctor
        public KonnectAllModelFactory(IKonnectAllService konnectAllService,
            ILocalizedModelFactory localizedModelFactory,
            ILocalizationService localizationService,
            INopFileProvider fileProvider,
            IPictureService pictureService)
        {
            _konnectAllService = konnectAllService;
            _localizedModelFactory = localizedModelFactory;
            _localizationService = localizationService;
            _fileProvider = fileProvider;
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
            var model = new OnlineSalesListModel().PrepareToGrid(searchModel, onlineSales, () =>
            {
                return onlineSales.Select(os =>
                {
                    //fill in model values from the entity
                    var osm = os.ToModel<OnlineSalesModel>();

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

        #region Application Request Feature
        /// <summary>
        /// Prepare application request search model
        /// </summary>
        /// <param name="searchModel">Application request search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales search model
        /// </returns>
        public async Task<ApplicationRequestSearchModel> PrepareApplicationRequestSearchModelAsync(ApplicationRequestSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged application request list model
        /// </summary>
        /// <param name="searchModel">Application request search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales list model
        /// </returns>
        public async Task<ApplicationRequestListModel> PrepareApplicationRequestListModelAsync(ApplicationRequestSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get filtered application request
            var applicationRequests = await _konnectAllService.GetAllApplicationRequestAsync(firstName: searchModel.SearchFirstName,
                lastName: searchModel.SearchLastName,
                email: searchModel.SearchEmail,
                phone: searchModel.SearchPhone,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            //prepare grid model
            var model = new ApplicationRequestListModel().PrepareToGrid(searchModel, applicationRequests, () =>
            {
                return applicationRequests.Select(ar =>
                {
                    //fill in model values from the entity
                    var arm = ar.ToModel<ApplicationRequestModel>();
                    arm.FullName = ar.FirstName + " " + ar.LastName;
                    return arm;
                }).OrderByDescending(x => x.Id);
            });

            return model;
        }

        /// <summary>
        /// Prepare application request model
        /// </summary>
        /// <param name="model">Application request model</param>
        /// <param name="applicationRequest">Application request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales model
        /// </returns>
        public async Task<ApplicationRequestModel> PrepareApplicationRequestModelAsync(ApplicationRequestModel model, ApplicationRequest applicationRequest)
        {
            if (applicationRequest != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = applicationRequest.ToModel<ApplicationRequestModel>();
                }

                model.FullName = applicationRequest.FirstName + " " + applicationRequest.LastName;

                //load applicant resume
                var resumePath = _fileProvider.GetAbsolutePath(string.Format(PluginDefaults.ResumePath, applicationRequest.Id));
                var resumeDestinationPath = _fileProvider.Combine(_fileProvider.GetAbsolutePath(resumePath), applicationRequest.Resume);

                if (File.Exists(resumeDestinationPath))
                    model.ResumeLink = resumeDestinationPath;

                //load applicant other documents
                var documents = await _konnectAllService.GetAllDocumentsByApplicationIdAsync(applicationRequest.Id);
                foreach (var document in documents)
                {
                    var finalPath = _fileProvider.GetAbsolutePath(string.Format(PluginDefaults.DocumentsPath, applicationRequest.Id, model.FullName));
                    var destinationPath = _fileProvider.Combine(_fileProvider.GetAbsolutePath(finalPath), document.DocumentName);

                    if (File.Exists(destinationPath))
                    {
                        var doc = new ApplicationOtherDocumentsModel
                        {
                            Name = document.DocumentName,
                            DownloadLink = destinationPath
                        };

                        model.Documents.Add(doc);
                    }
                }
            }

            return model;
        }
        #endregion

        #endregion
    }
}
