using Microsoft.AspNetCore.Routing;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Plugins;
using Nop.Web.Framework.Menu;
using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.KonnectAll.Features
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class PluginProvider : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly INopFileProvider _fileProvider;
        private readonly IRepository<Language> _languageRepository;
        #endregion

        #region Ctor
        public PluginProvider(ILocalizationService localizationService,
            ILogger logger,
            INopFileProvider fileProvider,
            IRepository<Language> languageRepository)
        {
            _localizationService = localizationService;
            _logger = logger;
            _fileProvider = fileProvider;
            _languageRepository = languageRepository;
        }


        #endregion

        #region Utlities
        /// <summary>
        /// install resource strings
        /// </summary>
        protected async Task InstallLocalResourcesAsync()
        {

            //'English' language
            var enLanguage = await _languageRepository.Table.FirstOrDefaultAsync(l => (l.Name == "EN" || l.LanguageCulture.ToLower() == "") && (l.Published));

            //save resources
            if (enLanguage != null)
            {
                foreach (var filePath in Directory.EnumerateFiles(_fileProvider.MapPath(_fileProvider.GetVirtualPath(PluginDefaults.LocalizationFilePath)),
                PluginDefaults.ResourceStringFileName, SearchOption.TopDirectoryOnly))
                {
                    //var localesXml = File.ReadAllText(filePath);
                    //localizationService.ImportResourcesFromXml(enLanguage, localesXml);
                    using (var streamReader = new StreamReader(filePath))
                    {
                        await _localizationService.ImportResourcesFromXmlAsync(enLanguage, streamReader);
                    }
                }
            }
        }

        ///<summry>
        ///Delete Resource String
        ///</summry>
        protected async Task DeleteLocalResourcesAsync()
        {
            var file = Path.Combine(_fileProvider.MapPath(_fileProvider.GetVirtualPath(PluginDefaults.LocalizationFilePath)), PluginDefaults.ResourceStringFileName);
            if (!string.IsNullOrEmpty(file))
            {
                var languageResourceNames = from name in XDocument.Load(file).Document.Descendants("LocaleResource")
                                            select name.Attribute("Name").Value;

                foreach (var item in languageResourceNames)
                {
                    await _localizationService.DeleteLocaleResourcesAsync(item);
                }
            }
        }
        #endregion

        #region Methods
        public string GetWidgetViewComponentName(string widgetZone)
        {
            if (widgetZone == null)
                throw new ArgumentNullException(nameof(widgetZone));

            if (widgetZone.Equals(PublicWidgetZones.GetToKnowUs))
                return "OnlineSales";

            if (widgetZone.Equals(PublicWidgetZones.JointThemTeam))
                return "ApplicationRequest";

            return string.Empty;
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { 
                PublicWidgetZones.GetToKnowUs,
                PublicWidgetZones.JointThemTeam
            });
        }

        #region Install/Uninstall

        /// <summary>
        /// Install plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            //Logic during installation goes here...
            await InstallLocalResourcesAsync();

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            await DeleteLocalResourcesAsync();

            await base.UninstallAsync();
        }

        #endregion

        #region Sitemap

        /// <summary>
        /// Manage sitemap. You can use "SystemName" of menu items to manage existing sitemap or add a new menu item.
        /// </summary>
        /// <param name="rootNode">Root node of the sitemap.</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {

            var osPluginMenu = new SiteMapNode
            {
                SystemName = PluginDefaults.KonnectAll_Root,
                Title = await _localizationService.GetResourceAsync(PluginDefaults.KonnectAll_Root),
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } },
                ChildNodes = new List<SiteMapNode>()
                {
                    new SiteMapNode()
                    {
                        SystemName = PluginDefaults.OnlineSales_Menu_SystemName,
                        Title = await _localizationService.GetResourceAsync(PluginDefaults.OnlineSales_Menu_SystemName),
                        ControllerName = "KonnectAllFeatures",
                        ActionName = "List",
                        IconClass = "nav-icon far fa-dot-circle",
                        Visible = true,
                        RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
                    },
                    new SiteMapNode()
                    {
                        SystemName = PluginDefaults.ApplicationRequest_Menu_SystemName,
                        Title = await _localizationService.GetResourceAsync(PluginDefaults.ApplicationRequest_Menu_SystemName),
                        ControllerName = "KonnectAllFeatures",
                        ActionName = "ApplicationList",
                        IconClass = "nav-icon far fa-dot-circle",
                        Visible = true,
                        RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
                    }
                }
            };
            rootNode.ChildNodes.Add(osPluginMenu);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;
        #endregion

        #endregion

    }
}