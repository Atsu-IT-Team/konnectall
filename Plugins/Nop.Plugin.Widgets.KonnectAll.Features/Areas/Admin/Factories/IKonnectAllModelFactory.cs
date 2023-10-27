using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using Nop.Plugin.Widgets.KonnectAll.Features.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Factories
{
    public interface IKonnectAllModelFactory
    {
        #region Online Sales Feature

        /// <summary>
        /// Prepare public info model
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the banner search model
        /// </returns>
        Task<IList<OnlineSalesPublicInfoModel>> PreparePublicInfoModelAsync();

        /// <summary>
        /// Prepare onlineSales search model
        /// </summary>
        /// <param name="searchModel">OnlineSales search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales search model
        /// </returns>
        Task<OnlineSalesSearchModel> PrepareOnlineSalesSearchModelAsync(OnlineSalesSearchModel searchModel);

        /// <summary>
        /// Prepare paged onlineSales list model
        /// </summary>
        /// <param name="searchModel">OnlineSales search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales list model
        /// </returns>
        Task<OnlineSalesListModel> PrepareOnlineSalesListModelAsync(OnlineSalesSearchModel searchModel);

        /// <summary>
        /// Prepare onlineSales model
        /// </summary>
        /// <param name="model">OnlineSales model</param>
        /// <param name="onlineSales">OnlineSales</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales model
        /// </returns>
        Task<OnlineSalesModel> PrepareOnlineSalesModelAsync(OnlineSalesModel model, OnlineSales onlineSales, bool excludeProperties = false);
        #endregion
    }
}
