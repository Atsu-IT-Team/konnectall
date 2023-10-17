namespace Konnectall.Plugin.Payment
{
    using Nop.Core.Configuration;

    /// <summary>
    /// Represents settings of manual payment plugin
    /// </summary>
    public class IyzicoSettings : ISettings
    {
        /// <summary>
        /// Api key
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// Secret key
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// Api base url
        /// </summary>
        public string Uri { get; set; }
        public string CallbackUrl { get; set; }
        public string CustomerIdentityAttributeKey { get; set; }
    }
}
