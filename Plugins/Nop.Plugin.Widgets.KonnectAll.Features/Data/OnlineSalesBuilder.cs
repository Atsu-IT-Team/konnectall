using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Data
{
    public class OnlineSalesBuilder : NopEntityBuilder<OnlineSales>
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(OnlineSales.Title)).AsString(400)
                .WithColumn(nameof(OnlineSales.Description)).AsString(2000)
                .WithColumn(nameof(OnlineSales.Picture)).AsInt32()
                .WithColumn(nameof(OnlineSales.DisplayOrder)).AsInt32()
                .WithColumn(nameof(OnlineSales.Published)).AsBoolean();
        }
    }
}
