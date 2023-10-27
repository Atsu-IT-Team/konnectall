using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Domain
{
    /// <summary>
    /// Represents a KonnectAll features online sales record
    /// </summary>
    public class OnlineSales : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the title of online sales
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of online sales
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int Picture { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }
    }
}
