using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.ApplicationRequest;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.OnlineSales;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Mapper
{
    /// <summary>
    /// Admin entity and model mapper
    /// </summary>
    public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AdminMapperConfiguration()
        {
            CreateMap();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Create Product Files Settings map 
        /// Entity -> Model and Model -> Entity
        /// </summary>
        protected virtual void CreateMap()
        {
            #region OnlineSales

            CreateMap<OnlineSales, OnlineSalesModel>()
                .ForMember(m => m.Locales, o => o.Ignore());

            CreateMap<OnlineSalesModel, OnlineSales>();

            #endregion

            #region Application Request

            CreateMap<ApplicationRequest, ApplicationRequestModel>()
                .ForMember(m => m.FullName, o => o.Ignore())
                .ForMember(m => m.ResumeLink, o => o.Ignore())
                .ForMember(m => m.Documents, o => o.Ignore());

            CreateMap<ApplicationRequestModel, ApplicationRequest>()
                .ForMember(m => m.CreatedOnUtc, o => o.Ignore());

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;

        #endregion
    }
}
