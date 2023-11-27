using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.ApplicationRequest;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using Nop.Plugin.Widgets.KonnectAll.Features.Services;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Controllers
{
    public class ApplicationRequestController : BasePluginController
    {
        #region Fields
        private readonly IKonnectAllService _konnectAllService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly INopFileProvider _fileProvider;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        #endregion

        #region Ctor
        public ApplicationRequestController(IKonnectAllService konnectAllService,
            ILocalizationService localizationService,
            ILogger logger,
            INopFileProvider fileProvider,
            INotificationService notificationService,
            IPermissionService permissionService)
        {
            _konnectAllService = konnectAllService;
            _localizationService = localizationService;
            _logger = logger;
            _fileProvider = fileProvider;
            _notificationService = notificationService;
            _permissionService = permissionService;
        }

        #endregion

        #region Methods
        [HttpPost]
        public async Task<IActionResult> SubmitApplication(ApplicationRequestModel model, IFormFile uploadedFile, IFormCollection form)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var applicationReq = model.ToEntity<ApplicationRequest>();

                //insert application request entity
                await _konnectAllService.InsertApplicationRequestAsync(applicationReq);

                //  Get all files from Request object  

                if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
                {
                    try
                    {
                        //copy uploaded resume to destination
                        var path = _fileProvider.GetAbsolutePath(string.Format(PluginDefaults.ResumePath, applicationReq.Id.ToString()));
                        if (!_fileProvider.DirectoryExists(path))
                            _fileProvider.CreateDirectory(path);

                        // Get the complete folder path and store the file inside it.  
                        var fileName = _fileProvider.Combine(_fileProvider.GetAbsolutePath(path), uploadedFile.FileName);
                        using var fileStream = new FileStream(fileName, FileMode.Create);
                        uploadedFile.CopyTo(fileStream);

                        applicationReq.Resume = uploadedFile.FileName;

                        await _konnectAllService.UpdateApplicationRequestAsync(applicationReq);
                    }
                    catch (Exception ex)
                    {
                        await _logger.ErrorAsync(ex.Message);
                        ModelState.AddModelError("", ex.Message);
                        throw;
                    }
                }

                string applicantName = applicationReq.FirstName + " " + applicationReq.LastName;
                //copy other documents to destination
                IFormFileCollection files = form.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    IFormFile file = files[i];

                    if (file.Name.Equals("uploadedFile"))
                        continue;

                    var path = _fileProvider.GetAbsolutePath(string.Format(PluginDefaults.DocumentsPath, applicationReq.Id.ToString(), applicantName));
                    if (!_fileProvider.DirectoryExists(path))
                        _fileProvider.CreateDirectory(path);

                    // Get the complete folder path and store the file inside it.  
                    string fname = _fileProvider.Combine(_fileProvider.GetAbsolutePath(path), file.FileName);

                    using var fileStream = new FileStream(fname, FileMode.Create);
                    file.CopyTo(fileStream);

                    var doc = new ApplicationDocuments
                    {
                        ApplicationId = applicationReq.Id,
                        DocumentName = file.FileName,
                    };

                    await _konnectAllService.InsertApplicationDocumentAsync(doc);
                }

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.Submit.Success"));
            }

            var newComponentHtml = await RenderViewComponentToStringAsync("ApplicationRequest", new { widgetZone = PublicWidgetZones.JointThemTeam, additionalData = model });
            return Json(newComponentHtml);
        }
        #endregion
    }
}
