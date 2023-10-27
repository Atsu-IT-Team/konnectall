using Nop.Data.Mapping;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Data
{
    public class PluginNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string> 
        {
            { typeof(OnlineSales), PluginDefaults.OnlineSales_TableName}
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>()
        {

        };
    }
}
