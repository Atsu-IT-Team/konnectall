using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Domain
{
    /// <summary>
    /// Represents a KonnectAll features commission module
    /// </summary>
    public class Commission : BaseEntity
    {
        /// <summary>
        /// Gets or sets the order identity
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the vendor identity
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets the order total including tax
        /// </summary>
        public decimal OrderTotalInclTax { get; set; }

        /// <summary>
        /// Gets or sets the order total excluding tax
        /// </summary>
        public decimal OrderTotalExclTax { get; set; }

        /// <summary>
        /// Gets or sets the commission rate in(%)
        /// </summary>
        public decimal CommissionRate { get; set; }

        /// <summary>
        /// Gets or sets the commission amount
        /// </summary>
        public decimal CommissionAmount { get; set; }

        /// <summary>
        /// Gets or sets the create date & time
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
