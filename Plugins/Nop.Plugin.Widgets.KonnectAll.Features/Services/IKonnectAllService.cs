using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Services
{
    /// <summary>
    /// Konnect all features service interface
    /// </summary>
    public interface IKonnectAllService
    {
        #region Online Sales Feature
        /// <summary>
        /// Delete onlineSales
        /// </summary>
        /// <param name="onlineSales">OnlineSales</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteOnlineSalesAsync(OnlineSales onlineSales);

        /// <summary>
        /// Gets all onlineSales
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        Task<IList<OnlineSales>> GetAllOnlineSalesAsync(bool showHidden = false);

        /// <summary>
        /// Gets all onlineSales
        /// </summary>
        /// <param name="title">OnlineSales Title</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="overridePublished">
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" sales
        /// false - load only "Unpublished" sales
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        Task<IPagedList<OnlineSales>> GetAllOnlineSalesAsync(string title, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, bool? overridePublished = null);

        /// <summary>
        /// Gets a onlineSales
        /// </summary>
        /// <param name="onelinSalesId">OnlineSales identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        Task<OnlineSales> GetOnlineSalesByIdAsync(int onelinSalesId);

        /// <summary>
        /// Inserts onlineSales
        /// </summary>
        /// <param name="onlineSales">OnlineSales</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertOnlineSalesAsync(OnlineSales onlineSales);

        /// <summary>
        /// Updates the onlineSales
        /// </summary>
        /// <param name="onlineSales">OnlineSales</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateOnlineSalesAsync(OnlineSales onlineSales);


        /// <summary>
        /// Delete a list of onlineSales
        /// </summary>
        /// <param name="onlineSales">Categories</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteOnlineSalesAsync(IList<OnlineSales> onlineSales);
        #endregion

        #region Employment Application Feature

        #region Application request
        /// <summary>
        /// Delete employment application request
        /// </summary>
        /// <param name="appRequest">Employment application</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteApplicationRequestAsync(ApplicationRequest appRequest);

        /// <summary>
        /// Gets all Application request
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the application request
        /// </returns>
        Task<IList<ApplicationRequest>> GetAllApplicationRequestAsync();

        /// <summary>
        /// Gets all applications
        /// </summary>
        /// <param name="firstName">Applicant's FirstName</param>
        /// <param name="lastName">Applicant's FirstName</param>
        /// <param name="email">Applicant's Email</param>
        /// <param name="phone">Applicant's Phone</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" sales
        /// false - load only "Unpublished" sales
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the application request
        /// </returns>
        Task<IPagedList<ApplicationRequest>> GetAllApplicationRequestAsync(string firstName = null,
            string lastName = null,
            string email = null,
            string phone = null,
            int pageIndex = 0, 
            int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a application request
        /// </summary>
        /// <param name="appRequestId">Application request identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        Task<ApplicationRequest> GetApplicationRequestByIdAsync(int appRequestId);

        /// <summary>
        /// Insert employment application request
        /// </summary>
        /// <param name="appRequest">Employment application</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertApplicationRequestAsync(ApplicationRequest appRequest);

        /// <summary>
        /// Update employment application request
        /// </summary>
        /// <param name="appRequest">Employment application</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateApplicationRequestAsync(ApplicationRequest appRequest);


        /// <summary>
        /// Delete a list of application requests
        /// </summary>
        /// <param name="appRequestList">Application request collection</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteApplicationRequestsAsync(IList<ApplicationRequest> appRequestList);
        #endregion

        #region Application documents
        /// <summary>
        /// Delete application document
        /// </summary>
        /// <param name="appDocument">Employment application documents</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteApplicationDocumentAsync(ApplicationDocuments appDocument);

        /// <summary>
        /// Gets all applicant's documents request
        /// </summary>
        /// <param name="appRequestId">Application request identity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the application request
        /// </returns>
        Task<IList<ApplicationDocuments>> GetAllDocumentsByApplicationIdAsync(int appRequestId);

        /// <summary>
        /// Insert application document
        /// </summary>
        /// <param name="appDocument">Employment application documents</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertApplicationDocumentAsync(ApplicationDocuments appDocument);
        #endregion

        #endregion

        #region Commission
        /// <summary>
        /// Delete commission
        /// </summary>
        /// <param name="commission">Commission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteCommissionAsync(Commission commission);

        /// <summary>
        /// Gets all commission
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        Task<IList<Commission>> GetAllCommissionAsync();

        /// <summary>
        /// Gets all commission
        /// </summary>
        /// <param name="vendorId">Vendor identity</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        Task<IPagedList<Commission>> GetAllCommissionAsync(int vendorId,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a commission
        /// </summary>
        /// <param name="commissionId">Commission identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        Task<Commission> GetCommissionByIdAsync(int commissionId);

        /// <summary>
        /// Gets a commission by order and vendor
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        Task<decimal> GetCommissionByOrderIdAndVendorAsync(int orderId, int vendorId);

        /// <summary>
        /// Inserts commission
        /// </summary>
        /// <param name="commission">Commission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertCommissionAsync(Commission commission);

        /// <summary>
        /// Updates commission
        /// </summary>
        /// <param name="commission">Commission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateCommissionAsync(Commission commission);

        Task<bool> ConfirmOrderProductsByVendor(Order order, int[] itemIds);
        #endregion
    }
}
