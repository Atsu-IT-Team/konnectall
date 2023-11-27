using Nop.Data.Mapping;
using Nop.Plugin.Widgets.KonnectAll.Features.Domain;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Data
{
    public class PluginNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string> 
        {
            { typeof(OnlineSales), PluginDefaults.OnlineSales_TableName },
            { typeof(ApplicationRequest), PluginDefaults.ApplicationRequest_TableName},
            { typeof(ApplicationDocuments), PluginDefaults.ApplicationDocument_TableName},
            { typeof(Commission), PluginDefaults.Commission_TableName}
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>()
        {

        };
    }
}
