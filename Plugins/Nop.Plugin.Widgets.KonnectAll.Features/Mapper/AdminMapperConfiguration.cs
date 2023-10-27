using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models;
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

            // Banner Map
            CreateMap<OnlineSales, OnlineSalesModel>()
                .ForMember(m => m.Locales, o => o.Ignore());

            CreateMap<OnlineSalesModel, OnlineSales>();

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
