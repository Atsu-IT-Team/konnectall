using Nop.Core;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Domain
{
    /// <summary>
    /// Represents a KonnectAll features application request other documents record
    /// </summary>
    public class ApplicationDocuments : BaseEntity
    {
        /// <summary>
        /// Gets or sets the application request identity
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the other documents of applicant
        /// </summary>
        public string DocumentName { get; set; }
    }
}
