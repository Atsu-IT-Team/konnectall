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
        /// Online sales admin menu system name
        /// </summary>
        public static string OnlineSales_Menu_SystemName => "Plugin.Widgets.KonnectAll.Features.OnlineSales.Admin.Menu";

        /// <summary>
        /// Get the localize resource string path
        /// </summary>
        public static string LocalizationFilePath => @"/Plugins/Widgets.KonnectAll/Localization/ResourceString";

        /// <summary>
        /// Get resource string file name
        /// </summary>
        public static string ResourceStringFileName => "ResourceString_EN.xml";

    }
}
