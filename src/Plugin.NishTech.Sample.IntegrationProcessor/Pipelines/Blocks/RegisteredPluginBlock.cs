﻿namespace Plugin.NishTech.Sample.IntegrationProcessor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    [PipelineDisplayName("IntegrationProcessor.block.registeredplugin")]
    public class RegisteredPluginBlock : PipelineBlock<IEnumerable<RegisteredPluginModel>, IEnumerable<RegisteredPluginModel>, CommercePipelineExecutionContext>
    {
        public override Task<IEnumerable<RegisteredPluginModel>> Run(IEnumerable<RegisteredPluginModel> arg, CommercePipelineExecutionContext context)
        {
            if (arg == null)
                return Task.FromResult(arg);
            List<RegisteredPluginModel> list = arg.ToList();
            PluginHelper.RegisterPlugin((object)this, list);
            return Task.FromResult(list.AsEnumerable());
        }

        public RegisteredPluginBlock()
          : base((string)null)
        {
        }
    }
}
