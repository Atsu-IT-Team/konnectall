using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Factories;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using Nop.Plugin.Widgets.KonnectAll.Features.Services;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using System.Reflection;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class KonnectAllFeaturesController : BasePluginController
    {
        #region Fields
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IKonnectAllModelFactory _konnectAllModelFactory;
        private readonly IKonnectAllService _konnectAllService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;

        #endregion

        #region Ctor
        public KonnectAllFeaturesController(ICustomerActivityService customerActivityService,
            IKonnectAllModelFactory konnectAllModelFactory,
            IKonnectAllService konnectAllService,
            ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IPictureService pictureService)
        {
            _customerActivityService = customerActivityService;
            _konnectAllModelFactory = konnectAllModelFactory;
            _konnectAllService = konnectAllService;
            _localizedEntityService = localizedEntityService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
        }
        #endregion

        #region Utilities
        protected virtual async Task UpdateLocalesAsync(OnlineSales onlineSales, OnlineSalesModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(onlineSales,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(onlineSales,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Methods

        #region List
        public async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //prepare model
            var model = await _konnectAllModelFactory.PrepareOnlineSalesSearchModelAsync(new OnlineSalesSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(OnlineSalesSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _konnectAllModelFactory.PrepareOnlineSalesListModelAsync(searchModel);

            return Json(model);
        }
        #endregion

        #region Create / Edit / Delete
        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //prepare model
            var model = await _konnectAllModelFactory.PrepareOnlineSalesModelAsync(new OnlineSalesModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(OnlineSalesModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var os = model.ToEntity<OnlineSales>();
                await _konnectAllService.InsertOnlineSalesAsync(os);

                //locales
                await UpdateLocalesAsync(os, model);
                
                //activity log
                await _customerActivityService.InsertActivityAsync("AddNewOnlineSales",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewOnlineSales"), os.Title), os);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.OnlineSales.Admin.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = os.Id });
            }

            //prepare model
            model = await _konnectAllModelFactory.PrepareOnlineSalesModelAsync(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //try to get a online sales with the specified id
            var os = await _konnectAllService.GetOnlineSalesByIdAsync(id);
            if (os == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _konnectAllModelFactory.PrepareOnlineSalesModelAsync(null, os);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(OnlineSalesModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //try to get a banner with the specified id
            var os = await _konnectAllService.GetOnlineSalesByIdAsync(model.Id);
            if (os == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                os = model.ToEntity<OnlineSales>();
                
                await _konnectAllService.UpdateOnlineSalesAsync(os);

                //locales
                await UpdateLocalesAsync(os, model);

                //activity log
                await _customerActivityService.InsertActivityAsync("EditOnlineSales",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditOnlineSales"), os.Title), os);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.OnlineSales.Admin.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = os.Id });
            }

            //prepare model
            model = await _konnectAllModelFactory.PrepareOnlineSalesModelAsync(model, os, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //try to get a banner with the specified id
            var os = await _konnectAllService.GetOnlineSalesByIdAsync(id);
            if (os == null)
                return RedirectToAction("List");

            await _konnectAllService.DeleteOnlineSalesAsync(os);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteOnlineSales",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteOnlineSales"), os.Title), os);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.OnlineSales.Admin.Deleted"));

            return new NullJsonResult();
        }
        #endregion

        #endregion
    }
}
