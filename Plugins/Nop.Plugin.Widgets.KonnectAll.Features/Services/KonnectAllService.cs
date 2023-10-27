using Microsoft.CodeAnalysis.Operations;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Services
{
    public class KonnectAllService : IKonnectAllService
    {
        #region Fields
        private readonly IRepository<OnlineSales> _onlineSalesRepository;
        #endregion

        #region Ctor
        public KonnectAllService(IRepository<OnlineSales> onlineSalesRepository) 
        {
            _onlineSalesRepository = onlineSalesRepository;
        }

        #endregion

        #region Methods

        #region Online Sales Feature
        /// <summary>
        /// Delete onlineSales
        /// </summary>
        /// <param name="onlineSales">OnlineSales</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteOnlineSalesAsync(OnlineSales onlineSales) 
        {
            await _onlineSalesRepository.DeleteAsync(onlineSales);
        }

        /// <summary>
        /// Gets all onlineSales
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        public virtual async Task<IList<OnlineSales>> GetAllOnlineSalesAsync(bool showHidden = false) 
        {
            var records = await _onlineSalesRepository.GetAllAsync(query => 
            {
                if (!showHidden)
                    query = query.Where(c => c.Published);

                return query.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id);
            });

            return records.ToList();
        }

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
        public virtual async Task<IPagedList<OnlineSales>> GetAllOnlineSalesAsync(string title,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, bool? overridePublished = null)
        {
            var records = await _onlineSalesRepository.GetAllAsync(query =>
            {
                if (!showHidden)
                    query = query.Where(c => c.Published);
                else if (overridePublished.HasValue)
                    query = query.Where(c => c.Published == overridePublished.Value);

                if (!string.IsNullOrWhiteSpace(title))
                    query = query.Where(c => c.Title.Contains(title));

                return query.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id);
            });

            //paging
            return new PagedList<OnlineSales>(records, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a onlineSales
        /// </summary>
        /// <param name="onelinSalesId">OnlineSales identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        public virtual async Task<OnlineSales> GetOnlineSalesByIdAsync(int onelinSalesId) 
        {
            return await _onlineSalesRepository.GetByIdAsync(onelinSalesId);
        }

        /// <summary>
        /// Inserts onlineSales
        /// </summary>
        /// <param name="onlineSales">OnlineSales</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertOnlineSalesAsync(OnlineSales onlineSales) 
        {
            await _onlineSalesRepository.InsertAsync(onlineSales);
        }

        /// <summary>
        /// Updates the onlineSales
        /// </summary>
        /// <param name="onlineSales">OnlineSales</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateOnlineSalesAsync(OnlineSales onlineSales) 
        {
            await _onlineSalesRepository.UpdateAsync(onlineSales);
        }

        /// <summary>
        /// Delete a list of onlineSales
        /// </summary>
        /// <param name="onlineSales">Categories</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteOnlineSalesAsync(IList<OnlineSales> onlineSales) 
        {
            if (onlineSales == null)
                throw new ArgumentNullException(nameof(onlineSales));

            foreach (var item in onlineSales)
                await DeleteOnlineSalesAsync(item);
        }
        #endregion

        #endregion
    }
}
