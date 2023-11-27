using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Vendors;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using System.Data;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Data
{
    public class CommissionBuilder : NopEntityBuilder<Commission>
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Commission.OrderId)).AsInt32().ForeignKey<Order>(onDelete: Rule.None).Nullable()
                .WithColumn(nameof(Commission.VendorId)).AsInt32().ForeignKey<Vendor>(onDelete: Rule.None).Nullable()
                .WithColumn(nameof(Commission.OrderTotalInclTax)).AsDecimal(18, 4).WithDefaultValue(0.0000)
                .WithColumn(nameof(Commission.OrderTotalExclTax)).AsDecimal(18, 4).WithDefaultValue(0.0000)
                .WithColumn(nameof(Commission.CommissionRate)).AsDecimal(18, 4).WithDefaultValue(0.0000)
                .WithColumn(nameof(Commission.CommissionAmount)).AsDecimal(18, 4).WithDefaultValue(0.0000);
        }
    }
}
