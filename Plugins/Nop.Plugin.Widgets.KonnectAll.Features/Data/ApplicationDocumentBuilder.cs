using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using System.Data;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Data
{
    public class ApplicationDocumentBuilder : NopEntityBuilder<ApplicationDocuments>
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ApplicationDocuments.ApplicationId)).AsInt32().ForeignKey<ApplicationRequest>(onDelete: Rule.None).Nullable();
        }
    }
}
