using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Data
{
    [NopMigration("2023/10/26 09:40:23:6455432", "Widgets.KonnectAll base schema")]
    public class SchemaMigration : AutoReversingMigration
    {
        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            Create.TableFor<OnlineSales>();
            Create.TableFor<ApplicationRequest>();
            Create.TableFor<ApplicationDocuments>();
            Create.TableFor<Commission>();
        }
    }
}
