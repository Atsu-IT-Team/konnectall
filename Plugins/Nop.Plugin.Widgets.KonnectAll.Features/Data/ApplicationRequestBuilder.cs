using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Data
{
    public class ApplicationRequestBuilder : NopEntityBuilder<ApplicationRequest>
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ApplicationRequest.FirstName)).AsString(200)
                .WithColumn(nameof(ApplicationRequest.LastName)).AsString(200)
                .WithColumn(nameof(ApplicationRequest.Email)).AsString(300)
                .WithColumn(nameof(ApplicationRequest.Phone)).AsString(50)
                .WithColumn(nameof(ApplicationRequest.Resume)).AsString(200).Nullable();
        }
    }
}
