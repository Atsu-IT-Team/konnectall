using Nop.Core;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Services
{
    /// <summary>
    /// OnlineSales service interface
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
    }
}
