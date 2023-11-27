using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Vendors;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using Nop.Services.Logging;
using Nop.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Services
{
    public class KonnectAllService : IKonnectAllService
    {
        #region Fields
        private readonly IRepository<ApplicationRequest> _appRequestRepository;
        private readonly IRepository<ApplicationDocuments> _appDocumentsRepository;
        private readonly IRepository<Commission> _commissionRepository;
        private readonly IRepository<OnlineSales> _onlineSalesRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly ILogger _logger;
        private readonly INopFileProvider _fileProvider;
        private readonly IOrderService _orderService;
        #endregion

        #region Ctor
        public KonnectAllService(IRepository<ApplicationRequest> appRequestRepository,
            IRepository<ApplicationDocuments> appDocumentsRepository,
            IRepository<Commission> commissionRepository,
            IRepository<OnlineSales> onlineSalesRepository,
            IRepository<Product> productRepository,
            IRepository<Vendor> vendorRepository,
            ILogger logger,
            INopFileProvider fileProvider,
            IOrderService orderService) 
        {
            _appRequestRepository = appRequestRepository;
            _appDocumentsRepository = appDocumentsRepository;
            _commissionRepository = commissionRepository;
            _onlineSalesRepository = onlineSalesRepository;
            _productRepository = productRepository;
            _vendorRepository = vendorRepository;
            _logger = logger;
            _fileProvider = fileProvider;
            _orderService = orderService;
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

        #region Employment Application Feature

        #region Application request

        /// <summary>
        /// Delete employment application request
        /// </summary>
        /// <param name="appRequest">Employment application</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteApplicationRequestAsync(ApplicationRequest appRequest) 
        {
            //remove application documents
            var documents = await _appDocumentsRepository.Table.Where(x => x.ApplicationId == appRequest.Id).ToListAsync();
            foreach (var item in documents)
            {
                await DeleteApplicationDocumentAsync(item);
            }

            //remove application request
            await _appRequestRepository.DeleteAsync(appRequest);
        }

        /// <summary>
        /// Gets all Application request
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the application request
        /// </returns>
        public virtual async Task<IList<ApplicationRequest>> GetAllApplicationRequestAsync() 
        {
            return await GetAllApplicationRequestAsync(string.Empty, string.Empty, string.Empty, string.Empty);
        }

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
        public virtual async Task<IPagedList<ApplicationRequest>> GetAllApplicationRequestAsync(string firstName = null,
            string lastName = null,
            string email = null,
            string phone = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var applications = await _appRequestRepository.GetAllAsync(async query =>
            {
                if (!string.IsNullOrEmpty(firstName))
                    query.Where(x => x.FirstName.Contains(firstName));

                if (!string.IsNullOrEmpty(lastName))
                    query.Where(x => x.LastName.Contains(lastName));

                if (!string.IsNullOrEmpty(email))
                    query.Where(x => x.Email.Contains(email));

                if (!string.IsNullOrEmpty(phone))
                    query.Where(x => x.Phone.Contains(phone));

                return query.OrderByDescending(x => x.CreatedOnUtc);
            });

            //paging
            return new PagedList<ApplicationRequest>(applications, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a application request
        /// </summary>
        /// <param name="appRequestId">Application request identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        public virtual async Task<ApplicationRequest> GetApplicationRequestByIdAsync(int appRequestId) 
        {
            return await _appRequestRepository.GetByIdAsync(appRequestId);
        }

        /// <summary>
        /// Insert employment application request
        /// </summary>
        /// <param name="appRequest">Employment application</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertApplicationRequestAsync(ApplicationRequest appRequest) 
        {
            await _appRequestRepository.InsertAsync(appRequest);
        }

        /// <summary>
        /// Update employment application request
        /// </summary>
        /// <param name="appRequest">Employment application</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateApplicationRequestAsync(ApplicationRequest appRequest) 
        {
            await _appRequestRepository.UpdateAsync(appRequest);
        }


        /// <summary>
        /// Delete a list of application requests
        /// </summary>
        /// <param name="appRequestList">Application request collection</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteApplicationRequestsAsync(IList<ApplicationRequest> appRequestList) 
        {
            foreach (var item in appRequestList)
                await DeleteApplicationRequestAsync(item);
        }
        #endregion

        #region Application documents
        /// <summary>
        /// Delete application document
        /// </summary>
        /// <param name="appDocument">Employment application documents</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteApplicationDocumentAsync(ApplicationDocuments appDocument) 
        {
            var appRequest = await GetApplicationRequestByIdAsync(appDocument.ApplicationId);
            string applicantName = appRequest.FirstName + " " + appRequest.LastName;
            var directoryPath = string.Format(PluginDefaults.DocumentsPath, appDocument.ApplicationId, applicantName);
            if (_fileProvider.DirectoryExists(directoryPath))
                _fileProvider.DeleteDirectory(directoryPath);

            await _appDocumentsRepository.DeleteAsync(appDocument);
        }

        /// <summary>
        /// Gets all applicant's documents request
        /// </summary>
        /// <param name="appRequestId">Application request identity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the application request
        /// </returns>
        public virtual async Task<IList<ApplicationDocuments>> GetAllDocumentsByApplicationIdAsync(int appRequestId) 
        {
            return await _appDocumentsRepository.Table.Where(x => x.ApplicationId == appRequestId).ToListAsync();
        }

        /// <summary>
        /// Insert application document
        /// </summary>
        /// <param name="appDocument">Employment application documents</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertApplicationDocumentAsync(ApplicationDocuments appDocument) 
        {
            await _appDocumentsRepository.InsertAsync(appDocument);
        }
        #endregion

        #endregion

        #region Commission
        /// <summary>
        /// Delete commission
        /// </summary>
        /// <param name="commission">Commission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteCommissionAsync(Commission commission) 
        {
            await _commissionRepository.DeleteAsync(commission);
        }

        /// <summary>
        /// Gets all commission
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        public virtual async Task<IList<Commission>> GetAllCommissionAsync() 
        {
            var records = await _commissionRepository.GetAllAsync(query =>
            {
                return query.OrderBy(x => x.Id);
            });

            return records.ToList();
        }

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
        public virtual async Task<IPagedList<Commission>> GetAllCommissionAsync(int vendorId,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var records = await _commissionRepository.GetAllAsync(query =>
            {
                if (vendorId > 0)
                    query = query.Where(c => c.VendorId == vendorId);

                return query.OrderBy(x => x.Id);
            });

            //paging
            return new PagedList<Commission>(records, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a commission
        /// </summary>
        /// <param name="commissionId">Commission identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        public virtual async Task<Commission> GetCommissionByIdAsync(int commissionId) 
        {
            return await _commissionRepository.GetByIdAsync(commissionId);
        }

        /// <summary>
        /// Gets a commission by order and vendor
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the onlineSales
        /// </returns>
        public virtual async Task<decimal> GetCommissionByOrderIdAndVendorAsync(int orderId, int vendorId)
        {
            var commission = (from c in _commissionRepository.Table
                               where c.OrderId == orderId &&
                               (vendorId > 0 ? c.VendorId == vendorId : true)
                               group c by new { c.OrderId } into g
                               let total = (g.Sum(x => x.CommissionAmount))
                              select total);

            return await commission.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts commission
        /// </summary>
        /// <param name="commission">Commission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertCommissionAsync(Commission commission) 
        {
            await _commissionRepository.InsertAsync(commission);
        }

        /// <summary>
        /// Updates commission
        /// </summary>
        /// <param name="commission">Commission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateCommissionAsync(Commission commission) 
        {
            await _commissionRepository.UpdateAsync(commission);
        }

        public virtual async Task<bool> ConfirmOrderProductsByVendor(Order order, int[] itemIds) 
        {
            bool result = true;

            try
            {
                await _orderService.UpdateOrderAsync(order);

                var orderItems = await _orderService.GetOrderItemsAsync(order.Id);

                orderItems = orderItems.Where(x => itemIds.Contains(x.Id)).ToList();

                var productIds = orderItems.Select(x => x.ProductId).Distinct().ToList();

                var query = from oi in orderItems
                            join p in _productRepository.Table on oi.ProductId equals p.Id
                            join v in _vendorRepository.Table on p.VendorId equals v.Id
                            group oi by new { oi.OrderId, v } into g
                            select new
                            {
                                OrderId = g.Key.OrderId,
                                Vendor = g.Key.v,
                                PriceInclTax = g.Sum(x => x.PriceInclTax),
                                PriceExclTax = g.Sum(x => x.PriceExclTax)
                            };

                foreach (var item in query)
                {
                    var commission = new Commission
                    {
                        OrderId = item.OrderId,
                        VendorId = item.Vendor.Id,
                        CommissionRate = item.Vendor.Commission,
                        OrderTotalInclTax = item.PriceInclTax,
                        OrderTotalExclTax = item.PriceExclTax,
                        CommissionAmount = ((item.PriceExclTax * item.Vendor.Commission) / 100),
                        CreatedOnUtc = DateTime.UtcNow
                    };

                    await InsertCommissionAsync(commission);
                }

                foreach (var item in orderItems)
                {
                    item.Confirmed = true;
                    await _orderService.UpdateOrderItemAsync(item);
                }

            }
            catch (Exception ex)
            {
                result = false;
                await _logger.ErrorAsync(ex.Message);
            }
            return result;
        }

        #endregion

        #endregion
    }
}
