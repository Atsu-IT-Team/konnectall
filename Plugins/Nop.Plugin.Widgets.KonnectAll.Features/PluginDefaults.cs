namespace Nop.Plugin.Widgets.KonnectAll.Features
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    internal class PluginDefaults
    {
        #region Company specific settings
        public static string TablePrefix => "KA_";

        public static string KonnectAll_Root => "Plugin.Widgets.KonnectAll.Features.Admin.Root.Menu";
        #endregion

        /// <summary>
        /// Online sales features table name
        /// </summary>
        public static string OnlineSales_TableName => TablePrefix + "OnlineSales";

        /// <summary>
        /// Application request features table name
        /// </summary>
        public static string ApplicationRequest_TableName => TablePrefix + "ApplicationRequest";

        /// <summary>
        /// Application documents table name
        /// </summary>
        public static string ApplicationDocument_TableName => TablePrefix + "ApplicationDocuments";

        /// <summary>
        /// Commission table name
        /// </summary>
        public static string Commission_TableName => TablePrefix + "Commission";

        /// <summary>
        /// Online sales admin menu system name
        /// </summary>
        public static string OnlineSales_Menu_SystemName => "Plugin.Widgets.KonnectAll.Features.OnlineSales.Admin.Menu";

        /// <summary>
        /// Application request admin menu system name
        /// </summary>
        public static string ApplicationRequest_Menu_SystemName => "Plugin.Widgets.KonnectAll.Features.ApplicationRequest.Admin.Menu";

        /// <summary>
        /// Commission admin menu system name
        /// </summary>
        public static string Commission_Menu_SystemName => "Plugin.Widgets.KonnectAll.Features.Commission.Admin.Menu";

        /// <summary>
        /// Get the localize resource string path
        /// </summary>
        public static string LocalizationFilePath => @"/Plugins/Widgets.KonnectAll/Localization/ResourceString";

        /// <summary>
        /// Get the application's resume path
        /// </summary>
        public static string ResumePath => @"/files/applicant_resume/{0}";

        /// <summary>
        /// Get the application's documents path
        /// </summary>
        public static string DocumentsPath => @"/files/applicant_documents/{0}/{1}";

        /// <summary>
        /// Get resource string file name
        /// </summary>
        public static string ResourceStringFileName => "ResourceString_EN.xml";

    }
}
