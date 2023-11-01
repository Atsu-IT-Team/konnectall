using Nop.Core;
using System;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Domain
{
    /// <summary>
    /// Represents a KonnectAll features application request record
    /// </summary>
    public class ApplicationRequest : BaseEntity
    {
        /// <summary>
        /// Gets or sets the first name of applicant
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of applicant
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets an email of applicant
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of applicant
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the resume of applicant
        /// </summary>
        public string Resume { get; set; }

        /// <summary>
        /// Gets or sets the record creation datetime
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
